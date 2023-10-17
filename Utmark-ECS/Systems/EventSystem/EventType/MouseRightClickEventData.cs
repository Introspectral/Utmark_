using Microsoft.Xna.Framework;

namespace Utmark_ECS.Systems.EventSystem.EventType
{
    public class MouseRightClickEventData
    {
        public Point ClickPosition { get; }

        public MouseRightClickEventData(Point clickPosition)
        {
            ClickPosition = clickPosition;
        }
    }
}
