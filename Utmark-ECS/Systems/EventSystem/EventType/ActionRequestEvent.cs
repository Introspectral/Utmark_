using static Utmark_ECS.Enums.InputActionEnum;

namespace Utmark_ECS.Systems.EventSystem.EventType
{
    public class ActionRequestEvent
    {
        public InputAction State { get; }

        public ActionRequestEvent(InputAction state)
        {
            State = state;
        }
    }
}
