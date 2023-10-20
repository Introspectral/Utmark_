using Utmark_ECS.Components;
using Utmark_ECS.Entities;
using Utmark_ECS.Enums;
using Utmark_ECS.Managers;
using Utmark_ECS.Systems.EventSystem.EventType;
using Utmark_ECS.Systems.EventSystem.EventType.ActionEvents;

namespace Utmark_ECS.Systems.EventHandlers
{
    public class ActionHandler
    {
        private readonly EventManager _eventManager;
        private ComponentManager _componentManager;
        private InventorySystem _inventorySystem;
        private RandomMessagePicker _randomMessagePicker;
        public ActionHandler(EventManager eventManager, ComponentManager componentManager, InventorySystem inventorySystem)
        {
            _eventManager = eventManager;
            _componentManager = componentManager;
            _eventManager.Subscribe<ActionEventData>(OnActionEvent);
            _eventManager.Subscribe<LookActionEventData>(OnLook);
            //_eventManager.Subscribe<SearchActionEventData>(OnSearch);
            //_eventManager.Subscribe<UseActionEventData>(OnUse);
            _eventManager.Subscribe<PickUpActionEventData>(OnPickUp);
            _inventorySystem = inventorySystem;
        }

        private void OnActionEvent(ActionEventData data)
        {
            var possition = _componentManager.GetComponent<PositionComponent>(data.Entity);
            if (possition != null)
            {
                if (data.State is InputAction)
                {
                    switch (data.State)
                    {
                        case InputAction.Look:
                            _eventManager.Publish(new LookRequestEventData(data.Entity, possition.Position));
                            return;
                        case InputAction.Search:
                            _eventManager.Publish(new SearchRequestEventData(data.Entity, possition.Position));
                            return;
                        case InputAction.Use:
                            _eventManager.Publish(new UseRequestEventData(data.Entity));
                            return;
                        case InputAction.PickUp:
                            _eventManager.Publish(new PickUpRequestEventData(data.Entity, possition.Position));
                            return;
                    }
                }
            }
        }

        private void OnLook(LookActionEventData data)
        {

            foreach (var item in data.Entities)
            {
                try
                {
                    var npcName = _componentManager.GetComponent<NameComponent>(item);
                    if (npcName != null)
                    {
                        _eventManager.Publish(new MessageEvent(this, $"[color=green]*[/color] You see a {npcName.Name}"));
                    }
                }
                catch { }
                try
                {
                    var itemName = _componentManager.GetComponent<ItemComponent>(item);
                    if (itemName != null)
                    {
                        _eventManager.Publish(new MessageEvent(this, $"[color=green]*[/color] You see a [color=blue]{itemName.Name}[/color]: {itemName.Description}"));
                    }

                }
                catch { }
            }
        }
        private void OnSearch(SearchActionEventData data)
        {

        }
        private void OnUse(UseActionEventData data)
        {

        }
        private void OnPickUp(PickUpActionEventData data)
        {
            var picker = data.Picker;
            var playerName = _componentManager.GetComponent<NameComponent>(picker);
            if (data.Item != null)
            {

                var item = data.Item;
                if (IsItem(item))
                {
                    var itemPossition = _componentManager.GetComponent<PositionComponent>(item);
                    var itemName = _componentManager.GetComponent<ItemComponent>(item);
                    _inventorySystem.AddItem(picker, itemName);

                    _eventManager.Publish(new RemoveEntityEventData(item, itemPossition.Position));
                    _eventManager.Publish(new MessageEvent(this, $"[color=green]*[/color] [color=red]{playerName.Name}[/color] picked up a [color=blue]{itemName.Name}[/color]"));
                }
                else
                {
                    _eventManager.Publish(new MessageEvent(this, $"[color=red]*[/color] You can not do that"));
                }
            }


            if (picker == null) { _eventManager.Publish(new MessageEvent(this, $"No Picker")); }


        }
        private bool IsItem(Entity entity) =>
            _componentManager.GetComponentsForEntity(entity).Any(component => component is ItemComponent);
    }
}


