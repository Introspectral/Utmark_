using Utmark_ECS.Entities;
using Utmark_ECS.Intefaces;
using Utmark_ECS.Systems.EventSystem.EventType;

namespace Utmark_ECS.Managers
{
    public class EntityManager : IEntityManager
    {
        private readonly EventManager _eventManager;

        private readonly Dictionary<Guid, Entity> _entities = new Dictionary<Guid, Entity>();

        public EntityManager(EventManager eventManager)
        {
            _eventManager = eventManager;
            _eventManager.Subscribe<RemoveEntityEventData>(RemoveEntity);
        }

        public Entity CreateEntity()
        {
            var entity = new Entity();

            _entities[entity.ID] = entity;

            return entity;
        }

        public void RemoveEntity(RemoveEntityEventData removedData)
        {
            _entities.Remove(removedData.Entity.ID);
        }

        public Entity GetEntityById(Guid id)
        {
            if (_entities.ContainsKey(id))
            {
                return _entities[id];
            }
            return null;
        }
    }
}


