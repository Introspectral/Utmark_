﻿using Utmark_ECS.Components;
using Utmark_ECS.Entities;
using Utmark_ECS.Managers;
using Utmark_ECS.Systems.EventSystem;
using Utmark_ECS.Systems.EventSystem.EventType;
using static Utmark_ECS.Enums.EventTypeEnum;

namespace Utmark_ECS.Systems
{
    public class EntityCleanUpSystem
    {
        private readonly EventManager _eventManager;
        private readonly ComponentManager _componentManager; // You need access to ComponentManager to remove components

        public EntityCleanUpSystem(EventManager eventManager, ComponentManager componentManager)
        {
            _eventManager = eventManager ?? throw new ArgumentNullException(nameof(eventManager));
            _componentManager = componentManager ?? throw new ArgumentNullException(nameof(componentManager));

            _eventManager.Subscribe(EventTypes.ComponentDelete, OnEntityDelete);
        }

        private void OnEntityDelete(EventData data)
        {
            if (data.Data is EntityRemoveData removedData)
            {
                var entityId = removedData.Entity;
                RemoveComponents(entityId); // Remove components first

            }
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
                _eventManager.Publish(EventTypes.EntityDelete, this, new EntityRemoveData(entityId, position));
            }
            else
            {
                // Log an error message or handle the case where the entity did not have an associated PositionComponent
                Console.WriteLine($"Error: Entity with ID {entityId.ID} did not have an associated PositionComponent.");
            }
        }

    }
}
