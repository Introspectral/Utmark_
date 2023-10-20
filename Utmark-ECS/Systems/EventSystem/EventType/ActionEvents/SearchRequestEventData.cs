using Microsoft.Xna.Framework;
using Utmark_ECS.Entities;

namespace Utmark_ECS.Systems.EventSystem.EventType.ActionEvents
{
    internal class SearchRequestEventData
    {
        public Entity Entity;
        public Vector2 Position;
        public SearchRequestEventData(Entity entity, Vector2 position)
        {
            Entity = entity;
            Position = position;
        }
    }
}