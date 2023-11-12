using Utmark_ECS.Components;
using Utmark_ECS.Enums;
using Utmark_ECS.Managers;
using Utmark_ECS.Systems.EventSystem.EventType;
using Utmark_ECS.Systems.EventSystem.EventType.ActionEvents;

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

                        case InputAction.Drop:
                            _eventManager.Publish(new DropItemEventData(data.Entity, possition.Position));
                            return;

                    }
                }
            }
        }

        private void OnLook(LookActionEventData data)
        {
            foreach (var entityId in data.Entities)
            {
                if (_componentManager.TryGetComponent(entityId, out NameComponent Name))
                {
                    // Publish message for NPCs (entities with a NameComponent).
                    _eventManager.Publish(new MessageEventData(this, $"[color=green]*[/color] You see a {Name.Name}"));
                }

            }
        }







    }
}


