namespace Utmark_ECS.Systems.EventSystem.EventType
{
    public class ComponentsRemoveData
    {
        public Guid EntityId { get; }

        public ComponentsRemoveData(Guid entityId)
        {
            EntityId = entityId;
        }
    }
}

