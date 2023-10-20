using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Utmark_ECS.Components;
using Utmark_ECS.Entities;
using Utmark_ECS.Enums;
using Utmark_ECS.Managers;
using Utmark_ECS.Systems.EventSystem.EventType;
using Utmark_ECS.Systems.EventSystem.EventType.ActionEvents;

namespace Utmark_ECS.UI
{
    public class ContextMenu
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
        public ContextMenu(Texture2D pixel, SpriteFont font, EventManager eventManager, ComponentManager componentManager)
        {
            _options = new List<string> { "Look", "Search", "Use", "Pick Up" }; // TODO: This will be Updated lated on to contain available actions based on the context of the player; Near water? Drink. Near Fire and has a sausage and a stick? Make Hotdog etc.
            _pixel = pixel;
            _font = font;
            _eventManager = eventManager;
            _componentManager = componentManager;

            _eventManager.Subscribe<MouseRightClickEventData>(OnRightClick);
        }
        public Entity GetPlayerEntity() =>
        _componentManager.GetEntitiesWithComponents(typeof(InputComponent)).FirstOrDefault();

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
            _rectangle = new Rectangle(position.X, position.Y, width +5, height+1);
        }

        public void Hide()
        {
            _isVisible = false;
        }

        public void Update(GameTime gameTime)
        {
            // Get the current state of the mouse.
            var currentMouseState = Mouse.GetState();
            var playerEntity = GetPlayerEntity();
            // Check if the left mouse button was just clicked.
            bool leftClickJustOccurred = currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released;

            // Check whether the mouse click occurred inside or outside the context menu's bounds.
            if (_rectangle.Contains(currentMouseState.Position))
            {
                // The mouse is inside the context menu.
                _hoveredItemIndex = -1;

                int relativeY = currentMouseState.Y - _rectangle.Y;

                int index = relativeY / _font.LineSpacing;

                if (index >= 0 && index < _options.Count)
                {
                    _hoveredItemIndex = index;

                    if (leftClickJustOccurred)
                    {
                        // Perform the action associated with the menu option here.
                        InputAction action = MapOptionToAction(_options[_hoveredItemIndex]);
                        //_eventManager.Publish(new MessageEvent(this, $"Action triggered: {action}"));

                        var menuEventData = new ActionEventData(action, playerEntity);
                        _eventManager.Publish(menuEventData);

                        Hide();  // Hide after action.
                    }
                }
            }
            else if (leftClickJustOccurred)
            {
                // The click occurred outside the context menu, so we hide it.
                Hide();
            }

            // Store the current state to compare with the next state in the subsequent update.
            _previousMouseState = currentMouseState;
        }


        // This method maps the option text to a specific action. This is a simplified version,
        // and you might need a more complex logic depending on your needs.
        private InputAction MapOptionToAction(string option)
        {
            switch (option)
            {
                case "Look":
                    return InputAction.Look;
                case "Search":
                    return InputAction.Search;
                case "Use":
                    return InputAction.Use;
                case "Pick Up":
                    return InputAction.PickUp;
                default:
                    throw new ArgumentOutOfRangeException(nameof(option), $"No mapping exists for option {option}");
            }
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
