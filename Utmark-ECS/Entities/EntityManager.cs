using Utmark_ECS.Intefaces;
using Utmark_ECS.Systems.EventSystem;
using Utmark_ECS.Systems.EventSystem.EventType;

namespace Utmark_ECS.Entities
{

    // The EntityFactory is responsible for creating and retrieving entities.
    // Entities are unique objects within the game, which can represent characters, items, or other game elements.

    public class EntityManager : IEntityManager
    {
        private readonly EventManager _eventManager;

        // A dictionary to hold all created entities with their Guid as the key.
        private readonly Dictionary<Guid, Entity> _entities = new Dictionary<Guid, Entity>();

        public EntityManager(EventManager eventManager)
        {
            _eventManager = eventManager;
            _eventManager.Subscribe<RemoveEntityEventData>(RemoveEntity);

        }
        public Entity CreateEntity()
        {
            // Creating a new entity instance.
            var entity = new Entity();

            // Storing the created entity in the dictionary with its ID as the key.
            _entities[entity.ID] = entity;
            // Adding the Entity to the spatialGrid

            // Returning the newly created entity.
            return entity;
        }

        public void RemoveEntity(RemoveEntityEventData removedData)
        {
            // Attempt to remove entity by ID from the dictionary
            _entities.Remove(removedData.Entity.ID);
        }


        public Entity GetEntityById(Guid id)
        {
            // Checking if an entity exists with the given ID and returning it if found.
            if (_entities.ContainsKey(id))
            {
                return _entities[id];
            }

            // Returning null if no entity is found with the given ID.
            return null;
        }
    }
    //public class EntityManager : IEntityManager
    //{
    //    private readonly EventManager _eventManager;
    //    // Removed SpatialGrid dependency to decouple spatial logic

    //    private readonly Dictionary<Guid, Entity> _entities = new Dictionary<Guid, Entity>();

    //    public EntityManager(EventManager eventManager)
    //    {
    //        _eventManager = eventManager;
    //        // Subscribe with method reference instead of method name as a string
    //        _eventManager.Subscribe<RemoveEntityEventData>(OnRemoveEntity);
    //    }

    //    public Entity CreateEntity()
    //    {
    //        var entity = new Entity();
    //        _entities[entity.ID] = entity;

    //        // Notify systems of a new entity, if needed. Systems subscribe to this event type.
    //        _eventManager.Publish(new EntityCreatedEventData { Entity = entity });

    //        return entity;
    //    }

    //    private void OnRemoveEntity(RemoveEntityEventData eventData)
    //    {
    //        if (eventData.Entity == null || !_entities.ContainsKey(eventData.Entity.ID))
    //        {
    //            // Log error or handle it appropriately
    //            return;
    //        }

    //        // Notify systems of the entity removal before actually removing it.
    //        _eventManager.Publish(new EntityRemovingEventData { Entity = eventData.Entity });

    //        _entities.Remove(eventData.Entity.ID);

    //        // If there are component cleanup actions, they should be handled by the respective systems or a dedicated cleanup system.
    //    }

    //    public Entity GetEntityById(Guid id)
    //    {
    //        _entities.TryGetValue(id, out var entity);
    //        return entity; // Could be null, which is expected if no entity matches the ID.
    //    }

    //    // Optionally, if querying for entities with specific components is a common operation, you can add:
    //    public IEnumerable<Entity> GetEntitiesWithComponent<TComponent>()
    //    {
    //        // Implementation depends on how components are associated with entities.
    //        // This method would require scanning through entities and checking for the existence of the specified component type.
    //    }
    //}
}


