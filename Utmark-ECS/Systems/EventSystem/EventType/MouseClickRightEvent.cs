using Microsoft.Xna.Framework;

namespace Utmark_ECS.Systems.EventSystem.EventType
{
    public class MouseClickRightEvent
    {
        public Point ClickPosition { get; }

        public MouseClickRightEvent(Point clickPosition)
        {
            ClickPosition = clickPosition;
        }
    }
}
