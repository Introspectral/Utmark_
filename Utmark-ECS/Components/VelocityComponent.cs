using Microsoft.Xna.Framework;
using Utmark_ECS.Intefaces;

namespace Utmark_ECS.Components
{
    public class VelocityComponent : IComponent
    {
        public Vector2 Velocity { get; set; }

        public VelocityComponent(Vector2 velocity)
        {
            Velocity = velocity;
        }
    }
}
