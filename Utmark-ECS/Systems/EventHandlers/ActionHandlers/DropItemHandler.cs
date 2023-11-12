using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using Utmark_ECS.Components;
using Utmark_ECS.Intefaces;
using Utmark_ECS.Managers;
using Utmark_ECS.Systems.EventSystem.EventType;
using Utmark_ECS.Systems.EventSystem.EventType.ActionEvents;

namespace Utmark_ECS.Systems.EventHandlers.ActionHandlers
{
    public class DropItemHandler : ISystem
    {
        private InventorySystem _inventorySystem;
        private ComponentManager _componentManager;
        private EventManager _eventManager;
        public DropItemHandler(EventManager eventManager, ComponentManager componentManager, InventorySystem inventorySystem)
        {
            _eventManager = eventManager;
            _componentManager = componentManager;
            _inventorySystem = inventorySystem;
            _eventManager.Subscribe<DropItemEventData>(OnDrop);
            _eventManager.Subscribe<PopulateEntityEventData>(OnPopulateEntity);


        }

        private void OnPopulateEntity(PopulateEntityEventData data)
        {

            _componentManager.AddComponent(data._entity, new PositionComponent(data._position));

            var entityName = _componentManager.GetComponent<NameComponent>(data._entity);
            _eventManager.Publish(new MessageEventData(this, $"You found a {entityName.Name}"));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
        }

        public void Update(GameTime gameTime)
        {
        }

        private void OnDrop(DropItemEventData data)
        {
            var inventory = _componentManager.GetComponent<InventoryComponent>(data.Entity);
            var itemToDrop = inventory.Items.FirstOrDefault();
            foreach (var item in inventory.Items) { Debug.WriteLine(item); }
            if (itemToDrop != null)
            {
                _eventManager.Publish(new MessageEventData(this, $"Drops a {itemToDrop}"));
                _componentManager.AddComponent(itemToDrop, new PositionComponent(data.Position));
                _inventorySystem.DropItem(data.Entity, itemToDrop);

            }
            else
            {
                _eventManager.Publish(new MessageEventData(this, $"Inventory is empty"));
            }
        }
    }
}
