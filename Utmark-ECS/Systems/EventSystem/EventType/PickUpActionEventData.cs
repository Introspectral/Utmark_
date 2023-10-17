
using Microsoft.Xna.Framework;
using Utmark_ECS.Entities;

namespace Utmark_ECS.Systems.EventSystem.EventType
{
    public class PickUpActionEventData
    {
        public Entity Picker { get; }
        public Entity Item { get; }

        public Vector2 Position { get; }
        // Other necessary properties...

        public PickUpActionEventData(Entity picker, Entity item, Vector2 position)
        {
            Picker = picker;
            Item = item;
            Position = position;
        }
    }
}
