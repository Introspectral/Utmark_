using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

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
