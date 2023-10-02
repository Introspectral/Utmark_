using Utmark_ECS.Systems.EventSystem;
using Utmark_ECS.Systems.EventSystem.EventType;
using static Utmark_ECS.Enums.EventTypeEnum;
using static Utmark_ECS.Enums.InputActionEnum;

namespace Utmark_ECS.Systems.EventHandlers
{
    public class ActionHandler
    {
        private readonly EventManager _eventManager;


        public ActionHandler(EventManager eventManager)
        {
            _eventManager = eventManager;
        }
    }
}


