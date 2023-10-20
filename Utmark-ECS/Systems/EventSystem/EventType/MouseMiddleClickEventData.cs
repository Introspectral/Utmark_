using Microsoft.Xna.Framework;

namespace Utmark_ECS.Systems.EventSystem.EventType
{
    public class MouseMiddleClickEventData
    {
        public Point ClickPosition { get; }

        public MouseMiddleClickEventData(Point clickPosition)
        {
            ClickPosition = clickPosition;
        }
    }
}
