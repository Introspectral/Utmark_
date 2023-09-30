using Microsoft.Xna.Framework;
using Utmark_ECS.Intefaces;

namespace Utmark_ECS.Components
{
    public class PositionComponent : IComponent
    {
        public Vector2 Position { get; set; }


        public PositionComponent(Vector2 position)
        {
            Position = position;

        }
    }
}
