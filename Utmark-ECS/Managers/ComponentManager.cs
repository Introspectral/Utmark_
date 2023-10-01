using Utmark_ECS.Components;
using Utmark_ECS.Entities;
using Utmark_ECS.Intefaces;
using Utmark_ECS.Systems;
using Utmark_ECS.Systems.EventSystem;

namespace Utmark_ECS.Managers
{
    public class ComponentManager : IComponentManager
    {
        private Dictionary<Type, Dictionary<Guid, IComponent>> _entityComponents = new Dictionary<Type, Dictionary<Guid, IComponent>>();
        private IEntityManager _entityManager;
        private SpatialGrid _spatialGrid;
        private EventManager _eventManager;
        private TileMap _tileMap;

        // Initializes a new instance of the class.
        public ComponentManager(EntityManager entityManager, EventManager eventManager, TileMap tileMap, SpatialGrid spatialGrid)
        {
            _entityManager = entityManager;
            _tileMap = tileMap;
            _eventManager = eventManager;
            _spatialGrid = spatialGrid;

        }
        public void SetTileMapAndSpatialGrid(TileMap tileMap, SpatialGrid spatialGrid)
        {
            _tileMap = tileMap;
            _spatialGrid = spatialGrid;
        }
        public void RemoveAllComponentsOfEntity(Guid entityId)
        {
            foreach (var componentDictionary in _entityComponents.Values)
            {
                componentDictionary.Remove(entityId);
            }
        }
        public void AddComponent(Entity entity, IComponent component)
        {
            // Null checks first to ensure valid inputs.
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null");
            }

            if (component == null)
            {
                throw new ArgumentNullException(nameof(component), "Component cannot be null");
            }

            var type = component.GetType();

            // Using TryGetValue to minimize dictionary lookups.
            if (!_entityComponents.TryGetValue(type, out var entityComponents))
            {
                entityComponents = new Dictionary<Guid, IComponent>();
                _entityComponents[type] = entityComponents;
            }

            // Check if the entity already has a component of this type.
            if (entityComponents.ContainsKey(entity.ID))
            {
                throw new InvalidOperationException($"Entity {entity.ID} already contains a component of type {type.Name}");
            }

            // Add the component to the dictionary for this entity and type.
            entityComponents[entity.ID] = component;

            // If the component is of type PositionComponent, add the entity to the spatial grid with the position from the component.
            if (component is PositionComponent positionComponent)
            {
                _spatialGrid.AddEntity(entity, positionComponent.Position);
            }
        }

        public T? GetComponent<T>(Entity entity) where T : class, IComponent
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null");

            var type = typeof(T);
            if (_entityComponents.TryGetValue(type, out var entityComponents) && entityComponents.TryGetValue(entity.ID, out var component))
                return component as T;

            throw new InvalidOperationException($"Entity {entity.ID} does not have a component of type {type.Name}");
        }

        public void RemoveComponent<T>(Entity entity) where T : class, IComponent
        {
            // Validate the entity is not null, if null, throw an exception as entity is mandatory for removing its component.
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null");
            }

            // Get the type of the component to be removed.
            var type = typeof(T);

            // If there are no components of the specified type or if the entity does not have a component of this type, throw an exception.
            if (!_entityComponents.ContainsKey(type) || !_entityComponents[type].ContainsKey(entity.ID))
            {
                throw new InvalidOperationException($"Entity {entity.ID} does not have a component of type {type.Name}");
            }

            // Remove the specified component from the entity by removing the entity’s ID from the dictionary corresponding to the component type.
            _entityComponents[type].Remove(entity.ID);

            // If there are no more entities with this component type, remove the entry for this component type from the component manager.
            if (_entityComponents[type].Count == 0)
            {
                _entityComponents.Remove(type);
            }
        }

        public IEnumerable<T> GetAllComponentsOfType<T>() where T : class, IComponent
        {
            // Get the type of the component to be retrieved.
            var type = typeof(T);

            // Check if there are components of the specified type in the manager,
            // if not, throw an InvalidOperationException.
            if (!_entityComponents.ContainsKey(type))
            {
                throw new InvalidOperationException($"No components of type {type.Name} exist");
            }

            // Return all components of the specified type casted to the specified type.
            return _entityComponents[type].Values.Cast<T>();
        }

        public List<Entity> GetEntitiesWithComponents(params Type[] componentTypes)
        {
            // Initialize a HashSet with the keys (Entity IDs) of the first component type
            var intersectedEntities = new HashSet<Guid>(_entityComponents[componentTypes[0]].Keys);

            // For each subsequent component type, intersect the HashSet with the keys of the current component type
            foreach (var componentType in componentTypes.Skip(1))
            {
                // If entities with the current component type exist, perform intersection, otherwise return an empty list.
                if (_entityComponents.ContainsKey(componentType))
                {
                    intersectedEntities.IntersectWith(_entityComponents[componentType].Keys);
                }
                else
                {
                    return new List<Entity>();
                }
            }

            // Convert the intersected entity IDs to Entity objects and return as a list
            return intersectedEntities.Select(id => _entityManager.GetEntityById(id)).ToList();
        }

        public List<IComponent> GetComponentsForEntity(Entity entity)
        {
            List<IComponent> components = new List<IComponent>();
            foreach (var componentType in _entityComponents.Keys)
            {
                if (_entityComponents[componentType].TryGetValue(entity.ID, out var component))
                {
                    components.Add(component);
                }
            }
            return components;
        }

        public bool TryGetComponent<T>(Entity entity, out T component) where T : class, IComponent
        {
            component = null;
            if (entity == null) return false;

            if (_entityComponents.TryGetValue(typeof(T), out var entityComponents) &&
                entityComponents.TryGetValue(entity.ID, out var retrievedComponent))
            {
                component = retrievedComponent as T;
                return component != null;
            }

            return false;
        }
    }
}
