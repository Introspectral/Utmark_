using Microsoft.Xna.Framework;
using Utmark_ECS.Entities;

namespace Utmark_ECS.Systems.EventSystem.EventType.ActionEvents
{
    public class DropItemRequestEventData
    {
        public Entity Entity;
        public Vector2 Position;
        public DropItemRequestEventData(Entity entity, Vector2 position)
        {
            Entity = entity;
            Position = position;
        }
    }
}