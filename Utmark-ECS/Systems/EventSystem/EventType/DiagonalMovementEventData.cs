using static Utmark_ECS.Enums.InputActionEnum;

namespace Utmark_ECS.Systems.EventSystem.EventType
{
    public class DiagonalMovementEventData
    {
        public InputAction VerticalAction { get; }
        public InputAction HorizontalAction { get; }

        public DiagonalMovementEventData(InputAction verticalAction, InputAction horizontalAction)
        {
            VerticalAction = verticalAction;
            HorizontalAction = horizontalAction;
        }
    }

}
