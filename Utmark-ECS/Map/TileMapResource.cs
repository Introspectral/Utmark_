using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Utmark_ECS.Map
{
    public class TileMapResource
    {
        public Texture2D? SpriteSheet { get; set; }
        public Dictionary<string, Rectangle>? Sprites { get; set; }


    }
}
