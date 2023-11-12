using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Utmark_ECS.Components;
using Utmark_ECS.Entities;
using Utmark_ECS.Intefaces;
using Utmark_ECS.Managers;

namespace Utmark_ECS.Systems
{
    public class InventorySystem : ISystem
    {
        private readonly ComponentManager _componentManager;
        public InventorySystem(ComponentManager componentManager)
        {
            _componentManager = componentManager;

        }
        private InventoryComponent? GetInventory(Entity entity)
        {
            return _componentManager.GetComponent<InventoryComponent>(entity);
        }


        public void AddItem(Entity entity, Entity item)
        {
            var inventory = GetInventory(entity);
            if (inventory == null)
            {
                Console.WriteLine("Error: Entity does not have an InventoryComponent");
                return;
            }
            inventory.Items.Add(item);
        }

        public void RemoveItem(Entity entity, Entity item)
        {
            var inventory = GetInventory(entity);
            if (inventory == null)
            {
                Console.WriteLine("Error: Entity does not have an InventoryComponent");
                return;
            }
            inventory.Items.Remove(item);
        }

        public void UseItem(Entity entity, Entity itemtem)
        {
            var inventory = GetInventory(entity);
            if (inventory == null)
            {
                Console.WriteLine("Error: Entity does not have an InventoryComponent");
                return;
            }
        }
        public void DropItem(Entity entity, Entity item)
        {
            var inventory = GetInventory(entity);
            if (inventory == null)
            {
                Console.WriteLine("Error: Entity does not have an InventoryComponent");
                return;
            }

            inventory.Items.Remove(item);
        }

        public void Draw(SpriteBatch spriteBatch) { }

        public void Update(GameTime gameTime) { }

    }
}
