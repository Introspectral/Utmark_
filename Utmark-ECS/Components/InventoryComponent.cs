using Utmark_ECS.Intefaces;

namespace Utmark_ECS.Components
{
    public class InventoryComponent : IComponent
    {
        public List<ItemComponent> Items { get; set; } = new List<ItemComponent>();


    }
}
