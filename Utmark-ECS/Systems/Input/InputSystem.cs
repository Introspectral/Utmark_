using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Utmark_ECS.Components;
using Utmark_ECS.Entities;
using Utmark_ECS.Managers;
using Utmark_ECS.Systems.EventHandlers;
using Utmark_ECS.Systems.EventSystem;
using Utmark_ECS.Systems.EventSystem.EventType;
using Utmark_ECS.UI;
using Utmark_ECS.Utilities;

namespace Utmark_ECS.Systems.Input
{
    public class InputSystem
    {
        // Member Variables
        private readonly ComponentManager _componentManager;
        private readonly CooldownManager _cooldownManager = new();
        private readonly InputMapper _inputMapper;
        private readonly EventManager _eventManager;
        private ContextMenu _contextMenu;
        private MouseState _currentMouseState;
        private MouseState _previousMouseState;
        private float _elapsedTimeSinceLastMove = 0f;
        private bool _isMoving = false;

        private const float MoveDelay = 0.20f;

        // Constructor
        public InputSystem(ComponentManager componentManager, EventManager eventManager, InputMapper inputMapper, ContextMenu contextMenu)
        {
            _componentManager = componentManager;
            _eventManager = eventManager;
            _inputMapper = inputMapper;
            _contextMenu = contextMenu;
        }

        // Main Update Method
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
            _contextMenu.Hide();
        }

        // Helper Methods
        private Entity GetPlayerEntity() =>
            _componentManager.GetEntitiesWithComponents(typeof(InputComponent)).FirstOrDefault();

        private (VelocityComponent, PositionComponent) GetPlayerComponents(Entity playerEntity)
        {
            var velocityComponent = _componentManager.GetComponent<VelocityComponent>(playerEntity);
            var positionComponent = _componentManager.GetComponent<PositionComponent>(playerEntity);
            return (velocityComponent, positionComponent);
        }

        private static bool IsAnyMovementKeyPressed(KeyboardState state) =>
            state.IsKeyDown(Keys.Up) || state.IsKeyDown(Keys.Down) || state.IsKeyDown(Keys.Left) || state.IsKeyDown(Keys.Right);

        private static bool IsActionKeyPressed(KeyboardState state) =>
            state.IsKeyDown(Keys.U) || state.IsKeyDown(Keys.T) || state.IsKeyDown(Keys.G) || state.IsKeyDown(Keys.D);

        private void HandleMouseInput()
        {
            var clickPosition = _currentMouseState.Position;
            if (_currentMouseState.LeftButton == ButtonState.Released && _previousMouseState.LeftButton == ButtonState.Pressed)
            {
                _contextMenu.Hide();
                HandleMouseClick(clickPosition);
            }
            else if (_currentMouseState.RightButton == ButtonState.Released && _previousMouseState.RightButton == ButtonState.Pressed)
            {
                _contextMenu.Show(clickPosition);
            }            
        }

        private void HandleMouseClick(Point clickPosition)
        {
            _eventManager.Publish(new MouseClickEvent(clickPosition));
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
                _inputMapper.HandleInput(state);
            }
        }

        private void UpdateMovementTimer(bool isAnyMovementKeyPressed, GameTime gameTime)
        {
            if (isAnyMovementKeyPressed)
            {
                if (!_isMoving)
                {
                    _elapsedTimeSinceLastMove = MoveDelay;
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
            var tileSize = GameConstants.GridSize;
            var oldPosition = positionComponent.Position;
            var newPosition = positionComponent.Position += movement * tileSize;
            _eventManager.Publish(new EntityMovedData(playerEntity, oldPosition, newPosition));
        }
    }

}