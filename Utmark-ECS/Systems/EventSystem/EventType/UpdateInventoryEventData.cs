using Utmark_ECS.Components;

namespace Utmark_ECS.Systems.EventSystem.EventType
{
    public class UpdateInventoryEventData
    {
        public ItemComponent Item { get; set; }

        public UpdateInventoryEventData(ItemComponent Items)
        {
            Item = Items;
        }

    }
}
