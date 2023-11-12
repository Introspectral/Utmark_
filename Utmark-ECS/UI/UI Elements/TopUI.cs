using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Utmark_ECS.Components;

namespace Utmark_ECS.UI.UI_Elements
{
    public class TopUI : UIComponent
    {
        private List<string> _options;
        private Rectangle _rectangle;
        private Color _backgroundColor = new Color(0, 0, 0, 200);
        private Texture2D _pixel;
        private SpriteFont _font;
        private Vector2 _position;
        private int _hoveredItemIndex = -1;

        private const int MenuItemPadding = 10;

        public TopUI(int width, int height, Texture2D pixel, SpriteFont font)
        {
            _options = new List<string> { "Character", "Inventory", "Map", "Options", "Help", "About" };
            _rectangle = new Rectangle(0, 0, width, height);
            _pixel = pixel;
            _font = font;


        }

        public override void Update(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();

            if (!_rectangle.Contains(mouseState.Position))
            {
                _hoveredItemIndex = -1;

            }
            int relativeX = mouseState.X - _rectangle.X;
            int totalWidthChecked = 0;

            for (int i = 0; i < _options.Count; i++)
            {
                var itemWidth = _font.MeasureString(_options[i]).X + MenuItemPadding * 2;

                if (relativeX >= totalWidthChecked && relativeX < totalWidthChecked + itemWidth)
                {
                    _hoveredItemIndex = i;
                    break;
                }

                totalWidthChecked += (int)itemWidth;
            }
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_pixel, _rectangle, _backgroundColor);

            int borderWidth = 2;
            spriteBatch.Draw(_pixel, new Rectangle(_rectangle.X, _rectangle.Y + _rectangle.Height - borderWidth, _rectangle.Width, borderWidth), Color.Gray);

            float currentX = _rectangle.X + MenuItemPadding;

            for (int i = 0; i < _options.Count; i++)
            {
                Color itemColor = (i == _hoveredItemIndex) ? Color.OliveDrab : Color.White;
                Vector2 position = new Vector2(currentX, _rectangle.Y + MenuItemPadding);

                spriteBatch.DrawString(_font, _options[i], position, itemColor);

                currentX += _font.MeasureString(_options[i]).X + MenuItemPadding * 2;
            }
        }

    }
}
