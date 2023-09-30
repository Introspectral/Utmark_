using Utmark_ECS.Components;
using Utmark_ECS.Entities;
using Utmark_ECS.Managers;

namespace Utmark_ECS.Systems
{
    public class InventorySystem
    {
        private readonly ComponentManager _componentManager;

        public InventorySystem(ComponentManager componentManager)
        {
            _componentManager = componentManager;
        }

        public void AddItem(Entity entity, ItemComponent item)
        {
            var inventory = _componentManager.GetComponent<InventoryComponent>(entity);
            if (inventory == null)
            {
                Console.WriteLine("Error: Entity does not have an InventoryComponent");
                return;
            }

            inventory.Items.Add(item);
        }

        public void RemoveItem(Entity entity, ItemComponent item)
        {
            var inventory = _componentManager.GetComponent<InventoryComponent>(entity);
            if (inventory == null)
            {
                Console.WriteLine("Error: Entity does not have an InventoryComponent");
                return;
            }
            inventory.Items.Remove(item);
        }

        public void UseItem(Entity entity, ItemComponent item)
        {

            var inventory = _componentManager.GetComponent<InventoryComponent>(entity);
            if (inventory == null)
            {
                Console.WriteLine("Error: Entity does not have an InventoryComponent");
                return;
            }
            // Implement logic for using items here. E.g. Apply effects, modify entity properties etc.
        }
        public void DropItem(Entity entity, ItemComponent item)
        {
            var inventory = _componentManager.GetComponent<InventoryComponent>(entity);
            if (inventory == null)
            {
                Console.WriteLine("Error: Entity does not have an InventoryComponent");
                return;
            }
            inventory.Items.Remove(item);
        }
    }
}
