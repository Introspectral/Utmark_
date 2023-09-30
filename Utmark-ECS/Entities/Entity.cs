using System.Numerics;

namespace Utmark_ECS.Entities
{
    public class Entity
    {
        public Guid ID { get; private set; }
        public Entity()
        {
            ID = Guid.NewGuid();

        }
    }
}
