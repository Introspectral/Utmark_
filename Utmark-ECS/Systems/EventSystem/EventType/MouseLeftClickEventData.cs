using Microsoft.Xna.Framework;

namespace Utmark_ECS.Systems.EventSystem.EventType
{
    public class MouseLeftClickEventData
    {
        public Point ClickPosition { get; }

        public MouseLeftClickEventData(Point clickPosition)
        {
            ClickPosition = clickPosition;
        }
    }
}
