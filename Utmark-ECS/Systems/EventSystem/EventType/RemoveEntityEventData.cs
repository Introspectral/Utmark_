using Microsoft.Xna.Framework;
using Utmark_ECS.Entities;

namespace Utmark_ECS.Systems.EventSystem.EventType
{
    public class RemoveEntityEventData
    {
        public Entity Entity { get; }
        public Vector2 Position { get; }

        public RemoveEntityEventData(Entity entity, Vector2 position)
        {
            Entity = entity;
            Position = position;
        }

    }

}
