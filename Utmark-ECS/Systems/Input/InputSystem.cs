using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Utmark_ECS.Intefaces;
using Utmark_ECS.Managers;
using Utmark_ECS.Systems.EventSystem.EventType;

namespace Utmark_ECS.Systems.Input
{
    public class InputSystem : ISystem
    {
        private EventManager _eventManager;
        private Dictionary<Keys, float> _heldKeys = new Dictionary<Keys, float>();
        private float _repeatDelay = 1.0f / 4;
        private MouseState _previousMouseState;
        private KeyboardState _previousKeyboardState;
        public InputSystem(EventManager eventManager)
        {
            _eventManager = eventManager;
        }
        public void Update(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();
            var keyboardState = Keyboard.GetState();
            HandleKeyboardInput(keyboardState, gameTime.ElapsedGameTime.TotalSeconds);
            HandleMouseInput(mouseState);
            _previousMouseState = mouseState;

        }

        private void HandleMouseInput(MouseState currentMouseState)
        {
            var currentPosition = currentMouseState.Position;
            // Check for left button click.
            if (currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
            {
                _eventManager.Publish(new MouseLeftClickEventData(currentPosition));
            }

            // Check for right button click.
            if (currentMouseState.RightButton == ButtonState.Pressed && _previousMouseState.RightButton == ButtonState.Released)
            {
                _eventManager.Publish(new MouseRightClickEventData(currentPosition));
            }

            // Check for middle button click (scroll wheel button).
            if (currentMouseState.MiddleButton == ButtonState.Pressed && _previousMouseState.MiddleButton == ButtonState.Released)
            {
                _eventManager.Publish(new MouseMiddleClickEventData(currentPosition));
            }

            // Check for scroll wheel movement.
            if (currentMouseState.ScrollWheelValue != _previousMouseState.ScrollWheelValue)
            {
                int delta = currentMouseState.ScrollWheelValue - _previousMouseState.ScrollWheelValue;
                _eventManager.Publish(new MouseScrollEventData(delta));
            }
            // After handling, save the current state as the previous state for the next cycle.
            _previousMouseState = currentMouseState;
        }

        private void HandleKeyboardInput(KeyboardState currentKeyboardState, double elapsedSeconds)
        {
            // Getting all possible key values.
            var allKeys = Enum.GetValues(typeof(Keys));
            foreach (Keys key in allKeys)
            {
                // If a key is down this frame and was not down last frame, it's a new press.
                if (currentKeyboardState.IsKeyDown(key) && _previousKeyboardState.IsKeyUp(key))
                {
                    var eventData = new KeyboardEventData(key);
                    // For each key that is pressed, we publish an event.
                    _eventManager.Publish(eventData);
                }
            }
            // TODO: HandleKeyboardInput - This is a temporary way of handling repeated movement, and will need to be changed when getting to keybindings.
            // The way to go would be to have the system check for InputAction states instead of Keys pressed.
            var movementKeys = new[] { Keys.W, Keys.A, Keys.S, Keys.D, Keys.Q, Keys.E, Keys.Z, Keys.C, Keys.I };
            foreach (var key in movementKeys)
            {
                if (currentKeyboardState.IsKeyDown(key))
                {
                    if (!_heldKeys.ContainsKey(key))
                    {
                        _heldKeys[key] = _repeatDelay; // initialize timer for the held key
                    }
                    else
                    {
                        _heldKeys[key] -= (float)elapsedSeconds; // decrease timer based on elapsed time
                        if (_heldKeys[key] <= 0)
                        {
                            // Time to trigger the action again
                            _eventManager.Publish(new KeyboardEventData(key));
                            _heldKeys[key] = _repeatDelay; // reset the timer
                        }
                    }
                }
                else
                {
                    _heldKeys.Remove(key);
                }
            }
            // After handling, save the current state as the previous state for the next cycle.
            _previousKeyboardState = currentKeyboardState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}