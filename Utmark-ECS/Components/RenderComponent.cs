using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Utmark_ECS.Intefaces;

namespace Utmark_ECS.Components
{
    public class RenderComponent : IComponent
    {
        public Texture2D Texture { get; set; }
        public Rectangle SourceRectangle { get; set; }
        public Color Tint { get; set; }
        public float Rotation { get; set; }
        public float LayerDepth { get; set; }

        public RenderComponent(Texture2D texture, Rectangle sourceRectangle, Color tint, float rotation, float layerDepth)
        {
            Texture = texture;
            SourceRectangle = sourceRectangle;
            Tint = tint;
            Rotation = rotation;
            LayerDepth = layerDepth;
        }
    }
}
