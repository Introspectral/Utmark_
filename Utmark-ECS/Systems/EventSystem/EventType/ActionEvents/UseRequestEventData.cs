using Utmark_ECS.Entities;

namespace Utmark_ECS.Systems.EventSystem.EventType.ActionEvents
{
    internal class UseRequestEventData
    {
        private Entity playerEntity;

        public UseRequestEventData(Entity playerEntity)
        {
            this.playerEntity = playerEntity;
        }
    }
}