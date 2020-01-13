using BlueBoxMoon.Data.EntityFramework;

namespace Console.Runner
{
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
