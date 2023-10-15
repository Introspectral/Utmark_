using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Utmark_ECS.Components;

namespace Utmark_ECS.UI.UI_Elements
{
    public class TopUI : UIComponent
    {
        private Rectangle _rectangle;
        private Color _backgroundColor = Color.Black;
        private Texture2D _pixel;
        private SpriteFont _font;

        public TopUI(int width, int height, Texture2D pixel, SpriteFont font)
        {
            _rectangle = new Rectangle(0, 0, width, height);
            _pixel = pixel;
            _font = font;
        }

        public override void Update(GameTime gameTime)
        {
            // Update any dynamic elements, like stats, here
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_pixel, _rectangle, _backgroundColor);
            int borderWidth = 2; // Set the width of the border
            spriteBatch.Draw(_pixel, new Rectangle(_rectangle.X, _rectangle.Y + _rectangle.Height - borderWidth, _rectangle.Width, borderWidth), Color.Gray); var position = new Vector2(16, 16);
            spriteBatch.DrawString(_font, $"Top-bar UI Element", position, Color.White);
        }
    }
}
