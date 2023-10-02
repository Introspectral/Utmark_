using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Utmark_ECS.Components;
using Utmark_ECS.Entities;
using Utmark_ECS.Managers;
using Utmark_ECS.Systems.EventHandlers;
using Utmark_ECS.Systems.EventSystem;
using Utmark_ECS.Systems.EventSystem.EventType;
using Utmark_ECS.Utilities;
using static Utmark_ECS.Enums.EventTypeEnum;

namespace Utmark_ECS.Systems.Input
{
    public class InputSystem
    {
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

            var state = Keyboard.GetState();
            HandleKeyRelease(state);

            var isAnyMovementKeyPressed = IsAnyMovementKeyPressed(state);
            UpdateMovementTimer(isAnyMovementKeyPressed, gameTime);

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



        private void HandleKeyRelease(KeyboardState state)
        {

            if (!IsAnyMovementKeyPressed(state) && _isMoving)
            {
                _elapsedTimeSinceLastMove = MoveDelay;
                _isMoving = false;
            }
            else if (IsActionKeyPressed(state))
            {
                const string actionId = "Action";
                if (_cooldownManager.IsCooldownExpired(actionId))
                {
                    _inputMapper.HandleInput(state); // Let the InputMapper handle the input and publish the necessary events.
                    _cooldownManager.ActivateCooldown(actionId, 0.2f); // 5 seconds cooldown
                }

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
            _eventManager.Publish(new MessageEvent(this, "This is a message."));
            _elapsedTimeSinceLastMove = 0f;
            var tileSize = GameConstants.GridSize; // Replace with actual tile size*/
            var oldPosition = positionComponent.Position;
            var newPosition = positionComponent.Position += movement * tileSize;
            _eventManager.Publish(new EntityMovedData(playerEntity, oldPosition, newPosition));

        }
    }
}