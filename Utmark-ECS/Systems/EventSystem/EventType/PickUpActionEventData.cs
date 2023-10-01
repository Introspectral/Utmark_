using Utmark_ECS.Entities;

namespace Utmark_ECS.Systems.EventSystem.EventType
{
    public class PickUpActionEventData
    {
        public Entity Picker { get; }
        public Entity Item { get; }
        // Other necessary properties...

        public PickUpActionEventData(Entity picker, Entity item)
        {
            Picker = picker;
            Item = item;
        }
    }
}
