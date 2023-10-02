using Utmark_ECS.Intefaces;
using Utmark_ECS.Systems;
using Utmark_ECS.Systems.EventSystem;
using Utmark_ECS.Systems.EventSystem.EventType;
using static Utmark_ECS.Enums.EventTypeEnum;

namespace Utmark_ECS.Entities
{

    // The EntityFactory is responsible for creating and retrieving entities.
    // Entities are unique objects within the game, which can represent characters, items, or other game elements.

    public class EntityManager : IEntityManager
    {
        private readonly EventManager _eventManager;
        private readonly SpatialGrid _spatialGrid;

        // A dictionary to hold all created entities with their Guid as the key.
        private readonly Dictionary<Guid, Entity> _entities = new Dictionary<Guid, Entity>();

        public EntityManager(EventManager eventManager, SpatialGrid spatialGrid)
        {
            _eventManager = eventManager;
            _spatialGrid = spatialGrid;
           _eventManager.Subscribe<EntityRemoveData> (RemoveEntity);

        }
        public Entity CreateEntity()
        {
            // Creating a new entity instance.
            var entity = new Entity();

            // Storing the created entity in the dictionary with its ID as the key.
            _entities[entity.ID] = entity;
            // Adding the Entity to the spatialGrid
            _spatialGrid.AddEntity(entity, new Microsoft.Xna.Framework.Vector2(0, 0));
            // Returning the newly created entity.
            return entity;
        }

        public void RemoveEntity(EntityRemoveData removedData)
        {
                // Attempt to remove entity by ID from the dictionary
                _entities.Remove(removedData.Entity.ID);
        }


        public Entity? GetEntityById(Guid id)
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


