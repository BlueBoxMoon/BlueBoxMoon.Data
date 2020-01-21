
using BlueBoxMoon.Data.EntityFramework;

namespace Console.Runner
{
    public class PersonDataSet : DataSet<Person>
    {
        public PersonDataSet( EntityDbContext context )
            : base( context )
        {
        }
    }
}
