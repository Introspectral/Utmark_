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
        private Color _backgroundColor = new Color(0, 0, 0, 200);  // RGBA values
        private Texture2D _pixel;
        private SpriteFont _font;
        private Vector2 _position;
        private int _hoveredItemIndex = -1;

        private const int MenuItemPadding = 10; // Padding for menu items, for better visual appeal

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

            // Check if the mouse is outside the menu rectangle entirely.
            if (!_rectangle.Contains(mouseState.Position))
            {
                _hoveredItemIndex = -1; // No item is hovered over.
                return;
            }

            // Calculate which menu item is under the cursor.
            int relativeX = mouseState.X - _rectangle.X; // How far right the mouse is from the left edge of the menu.
            int totalWidthChecked = 0;

            // Check each item to see if the mouse is hovering over it.
            for (int i = 0; i < _options.Count; i++)
            {
                // Measure the width of the current item.
                var itemWidth = _font.MeasureString(_options[i]).X + MenuItemPadding * 2; // Item width plus padding on both sides.

                // Check if the mouse is within the width of this item.
                if (relativeX >= totalWidthChecked && relativeX < totalWidthChecked + itemWidth)
                {
                    _hoveredItemIndex = i;
                    break;
                }

                // Add the item's width to the total width checked so far.
                totalWidthChecked += (int)itemWidth;
            }
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_pixel, _rectangle, _backgroundColor);

            int borderWidth = 2; // Set the width of the border.
            spriteBatch.Draw(_pixel, new Rectangle(_rectangle.X, _rectangle.Y + _rectangle.Height - borderWidth, _rectangle.Width, borderWidth), Color.Gray);

            // Start drawing items from the left, moving right for each one.
            float currentX = _rectangle.X + MenuItemPadding; // Start at the left edge of the menu.

            for (int i = 0; i < _options.Count; i++)
            {
                Color itemColor = (i == _hoveredItemIndex) ? Color.OliveDrab : Color.White; // Highlight if hovered.
                Vector2 position = new Vector2(currentX, _rectangle.Y + MenuItemPadding); // Keep the same vertical position for all items.

                spriteBatch.DrawString(_font, _options[i], position, itemColor);

                // Move the starting point rightward by the width of the item plus padding.
                currentX += _font.MeasureString(_options[i]).X + MenuItemPadding * 2;
            }
        }

    }
}
