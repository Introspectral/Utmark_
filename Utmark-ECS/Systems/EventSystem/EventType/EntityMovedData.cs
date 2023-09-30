using Microsoft.Xna.Framework;
using Utmark_ECS.Entities;

namespace Utmark_ECS.Systems.EventSystem.EventType
{
    public class EntityMovedData
    {
        public Entity Entity { get; }
        public Vector2 OldPosition { get; }
        public Vector2 NewPosition { get; }

        public EntityMovedData(Entity entity, Vector2 oldPosition, Vector2 newPosition)
        {
            Entity = entity;
            OldPosition = oldPosition;
            NewPosition = newPosition;

        }

    }
}
