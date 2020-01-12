namespace BlueBoxMoon.Data.EntityFramework.Sqlite
{
    public static class ModelDbContextOptionsBuilderExtensions
    {
        /// <summary>
        /// Configures the model database to use the Sqlite provider.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <returns></returns>
        public static ModelDbContextOptionsBuilder UseSqlite( this ModelDbContextOptionsBuilder optionsBuilder )
            => optionsBuilder.UseDatabaseProviderFeatures<SqliteModelDatabaseFeatures>();
    }
}
