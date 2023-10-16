using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Utmark_ECS.Systems.EventSystem.EventType
{
    public class RenderEventData
    {
        public Vector2 Position { get; set; }
        public RenderEventData()
        {
            Position = new Vector2();
        }
    }
}
