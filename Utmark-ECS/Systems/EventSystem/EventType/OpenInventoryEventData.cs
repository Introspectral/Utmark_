using Utmark_ECS.Entities;

namespace Utmark_ECS.Systems.EventSystem.EventType
{
    public class OpenInventoryEventData
    {
        public Entity Entity;
        public OpenInventoryEventData(Entity entity)
        {
            Entity = entity;
        }
    }
}