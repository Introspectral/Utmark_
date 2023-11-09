using Microsoft.Xna.Framework;
using Utmark_ECS.Entities;

namespace Utmark_ECS.Systems.EventSystem.EventType
{
    public class PopulateEntityEventData
    {
        public Entity _entity { get; set; }
        public Vector2 _position { get; set; }
        public PopulateEntityEventData(Entity entity, Vector2 position)
        {
            _entity = entity;
            _position = position;
        }
    }
}
