using Utmark_ECS.Entities;

namespace Utmark_ECS.Intefaces
{
    public interface IEntityManager
    {
        Entity CreateEntity();
        Entity GetEntityById(Guid id);

    }
}
