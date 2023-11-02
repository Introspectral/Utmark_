using Utmark_ECS.Entities;
using Utmark_ECS.Intefaces;

namespace Utmark_ECS.Components
{
    public class InventoryComponent : IComponent
    {
        public List<Entity> Items { get; set; } = new List<Entity>();
        public int MaxSize { get; set; }
    }
}
