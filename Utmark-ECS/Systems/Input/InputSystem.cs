using Microsoft.Xna.Framework.Input;
using Utmark_ECS.Systems.EventHandlers;
using Utmark_ECS.Systems.EventSystem;
using Utmark_ECS.Systems.EventSystem.EventType;

namespace Utmark_ECS.Systems.Input
{
    public class InputSystem
    {
        private EventManager _eventManager;

        private MouseState _previousMouseState;
        private KeyboardState _previousKeyboardState;
        public InputSystem(EventManager eventManager)
        {
            _eventManager = eventManager;
        }
        public void Update()
        {
            var mouseState = Mouse.GetState();
            var keyboardState = Keyboard.GetState();
            HandleKeyboardInput(keyboardState);
            HandleMouseInput(mouseState);

            _previousMouseState = mouseState;
        }

        private void HandleMouseInput(MouseState currentMouseState)
        {
            // Check for left button click.
            if (currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
            {
                _eventManager.Publish(new MessageEvent(this, "LeftClick"));
            }

            // Check for right button click.
            if (currentMouseState.RightButton == ButtonState.Pressed && _previousMouseState.RightButton == ButtonState.Released)
            {
                _eventManager.Publish(new MessageEvent(this, "RightClick"));
            }

            // Check for middle button click (scroll wheel button).
            if (currentMouseState.MiddleButton == ButtonState.Pressed && _previousMouseState.MiddleButton == ButtonState.Released)
            {
                _eventManager.Publish(new MessageEvent(this, "MiddleClick"));
            }

            // Check for scroll wheel movement.
            if (currentMouseState.ScrollWheelValue != _previousMouseState.ScrollWheelValue)
            {
                int delta = currentMouseState.ScrollWheelValue - _previousMouseState.ScrollWheelValue;
                _eventManager.Publish(new MessageEvent(this, $"MouseScroll: {delta}"));
            }

            //// Always capture the mouse position
            //var currentPosition = currentMouseState.Position;
            //if (currentPosition != _previousMouseState.Position)
            //{
            //    _eventManager.Publish(new MessageEvent(this, $"MouseMove: {currentPosition}"));
            //}

            // After handling, save the current state as the previous state for the next cycle.
            _previousMouseState = currentMouseState;
        }






        private void HandleKeyboardInput(KeyboardState currentKeyboardState)
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

            // After handling, we save the current state as the previous state for the next cycle.
            _previousKeyboardState = currentKeyboardState;
        }




        //    // Member Variables
        //    private readonly ComponentManager _componentManager;
        //    private readonly CooldownManager _cooldownManager = new();
        //    private readonly InputMapper _inputMapper;
        //    private readonly EventManager _eventManager;
        //    //private ContextMenu _contextMenu;
        //    private MouseState _currentMouseState;
        //    private MouseState _previousMouseState;
        //    //private float _elapsedTimeSinceLastMove = 0f;
        //    //private bool _isMoving = false;

        //    //private const float MoveDelay = 0.20f;

        //    // Constructor
        //    public InputSystem(ComponentManager componentManager, EventManager eventManager, InputMapper inputMapper/*, ContextMenu contextMenu*/)
        //    {
        //        _componentManager = componentManager;
        //        _eventManager = eventManager;
        //        _inputMapper = inputMapper;
        //        //_contextMenu = contextMenu;
        //    }

        //    // Main Update Method
        //    public void Update(GameTime gameTime)
        //    {
        //        var playerEntity = GetPlayerEntity();
        //        if (playerEntity == null) return;

        //        var (velocityComponent, positionComponent) = GetPlayerComponents(playerEntity);
        //        if (velocityComponent == null || positionComponent == null) return;

        //        _currentMouseState = Mouse.GetState();
        //        var state = Keyboard.GetState();

        //        HandleKeyRelease(state);
        //        HandleMouseInput();

        //        var isAnyMovementKeyPressed = IsAnyMovementKeyPressed(state);
        //        //UpdateMovementTimer(isAnyMovementKeyPressed, gameTime);

        //        _previousMouseState = _currentMouseState;

