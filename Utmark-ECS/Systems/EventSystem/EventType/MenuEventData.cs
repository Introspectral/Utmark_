using Utmark_ECS.Entities;
using Utmark_ECS.Enums;

namespace Utmark_ECS.Systems.EventSystem.EventType
{
    public class MenuEventData
    {
        public InputAction Action { get; }
        public Entity Entity { get; }
        public MenuEventData(InputAction action, Entity entity)
        {
            Action = action;
            Entity = entity;
        }
    }
}
