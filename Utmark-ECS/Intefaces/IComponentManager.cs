using Utmark_ECS.Entities;

namespace Utmark_ECS.Intefaces
{
    public interface IComponentManager
    {
        void AddComponent(Entity entity, IComponent component);
        T? GetComponent<T>(Entity entity) where T : class, IComponent;
        void RemoveComponent<T>(Entity entity) where T : class, IComponent;
        IEnumerable<T> GetAllComponentsOfType<T>() where T : class, IComponent;
        // Other component-related methods...
    }
}
