using Utmark_ECS.Entities;
using Utmark_ECS.Intefaces;
using Utmark_ECS.Systems.EventSystem.EventType;

namespace Utmark_ECS.Managers
{
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
}


