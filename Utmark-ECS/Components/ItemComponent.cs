using Utmark_ECS.Intefaces;
using static Utmark_ECS.Enums.ItemTypeEnum;

namespace Utmark_ECS.Components
{
    public class ItemComponent : IComponent
    {
        public ItemType ItemType { get; set; }

        public ItemComponent(ItemType itemType)
        {
            ItemType = itemType;
        }
    }
}
