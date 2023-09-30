using Utmark_ECS.Components;
using Utmark_ECS.Entities;
using Utmark_ECS.Intefaces;
using Utmark_ECS.Systems;
using Utmark_ECS.Systems.EventSystem;
using Utmark_ECS.Utilities;

namespace Utmark_ECS.Managers
{
    public class ComponentManager : IComponentManager
    {
        private Dictionary<Type, Dictionary<Guid, IComponent>> _entityComponents = new Dictionary<Type, Dictionary<Guid, IComponent>>();
        private IEntityManager _entityManager;
        private SpatialGrid _spatialGrid;
        private EventManager _eventManager;

        // Initializes a new instance of the class.
        public ComponentManager(EntityManager entityManager, EventManager eventManager)
        {
            _entityManager = entityManager;
            _eventManager = eventManager;
            _spatialGrid = new SpatialGrid(GameConstants.GridSize, eventManager);




        }
        public void RemoveAllComponentsOfEntity(Guid entityId)
        {
            foreach (var componentDictionary in _entityComponents.Values)
            {
                componentDictionary.Remove(entityId);
            }
        }


        /// <summary>
        /// Adds a component to a specified entity.
        /// </summary>
        /// <param name="entity">The entity to which the component should be added. Cannot be null.</param>
        /// <param name="component">The component to be added to the entity. Cannot be null.</param>
        /// <exception cref="ArgumentNullException">Thrown if either entity or component is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the entity already contains a component of the same type as the one being added.</exception>
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


        /// <summary>
        /// Retrieves a component of the specified type from the given entity.
        /// </summary>
        /// <typeparam name="T">The type of the component to retrieve. Must implement <see cref="IComponent"/>.</typeparam>
        /// <param name="entity">The entity from which to retrieve the component.</param>
        /// <returns>
        /// The component of the specified type, if the entity has such a component; 
        /// otherwise, throws an InvalidOperationException.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when the provided entity is null.</exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no components of the specified type exist or the entity does not have a component of the specified type.
        /// </exception>
        public T? GetComponent<T>(Entity entity) where T : class, IComponent
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null");

            var type = typeof(T);
            if (_entityComponents.TryGetValue(type, out var entityComponents) && entityComponents.TryGetValue(entity.ID, out var component))
                return component as T;

            throw new InvalidOperationException($"Entity {entity.ID} does not have a component of type {type.Name}");
        }

        /// <summary>
        /// Method to remove a specific component from an entity.
        /// </summary>
        /// <typeparam name="T">The type of component to remove, must be a class that implements IComponent.</typeparam>
        /// <param name="entity">The entity from which to remove the component.</param>
        /// <exception cref="ArgumentNullException">Thrown if the provided entity is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the entity does not have a component of the specified type or if no components of this type exist.</exception>
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

        /// <summary>
        /// Retrieves all components of a specified type from the Component Manager.
        /// </summary>
        /// <typeparam name="T">The type of components to retrieve, must be a class implementing IComponent.</typeparam>
        /// <returns>An IEnumerable of components of the specified type.</returns>
        /// <exception cref="InvalidOperationException">Thrown when there are no components of the specified type in the manager.</exception>
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

        /// <summary>
        /// Gets a list of entities that have all the specified component types.
        /// </summary>
        /// <param name="componentTypes">Array of component types to filter entities by.</param>
        /// <returns>List of entities that have all the specified component types.</returns>
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

        /// <summary>
        /// Updates the position of an entity and moves it in the spatial grid.
        /// </summary>
        /// <param name="entity">The entity whose position needs to be updated.</param>
        /// <param name="newPosition">The new position to move the entity to.</param>
        /// <exception cref="InvalidOperationException">Thrown when the entity does not have a PositionComponent.</exception>
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
