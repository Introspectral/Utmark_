using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utmark_ECS.Entities;

namespace Utmark_ECS.Systems.EventSystem.EventType.ActionEvents
{
    public class PickUpRequestEventData
    {
        public Entity Entity;
        public Vector2 Position;
        public PickUpRequestEventData(Entity entity, Vector2 position)
        {
            Entity = entity;
            Position = position;
        }
    }
}
