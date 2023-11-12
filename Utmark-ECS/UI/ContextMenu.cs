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
        private const int BorderThickness = 2;
        private const int MenuItemPadding = 10;

        private MouseState _previousMouseState;
        private List<string> _options;
        private Rectangle _rectangle;
        private Color _backgroundColor = new Color(0, 0, 0, 200);
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
            // TODO: ContextMenu - This will be used to display information on the cell/position that is being clicked. Like Terrain, Items/Entities that is located there and so forth.
            // Examples: Description, Items, Distance etc.
            _options = new List<string> { "Look", "Search", "Shout", "Whistle" };
            _pixel = pixel;
            _font = font;
            _eventManager = eventManager;
            _componentManager = componentManager;

            _eventManager.Subscribe<MouseRightClickEventData>(OnRightClick);
        }
        public Entity GetPlayerEntity() =>
        _componentManager.GetEntitiesWithComponents(typeof(PlayerComponent)).FirstOrDefault();

        public void OnRightClick(MouseRightClickEventData data)
        {
            Show(data.ClickPosition);
        }

        public void Show(Point position)
        {
            _isVisible = true;
            _position = position;
            int height = _options.Count * _font.LineSpacing + MenuItemPadding * 2;
            int width = 200;
            _rectangle = new Rectangle(position.X, position.Y, width +5, height+1);
        }

        public void Hide()
        {
            _isVisible = false;
        }

        public void Update(GameTime gameTime)
        {
            var currentMouseState = Mouse.GetState();
            var playerEntity = GetPlayerEntity();
            bool leftClickJustOccurred = currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released;

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
                Hide();
            }

            _previousMouseState = currentMouseState;
        }


        private InputAction MapOptionToAction(string option)
        {
            switch (option)
            {
                case "Look":
                    return InputAction.Look;
                case "Search":
                    return InputAction.Search;
                case "Shout":
                    _eventManager.Publish(new MessageEventData(this, $"[color=Red]You let out a Howl!![/color]"));
                    return InputAction.Blank;
                case "Whistle":
                    _eventManager.Publish(new MessageEventData(this, $"[color=orange]You imitate the sound of a bird.[/color]"));
                    return InputAction.Blank;
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
