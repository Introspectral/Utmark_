using Utmark_ECS.Components;
using Utmark_ECS.Entities;
using Utmark_ECS.Managers;
using Utmark_ECS.Systems.EventSystem;
using Utmark_ECS.Systems.EventSystem.EventType;

namespace Utmark_ECS.Systems.EventHandlers
{
    public class ActionHandler
    {
        private readonly EventManager _eventManager;
        private ComponentManager _componentManager;
        private InventorySystem _inventorySystem;
        public ActionHandler(EventManager eventManager, ComponentManager componentManager, InventorySystem inventorySystem)
        {
            _eventManager = eventManager;
            _componentManager = componentManager;

            _eventManager.Subscribe<PickUpActionEvent>(OnPickUp);
            _inventorySystem = inventorySystem;

            //_eventManager.Subscribe<PickUpActionEvent>(OnUse);
            //_eventManager.Subscribe<PickUpActionEvent>(OnThrow);
        }


        //private void OnThrow(PickUpActionEvent @event)
        //{
        //    throw new NotImplementedException();
        //}

        //private void OnUse(PickUpActionEvent @event)
        //{
        //    throw new NotImplementedException();
        //}



        private void OnPickUp(PickUpActionEvent data)
        {
            var picker = data.Picker;
            var playerName = _componentManager.GetComponent<NameComponent>(picker);
          
            
            if (data.Item != null)
            {

                var item = data.Item;
                if (picker != item && IsItem(item))
                {
                    var itemName = _componentManager.GetComponent<ItemComponent>(item);
                    _inventorySystem.AddItem(picker, itemName);
                    _eventManager.Publish(new InventoryUpdate(itemName));
                    _eventManager.Publish(new EntityRemoveData(item, data.Position));
                    _eventManager.Publish(new MessageEvent(this, $"[color=green]*[/color] [color=red]{playerName.Name}[/color] picked up a [color=blue]{itemName.Name}[/color]"));
                }

            }
            if (picker == null) { _eventManager.Publish(new MessageEvent(this, $"No Picker")); }


        }
        private bool IsItem(Entity entity) =>
            _componentManager.GetComponentsForEntity(entity).Any(component => component is ItemComponent);

    }
}


