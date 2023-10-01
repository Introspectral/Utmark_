using Utmark_ECS.Entities;
using Utmark_ECS.Enums;

namespace Utmark_ECS.Systems.EventSystem.EventType
{
    public class ActionEventData
    {
        public ActionType Action { get; set; } // The type of action being performed

        public ActionEventData(ActionType action)
        {

            Action = action;

        }
    }

}
