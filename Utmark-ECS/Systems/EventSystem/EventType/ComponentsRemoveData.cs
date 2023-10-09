using Utmark_ECS.Entities;

namespace Utmark_ECS.Systems.EventSystem.EventType
{
    public class ComponentsRemoveData
    {
        public Entity EntityId { get; }

        public ComponentsRemoveData(Entity entityId)
        {
            EntityId = entityId;
        }
    }
}

