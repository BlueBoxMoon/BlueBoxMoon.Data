using System.ComponentModel.DataAnnotations.Schema;

using BlueBoxMoon.Data.EntityFramework;
using BlueBoxMoon.Data.EntityFramework.Common.Cache;

namespace Console.Runner
{
    [Table( "People", Schema = "testSchema" )]
    public class Person : Entity
    {
        public string FirstName
        {
            get => ( string ) GetValue();
            set => SetValue( value );
        }

        public string LastName
        {
            get => ( string ) GetValue();
            set => SetValue( value );
        }
    }

    public class PersonDataSet : DataSet<Person>
    {
        public PersonDataSet( EntityDbContext context )
            : base( context )
        {
        }
    }

    public class CachedPerson : CachedEntity
    {
        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public override void UpdateFromEntity( IEntity entity )
        {
            base.UpdateFromEntity( entity );

            var person = ( Person ) entity;

            FirstName = person.FirstName;
            LastName = person.LastName;
        }
    }
}
