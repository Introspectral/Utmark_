using Utmark_ECS.Entities;

namespace Utmark_ECS.Systems.EventSystem.EventType
{
    public class RemoveComponentsEventData
    {
        public Entity EntityId { get; }

        public RemoveComponentsEventData(Entity entityId)
        {
            EntityId = entityId;
        }
    }
}

