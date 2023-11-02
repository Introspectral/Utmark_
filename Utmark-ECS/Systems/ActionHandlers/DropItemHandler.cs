using GoRogue.DiceNotation.Terms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utmark_ECS.Components;
using Utmark_ECS.Enums;
using Utmark_ECS.Intefaces;
using Utmark_ECS.Managers;
using Utmark_ECS.Systems.EventSystem.EventType;
using Utmark_ECS.Systems.EventSystem.EventType.ActionEvents;

namespace Utmark_ECS.Systems.ActionHandlers
{
    public class DropItemHandler 
    {
        private EntityManager _entityManager;
        private InventorySystem _inventorySystem;   
        private ComponentManager _componentManager;
        private EventManager _eventManager;
        public DropItemHandler(EntityManager entityManager, EventManager eventManager, ComponentManager componentManager, InventorySystem inventorySystem)
        {
            _entityManager = entityManager;
            _eventManager = eventManager;
            _componentManager = componentManager;
            _inventorySystem = inventorySystem;
            _eventManager.Subscribe<DropItemRequestEventData>(OnDrop);      
        }


        private void OnDrop(DropItemRequestEventData data)
        {
            var inventory = _componentManager.GetComponent<InventoryComponent>(data.Entity);
            var itemToDrop = inventory.Items.FirstOrDefault();
            foreach( var item in inventory.Items ) { Debug.WriteLine(item); }
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
