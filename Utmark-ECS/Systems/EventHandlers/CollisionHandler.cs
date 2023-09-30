using Utmark_ECS.Components;
using Utmark_ECS.Managers;
using Utmark_ECS.Systems.EventSystem;
using Utmark_ECS.Systems.EventSystem.EventType;
using Utmark_ECS.Systems.Input;
using static Utmark_ECS.Enums.EventTypeEnum;

namespace Utmark_ECS.Systems.EventHandlers
{
    public class CollisionHandler
    {
        private readonly EventManager _eventManager;
        private readonly ComponentManager _componentManager;



        public CollisionHandler(EventManager eventManager, ComponentManager componentManager)
        {
            _eventManager = eventManager;
            _componentManager = componentManager;

            _eventManager.Subscribe(EventTypes.CollisionCheck, OnCollision);
        }

        private void OnCollision(EventData data)
        {
            if (data.Data is CollisionEventData collisionData)
            {
                HandleCollision(collisionData);
            }
        }

        private void HandleCollision(CollisionEventData collisionData)
        {
            var entityBComponents = _componentManager.GetComponentsForEntity(collisionData.EntityB);
            _eventManager.Publish(EventTypes.Message, this, $"CollisionHandler - HandleCollision");

            foreach (var component in entityBComponents)
            {
                if (component is ItemComponent item)
                {
                    HandleItemCollision(collisionData, item);
                }
                else if (component is NameComponent name)
                {
                    HandleNameCollision(collisionData, name);
                }
            }
        }

        private void HandleItemCollision(CollisionEventData collisionData, ItemComponent item)
        {
            // Handle collision with ItemComponent
            _eventManager.Publish(EventTypes.Message, this, $"This is {item.Name}");

            // Subscribe to InputEvent here.
            _eventManager.Subscribe(EventTypes.InputEvent, OnInputReceived);
        }

        private void OnInputReceived(EventData data)
        {
            if (data.Data is InputEventData inputEventData)
            {

            }
        }


        private void HandleNameCollision(CollisionEventData collisionData, NameComponent name)
        {

            _eventManager.Publish(EventTypes.Message, this, $"This is {name.Name}");

        }

    }
}

