using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using Utmark_ECS.Components;
using Utmark_ECS.Intefaces;
using Utmark_ECS.Managers;
using Utmark_ECS.Systems.EventSystem.EventType;

namespace Utmark_ECS.UI
{
    public class ContextMenu 
    {
        private const int BorderThickness = 2; // Consistent thickness for the border
        private const int MenuItemPadding = 10; // Padding for menu items, for better visual appeal

        private List<string> _options;
        private Rectangle _rectangle;
        private Color _backgroundColor = new Color(0, 0, 0, 200);  // RGBA values
        private Texture2D _pixel;
        private SpriteFont _font;
        private bool _isVisible = false;
        private int _hoveredItemIndex = -1;
        private Color _borderColor = Color.Gray;
        private Point _position;
        private EventManager _eventManager;
        public ContextMenu(Texture2D pixel, SpriteFont font, EventManager eventManager)
        {
            _options = new List<string> { "Look", "Search", "Get", "Use", "Hide", "Rest" };
            _pixel = pixel;
            _font = font;
            _eventManager = eventManager;
  

            _eventManager.Subscribe<MouseRightClickEventData>(OnRightClick);
            _eventManager.Subscribe<MouseLeftClickEventData>(OnLeftClick);
        }

        private void OnLeftClick(MouseLeftClickEventData data)
        {
            if (_isVisible) 
            {
                Hide();
            }
        }

        public void OnRightClick(MouseRightClickEventData data)
        {
            Show(data.ClickPosition);
            
        }

        public void Show(Point position)
        {
            _isVisible = true;
            _position = position;
            int height = _options.Count * _font.LineSpacing + MenuItemPadding * 2;
            int width = 200; // This could be dynamic based on the longest string in _options.
            _rectangle = new Rectangle(position.X, position.Y, width, height);
        }

        public void Hide()
        {
            _isVisible = false;
        }

        public void Update(GameTime gameTime)
        {
            if (!_isVisible) return;


            var mouseState = Mouse.GetState();
            if (!_rectangle.Contains(mouseState.Position)) return;
            _hoveredItemIndex = -1;

            int relativeY = mouseState.Y - _rectangle.Y;
            int index = relativeY / _font.LineSpacing;

            if (index >= 0 && index < _options.Count)
                _hoveredItemIndex = index;


            // Additional input handling can go here (e.g., clicks)
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!_isVisible) return;

            DrawBorder(spriteBatch);
            DrawBackground(spriteBatch);
            DrawMenuOptions(spriteBatch);
        }

        private void DrawBorder(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_pixel, new Rectangle(_rectangle.Left, _rectangle.Top, _rectangle.Width, BorderThickness), _borderColor); // Top
            spriteBatch.Draw(_pixel, new Rectangle(_rectangle.Left, _rectangle.Bottom - BorderThickness, _rectangle.Width, BorderThickness), _borderColor); // Bottom
            spriteBatch.Draw(_pixel, new Rectangle(_rectangle.Left, _rectangle.Top, BorderThickness, _rectangle.Height), _borderColor); // Left
            spriteBatch.Draw(_pixel, new Rectangle(_rectangle.Right - BorderThickness, _rectangle.Top, BorderThickness, _rectangle.Height), _borderColor); // Right
        }

        private void DrawBackground(SpriteBatch spriteBatch)
        {
            var innerRect = new Rectangle(_rectangle.Left + BorderThickness, _rectangle.Top + BorderThickness, _rectangle.Width - BorderThickness * 2, _rectangle.Height - BorderThickness * 2);
            spriteBatch.Draw(_pixel, innerRect, _backgroundColor);
        }

        private void DrawMenuOptions(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _options.Count; i++)
            {
                var position = new Vector2(_rectangle.X + MenuItemPadding, _rectangle.Y + i * _font.LineSpacing + MenuItemPadding);
                Color itemColor = (i == _hoveredItemIndex) ? Color.OliveDrab : Color.DarkGray; // Highlight if hovered.
                spriteBatch.DrawString(_font, _options[i], position, itemColor);
            }
        }
    }

}
