using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Utmark_ECS.Managers;

namespace Utmark_ECS.UI
{
    public abstract class BaseMenu
    {
        private const int BorderThickness = 2; // Consistent thickness for the border
        private const int MenuItemPadding = 10; // Padding for menu items, for better visual appeal

        private MouseState _previousMouseState;
        private List<string> _options;
        private Rectangle _rectangle;
        private Color _backgroundColor = new Color(0, 0, 0, 200);  // RGBA values
        private Texture2D _pixel;
        private SpriteFont _font;
        private bool _isVisible = false;
        private int _hoveredItemIndex = -1;
        private Color _borderColor = Color.Gray;
        private Point _position;
        private ComponentManager _componentManager;
        private EventManager _eventManager;

        public BaseMenu(Texture2D pixel, SpriteFont font, EventManager eventManager)
        {
            _pixel = pixel;
            _font = font;
            _eventManager = eventManager;
            _hoveredItemIndex = -1;
        }

        public virtual void Show(Point position)
        {
            _position = position;
            _isVisible = true;
            // Calculate the dimensions and position of the rectangle here based on the options and font metrics.
        }

        public virtual void Hide()
        {
            _isVisible = false;
        }

        public virtual void Update(GameTime gameTime)
        {
            // Common update logic, if any, or keep it abstract to force derived classes to implement their versions.
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (!_isVisible) return;

            DrawBorder(spriteBatch);
            DrawBackground(spriteBatch);
            DrawMenuOptions(spriteBatch);
        }

        protected virtual void DrawBorder(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_pixel, new Rectangle(_rectangle.Left, _rectangle.Top, _rectangle.Width, BorderThickness), _borderColor); // Top
            spriteBatch.Draw(_pixel, new Rectangle(_rectangle.Left, _rectangle.Bottom - BorderThickness, _rectangle.Width, BorderThickness), _borderColor); // Bottom
            spriteBatch.Draw(_pixel, new Rectangle(_rectangle.Left, _rectangle.Top, BorderThickness, _rectangle.Height), _borderColor); // Left
            spriteBatch.Draw(_pixel, new Rectangle(_rectangle.Right - BorderThickness, _rectangle.Top, BorderThickness, _rectangle.Height), _borderColor); // Right
        }

        protected virtual void DrawBackground(SpriteBatch spriteBatch)
        {
            var innerRect = new Rectangle(_rectangle.Left + BorderThickness, _rectangle.Top + BorderThickness, _rectangle.Width - BorderThickness * 2, _rectangle.Height - BorderThickness * 2);
            spriteBatch.Draw(_pixel, innerRect, _backgroundColor);
        }

        protected virtual void DrawMenuOptions(SpriteBatch spriteBatch)
        {
            // Draw the menu options, this can be overridden if the derived class requires a different way of displaying options.
        }

        // Other utility methods common to all menus.
    }
}
