using Utmark_ECS.Components;
using Utmark_ECS.Managers;
using Utmark_ECS.Systems.EventSystem;
using Utmark_ECS.Systems.EventSystem.EventType;

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
            _eventManager.Publish(new MessageEvent(this, $"This is {item.Name}"));
        }

        private void HandleNameCollision(CollisionEventData collisionData, NameComponent name)
        {

        }
    }
}

