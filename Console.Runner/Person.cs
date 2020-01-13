using BlueBoxMoon.Data.EntityFramework;

namespace Console.Runner
{
    public class Person : Entity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
