using Utmark_ECS.Entities;
using Utmark_ECS.Enums;

namespace Utmark_ECS.Systems.EventSystem.EventType
{
    public class ActionEventData
    {

        public Entity Actor { get; } // The entity performing the action
        public Entity Target { get; } // The object or entity being acted upon
        public ActionType Action { get; set; } // The type of action being performed

        public ActionEventData(Entity actor, Entity target, ActionType action)
        {
            Actor = actor;
            Action = action;
            Target = target;
        }
    }

}
