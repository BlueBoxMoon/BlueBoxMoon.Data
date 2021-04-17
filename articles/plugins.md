# Plugins

The core of the entire system is the ability to load plugins to provide all the useful features. Each plugin might depend on other plugins. This allows you to consume various plugins without having to know all the requirements of how to set up the plugin.

When building your own functionality I recommend you build it as if it were a plugin, even if it is the functionality of your core application. While you can technically build all your migrations and dependency tracking yourself, treating your core logic as if it were a plugin allows you to take advantage of everything built-in to do that for you.

## Defining a Plugin

A plugin consists of a class that inherits from `EntityPlugin`. Your implementation then provides a few details that identify your plugin to the system as well as the user (if your application chooses to expose what plugins are installed).

Your plugin will probably also consist of one or more migrations.

## Migrations

A migration operates much like any other Entity Framework migration, though you need to inherit from `EntityMigration` and also adorn your implementation with the `PluginMigration` attribute. These provide a few additional features over their standard Entity Framework counterparts.

For the most part, you make your changes to the database as you normally would. There are a few helpful extension methods you can use to make your life easier, such as `CreateEntityTable()`, or `AutoIncrement()`. These are meant to take some most time consuming parts and do them for you. Since this is meant to allow your migrations to be database agnostic, the `AutoIncrement()` extension for column operations will automatically tag the column with the correct database provider's information to make it an auto-incrementing column.

The `PluginMigration` attribute takes two parameters. The first is a version string for what version of your plugin this migration was introduced in. For example, if you already have released version 1.0 of your plugin and you are now adding a new table that will be in version 2.0, you would pass `"2.0"` as the version string. Note: The version string can have 4 total integer segments. The second parameter is optional, but you should get in the habit of providing it. This is the migration step number. Suppose in version 2.0 you end up with 3 migrations before you finish putting everything in for the release. You would use steps `1`, `2`, and `3`. These steps define the order in which your migrations run for a specific version.

Now, you might be wondering why you have to define all this version stuff instead of just a simple incrementing migration identifier. The answer is because there is another class attribute you can apply to your migration: `DependsOnPlugin`. This attribute allows you to setup proper dependency on another plugin. Your NuGet package would have the DLL dependency taking care of, but you need to make sure your migrations run after the migrations of the plugin you depend on. This attribute takes 3 parameters. The `Type` that identifies the plugin, and then the version number and (optionally) the step number. If the feature you need to ensure exists in the database before your migration run is in version 1.0.0 of the plugin, then you would specify `"1.0.0"` and leave the step blank. If you don't provide a step that effectively means "make sure all steps for that version number have run".

The step number comes in handy if you have two plugins that both depend on each other. They both might add features in version 2.0.0 but PluginA needs to run steps 1-3 before PluginB can run its first migration step. Then PluginA might need PluginB to run migration step 1 before it can run its own step 4. You can configure this interdependence easily.

## Consuming a Plugin

Following the pattern set forth in Entity Framework Core already, consuming a plugin is very straight forward. For example:

```cs
serviceCollection.AddEntityDbContext<MyContext>( options =>
{
    options.UseSqlite();
}, entityOptions =>
{
    entityOptions.UseSqlite();
    entityOptions.WithPlugin<PluginA>();
} );
```

Okay, there is a little more to it than that. But that is all that is required to register your plugin - assuming your plugin doesn't actually do anything. In reality, you probably want to create an extension method for the `EntityDbContextOptionsBuilder` class. For example, the Facets plugin creates an extension method called `UseFacets()`. That method takes care of all initialization, such as ensuring that the Cache plugin has been requested, registering it's own settings into the service provider, calling the `WithPlugin()` and `WithEntity()` methods to register itself and its custom entity types.
