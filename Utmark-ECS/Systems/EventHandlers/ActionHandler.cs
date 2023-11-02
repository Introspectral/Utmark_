using Utmark_ECS.Components;
using Utmark_ECS.Entities;
using Utmark_ECS.Enums;
using Utmark_ECS.Managers;
using Utmark_ECS.Systems.EventSystem.EventType;
using Utmark_ECS.Systems.EventSystem.EventType.ActionEvents;
using Utmark_ECS.Utilities;

namespace Utmark_ECS.Systems.EventHandlers
{
    // TODO: Action Handling - This will be Updated lated on to contain available actions based on the context of the player; Near water? Drink.Near Fire and has a sausage and a stick? Make Hotdog etc.
    public class ActionHandler
    {
        private readonly EventManager _eventManager;
        private ComponentManager _componentManager;
        private InventorySystem _inventorySystem;
        public ActionHandler(EventManager eventManager, ComponentManager componentManager, InventorySystem inventorySystem)
        {
            _eventManager = eventManager;
            _componentManager = componentManager;
            _eventManager.Subscribe<ActionEventData>(OnActionEvent);
            _eventManager.Subscribe<LookActionEventData>(OnLook);
            _eventManager.Subscribe<PickUpActionEventData>(OnPickUp);
            //_eventManager.Subscribe<SearchActionEventData>(OnSearch);
            //_eventManager.Subscribe<UseActionEventData>(OnUse);

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
                        case InputAction.Inventory:
                            _eventManager.Publish(new OpenInventoryEventData(data.Entity));
                            return;
                        case InputAction.Drop:
                            _eventManager.Publish(new DropItemRequestEventData(data.Entity, possition.Position));
                            return;

                    }
                }
            }
        }

        private void OnLook(LookActionEventData data)
        {
            // Assuming 'data.Entities' is a collection of entity IDs.
            foreach (var entityId in data.Entities)
            {
                // We can avoid the try-catch by checking the existence of the component beforehand.
                if (_componentManager.TryGetComponent(entityId, out NpcComponent npcName))
                {
                    // Publish message for NPCs (entities with a NameComponent).
                    _eventManager.Publish(new MessageEventData(this, $"[color=green]*[/color] You see a {npcName.GetType}"));
                }
                // The same approach can be used for items.
                if (_componentManager.TryGetComponent(entityId, out ItemComponent itemComponent))
                {
                    continue;
                }
                // Publish message for items with both name and description.
                _eventManager.Publish(new MessageEventData(this, $"[color=green]*[/color] You see a [color=blue]{itemComponent}[/color]: {itemComponent}"));
            }
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
                    
                    _inventorySystem.AddItem(picker, item);
                    _eventManager.Publish(new MessageEventData(this, $"[color=green]*[/color] [color=red]{playerName.Name}[/color] picked up a [color=blue]{item}[/color]"));
                    _componentManager.RemoveComponent<PositionComponent>(item);
                }
                else
                {
                    _eventManager.Publish(new MessageEventData(this, $"[color=red]*[/color] You can not do that"));
                }
            }
            if (picker == null) { _eventManager.Publish(new MessageEventData(this, $"No Picker")); }
        }


        private void OnSearch(SearchActionEventData data)
        {

        }
        private void OnUse(UseActionEventData data)
        {

        }
        private void OnDrop(DropActionEventData data)
        {

        }

        private bool IsItem(Entities.Entity entity) =>
            _componentManager.GetComponentsForEntity(entity).Any(component => component is Components.ItemComponent);
    }
}


