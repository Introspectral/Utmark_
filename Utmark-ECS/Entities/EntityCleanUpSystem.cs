using Utmark_ECS.Components;
using Utmark_ECS.Managers;
using Utmark_ECS.Systems.EventSystem.EventType;

namespace Utmark_ECS.Entities
{
    public class EntityCleanUpSystem
    {
        private readonly EventManager _eventManager;
        private readonly ComponentManager _componentManager; // You need access to ComponentManager to remove components


        public EntityCleanUpSystem(EventManager eventManager, ComponentManager componentManager)
        {
            _eventManager = eventManager ?? throw new ArgumentNullException(nameof(eventManager));
            _componentManager = componentManager ?? throw new ArgumentNullException(nameof(componentManager));

            _eventManager.Subscribe<RemoveComponentsEventData>(OnEntityDelete);
        }

        private void OnEntityDelete(RemoveComponentsEventData removedData)
        {
            var entityId = removedData.EntityId;
            RemoveComponents(entityId); // Remove components first

        }

        private void RemoveComponents(Entity entityId)
        {
            // Attempt to get the PositionComponent of the entity
            if (_componentManager.TryGetComponent<PositionComponent>(entityId, out var positionComponent))
            {
                // Store the position before removing the components
                var position = positionComponent.Position;

                // Now, remove all components of the entity
                _componentManager.RemoveAllComponentsOfEntity(entityId.ID);

                // Finally, publish the EntityDelete event with the stored position
                _eventManager.Publish(new RemoveEntityEventData(entityId, position));
            }
            else
            {
                // Log an error message or handle the case where the entity did not have an associated PositionComponent
                Console.WriteLine($"Error: Entity with ID {entityId.ID} did not have an associated PositionComponent.");
            }
        }

    }
}