        //        //if (_elapsedTimeSinceLastMove < MoveDelay) return;
        //        var movement = GetMovementVector(state);
        //        ExecuteMovement(movement, playerEntity, positionComponent);
        //        //_contextMenu.Hide();
        //    }

        //    // Helper Methods
        //    private Entity GetPlayerEntity() =>
        //        _componentManager.GetEntitiesWithComponents(typeof(InputComponent)).FirstOrDefault();

        //    private (VelocityComponent, PositionComponent) GetPlayerComponents(Entity playerEntity)
        //    {
        //        var velocityComponent = _componentManager.GetComponent<VelocityComponent>(playerEntity);
        //        var positionComponent = _componentManager.GetComponent<PositionComponent>(playerEntity);
        //        return (velocityComponent, positionComponent);
        //    }

        //    private static bool IsAnyMovementKeyPressed(KeyboardState state) =>
        //        state.IsKeyDown(Keys.W) || state.IsKeyDown(Keys.S) || state.IsKeyDown(Keys.A) || state.IsKeyDown(Keys.D);

        //    private static bool IsActionKeyPressed(KeyboardState state) =>
        //        state.IsKeyDown(Keys.U) || state.IsKeyDown(Keys.T) || state.IsKeyDown(Keys.G) || state.IsKeyDown(Keys.D);

        //    private void HandleMouseInput()
        //    {
        //        var clickPosition = _currentMouseState.Position;
        //        if (_currentMouseState.LeftButton == ButtonState.Released && _previousMouseState.LeftButton == ButtonState.Pressed)
        //        {
        //            //_contextMenu.Hide();
        //            HandleMouseClick(clickPosition);
        //        }
        //        else if (_currentMouseState.RightButton == ButtonState.Released && _previousMouseState.RightButton == ButtonState.Pressed)
        //        {
        //            //_contextMenu.Show(clickPosition);
        //        }
        //    }

        //    private void HandleMouseClick(Point clickPosition)
        //    {
        //        _eventManager.Publish(new MouseClickEventData(clickPosition));
        //    }

        //    private void HandleKeyRelease(KeyboardState state)
        //    {
        //        if (!IsAnyMovementKeyPressed(state)/* && _isMoving*/)
        //        {
        //            //_elapsedTimeSinceLastMove = MoveDelay;
        //            //_isMoving = false;
        //        }
        //        else if (IsActionKeyPressed(state))
        //        {
        //            _inputMapper.HandleInput(state);
        //        }
        //    }

        //    //private void UpdateMovementTimer(bool isAnyMovementKeyPressed, GameTime gameTime)
        //    //{
        //    //    if (isAnyMovementKeyPressed)
        //    //    {
        //    //        if (!_isMoving)
        //    //        {
        //    //            _elapsedTimeSinceLastMove = MoveDelay;
        //    //            _isMoving = true;
        //    //        }
        //    //        else
        //    //        {
        //    //            _elapsedTimeSinceLastMove += (float)gameTime.ElapsedGameTime.TotalSeconds;
        //    //        }
        //    //    }
        //    //}

        //    private static Vector2 GetMovementVector(KeyboardState state)
        //    {
        //        var movement = Vector2.Zero;
        //        if (state.IsKeyDown(Keys.W)) movement += new Vector2(0, -1);
        //        if (state.IsKeyDown(Keys.S)) movement += new Vector2(0, 1);
        //        if (state.IsKeyDown(Keys.A)) movement += new Vector2(-1, 0);
        //        if (state.IsKeyDown(Keys.D)) movement += new Vector2(1, 0);
        //        return movement;
        //    }

        //    private void ExecuteMovement(Vector2 movement, Entity playerEntity, PositionComponent positionComponent)
        //    {
        //        //_elapsedTimeSinceLastMove = 0f;
        //        var tileSize = Globals.GridSize;
        //        var oldPosition = positionComponent.Position;
        //        var newPosition = positionComponent.Position += movement * tileSize;
        //        _eventManager.Publish(new MoveEntityEventData(playerEntity, oldPosition, newPosition));
        //    }
        //}

    }
}