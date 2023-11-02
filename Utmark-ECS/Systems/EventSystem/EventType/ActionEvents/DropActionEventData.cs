using Microsoft.Xna.Framework;
using Utmark_ECS.Entities;

namespace Utmark_ECS.Systems.EventSystem.EventType.ActionEvents
{
    public class DropActionEventData
    {
        public Entity Entity;
        public Entity Item;
        public Vector2 Position;
        public DropActionEventData(Entity entity, Entity item, Vector2 position)
        {
            Entity = entity;
            Item = item;
            Position = position;
        }

    }
}