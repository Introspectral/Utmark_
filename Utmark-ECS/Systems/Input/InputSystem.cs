using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Utmark_ECS.Components;
using Utmark_ECS.Entities;
using Utmark_ECS.Managers;
using Utmark_ECS.Systems.EventHandlers;
using Utmark_ECS.Systems.EventSystem;
using Utmark_ECS.Systems.EventSystem.EventType;
using Utmark_ECS.Utilities;

namespace Utmark_ECS.Systems.Input
{
    public class InputSystem
    {
        private MouseState _currentMouseState;
        private MouseState _previousMouseState;
        private readonly ComponentManager _componentManager;
        private readonly CooldownManager _cooldownManager = new();
        private readonly InputMapper _inputMapper;
        private EventManager _eventManager;
        private float _elapsedTimeSinceLastMove = 0f;
        private const float MoveDelay = 0.20f;
        private bool _isMoving = false;

        public InputSystem(ComponentManager componentManager, EventManager eventManager, InputMapper inputMapper)
        {
            _componentManager = componentManager;
            _eventManager = eventManager;
            _inputMapper = inputMapper;
        }

        public void Update(GameTime gameTime)
        {
            var playerEntity = GetPlayerEntity();
            if (playerEntity == null) return;

            var (velocityComponent, positionComponent) = GetPlayerComponents(playerEntity);
            if (velocityComponent == null || positionComponent == null) return;

            _currentMouseState = Mouse.GetState();
            var state = Keyboard.GetState();
            HandleKeyRelease(state);
            HandleMouseInput();
            var isAnyMovementKeyPressed = IsAnyMovementKeyPressed(state);
            UpdateMovementTimer(isAnyMovementKeyPressed, gameTime);

            _previousMouseState = _currentMouseState;
            if (_elapsedTimeSinceLastMove < MoveDelay) return;

            var movement = GetMovementVector(state);
            ExecuteMovement(movement, playerEntity, positionComponent);
        }

        private Entity GetPlayerEntity() =>
            _componentManager.GetEntitiesWithComponents(typeof(InputComponent)).FirstOrDefault();

        private (VelocityComponent, PositionComponent) GetPlayerComponents(Entity playerEntity)
        {
            var velocityComponent = _componentManager.GetComponent<VelocityComponent>(playerEntity);
            var positionComponent = _componentManager.GetComponent<PositionComponent>(playerEntity);
            return (velocityComponent, positionComponent);
        }

        private static bool IsAnyMovementKeyPressed(KeyboardState state) =>
            state.IsKeyDown(Keys.Up) || state.IsKeyDown(Keys.Down) ||
            state.IsKeyDown(Keys.Left) || state.IsKeyDown(Keys.Right);

        private static bool IsActionKeyPressed(KeyboardState state) =>
            state.IsKeyDown(Keys.U) || state.IsKeyDown(Keys.T) ||
            state.IsKeyDown(Keys.G) || state.IsKeyDown(Keys.D);
        private void HandleMouseInput()
        {
            if (_currentMouseState.LeftButton == ButtonState.Released && _previousMouseState.LeftButton == ButtonState.Pressed)
            {
                // The left mouse button was just released.
                var clickPosition = _currentMouseState.Position;

                _eventManager.Publish(new MessageEvent(this, $"Clicked {clickPosition}"));
                HandleMouseClick(clickPosition);
            }
            else if (_currentMouseState.RightButton == ButtonState.Released && _previousMouseState.RightButton == ButtonState.Pressed)
            {
                _eventManager.Publish(new MessageEvent(this, $"[color=green]There will be a menu here[/color]"));
            }
        }
        private void HandleMouseClick(Point clickPosition)
        {
            // Do something with the click position. E.g.:
            _eventManager.Publish(new MouseClickEvent(clickPosition));
            //_inputMapper.HandleMouseInput(clickPosition);
        }
        private void HandleKeyRelease(KeyboardState state)
        {
            if (!IsAnyMovementKeyPressed(state) && _isMoving)
            {
                _elapsedTimeSinceLastMove = MoveDelay;
                _isMoving = false;
            }
            else if (IsActionKeyPressed(state))
            {
                _inputMapper.HandleInput(state); // Let the InputMapper handle the input and publish the necessary events.
            }
        }

        private void UpdateMovementTimer(bool isAnyMovementKeyPressed, GameTime gameTime)
        {
            if (isAnyMovementKeyPressed)
            {
                if (!_isMoving)
                {
                    _elapsedTimeSinceLastMove = MoveDelay; // Set to MoveDelay for immediate response on first press
                    _isMoving = true;
                }
                else
                {
                    _elapsedTimeSinceLastMove += (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
        }

        private static Vector2 GetMovementVector(KeyboardState state)
        {
            var movement = Vector2.Zero;
            if (state.IsKeyDown(Keys.Up)) movement += new Vector2(0, -1);
            if (state.IsKeyDown(Keys.Down)) movement += new Vector2(0, 1);
            if (state.IsKeyDown(Keys.Left)) movement += new Vector2(-1, 0);
            if (state.IsKeyDown(Keys.Right)) movement += new Vector2(1, 0);
            return movement;
        }

        private void ExecuteMovement(Vector2 movement, Entity playerEntity, PositionComponent positionComponent)
        {
            _elapsedTimeSinceLastMove = 0f;
            var tileSize = GameConstants.GridSize; // Replace with actual tile size*/
            var oldPosition = positionComponent.Position;
            var newPosition = positionComponent.Position += movement * tileSize;
            _eventManager.Publish(new EntityMovedData(playerEntity, oldPosition, newPosition));

        }
    }
}