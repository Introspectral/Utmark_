using Utmark_ECS.Intefaces;
using static Utmark_ECS.Enums.ItemTypeEnum;

namespace Utmark_ECS.Components
{
    public class ItemComponent : IComponent
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ItemType ItemType { get; set; }

        public ItemComponent(string name, string description, ItemType itemType)
        {
            Name = name;
            Description = description;
            ItemType = itemType;
        }
    }
}
