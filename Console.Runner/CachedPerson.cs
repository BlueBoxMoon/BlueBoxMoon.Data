
using BlueBoxMoon.Data.EntityFramework;
using BlueBoxMoon.Data.EntityFramework.Common.Cache;

namespace Console.Runner
{
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
