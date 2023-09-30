using Microsoft.Xna.Framework;
using Utmark_ECS.Entities;

namespace Utmark_ECS.Systems.EventSystem.EventType
{
    public class EntityRemoveData
    {
        public Entity Entity { get; }
        public Vector2 Position { get; }

        public EntityRemoveData(Entity entity, Vector2 position)
        {
            Entity = entity;
            Position = position;
        }

    }

}
