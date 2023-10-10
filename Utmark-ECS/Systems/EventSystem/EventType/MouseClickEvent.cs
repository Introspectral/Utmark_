using Microsoft.Xna.Framework;

namespace Utmark_ECS.Systems.EventSystem.EventType
{
    public class MouseClickEvent
    {
        public Point ClickPosition { get; }

        public MouseClickEvent(Point clickPosition)
        {
            ClickPosition = clickPosition;
        }
    }
}
