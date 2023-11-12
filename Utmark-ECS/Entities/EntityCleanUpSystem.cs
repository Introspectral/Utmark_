using Utmark_ECS.Components;
using Utmark_ECS.Managers;
using Utmark_ECS.Systems.EventSystem.EventType;

namespace Utmark_ECS.Entities
{
    public class EntityCleanUpSystem
    {
        private readonly EventManager _eventManager;
        private readonly ComponentManager _componentManager;


        public EntityCleanUpSystem(EventManager eventManager, ComponentManager componentManager)
        {
            _eventManager = eventManager ?? throw new ArgumentNullException(nameof(eventManager));
            _componentManager = componentManager ?? throw new ArgumentNullException(nameof(componentManager));

            _eventManager.Subscribe<RemoveComponentsEventData>(OnEntityDelete);
        }

        private void OnEntityDelete(RemoveComponentsEventData removedData)
        {
            var entityId = removedData.EntityId;
            RemoveComponents(entityId);

        }

        private void RemoveComponents(Entity entityId)
        {
            if (_componentManager.TryGetComponent<PositionComponent>(entityId, out var positionComponent))
            {
                var position = positionComponent.Position;

                _componentManager.RemoveAllComponentsOfEntity(entityId.ID);

                _eventManager.Publish(new RemoveEntityEventData(entityId, position));
            }
            else
            {
                Console.WriteLine($"Error: Entity with ID {entityId.ID} did not have an associated PositionComponent.");
            }
        }

    }
}
