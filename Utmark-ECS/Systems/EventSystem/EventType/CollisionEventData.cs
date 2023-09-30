using Microsoft.Xna.Framework;
using Utmark_ECS.Entities;

namespace Utmark_ECS.Systems.EventSystem.EventType
{
    public class CollisionEventData
    {
        public Entity EntityA { get; }
        public Entity EntityB { get; }

        public Vector2 Position { get; }
        // ... any other relevant properties, e.g. collision point

        public CollisionEventData(Entity entityA, Entity entityB, Vector2 position /*, ... other parameters */)
        {
            EntityA = entityA;
            EntityB = entityB;
            Position = position;
            // ... initialize other properties
        }
    }

}
