using System.ComponentModel.DataAnnotations.Schema;

using BlueBoxMoon.Data.EntityFramework;

namespace Console.Runner
{
    [Table( "People", Schema = "testSchema" )]
    public class Person : Entity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }

    public class PersonDataSet : DataSet<Person>
    {
        public PersonDataSet( EntityDbContext context )
            : base( context )
        {
        }
    }
}
