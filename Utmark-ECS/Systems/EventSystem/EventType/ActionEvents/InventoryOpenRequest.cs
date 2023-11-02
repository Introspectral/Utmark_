using Utmark_ECS.Entities;

namespace Utmark_ECS.Systems.EventSystem.EventType.ActionEvents
{
    internal class InventoryOpenRequest
    {
        private Entity entity;

        public InventoryOpenRequest(Entity entity)
        {
            this.entity = entity;
        }
    }
}