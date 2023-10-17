using Utmark_ECS.Entities;
using Utmark_ECS.Enums;

namespace Utmark_ECS.Systems.EventSystem.EventType
{
    public class ActionEventData
    {
        public Entity Entity { get; set; }
        public InputAction State { get; }

        public ActionEventData(InputAction state, Entity entity)
        {
            State = state;
            Entity = entity;
        }
    }
}
