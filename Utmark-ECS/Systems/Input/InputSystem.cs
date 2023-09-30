﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Runtime.CompilerServices;
using Utmark_ECS.Components;
using Utmark_ECS.Entities;
using Utmark_ECS.Managers;
using Utmark_ECS.Systems.EventSystem;
using Utmark_ECS.Utilities;

namespace Utmark_ECS.Systems.Input
{
    public class InputSystem
    {
        private readonly ComponentManager _componentManager;
        private readonly InputMapper _inputMapper;
        private readonly EventManager _eventManager;
        private float _elapsedTimeSinceLastMove = 0f;
        private const float MoveDelay = 0.20f;
        private bool _isMoving;

        public InputSystem(ComponentManager componentManager, EventManager eventManager, InputMapper inputMapper)
        {

            _componentManager = componentManager;
            _eventManager = eventManager;
            _inputMapper = inputMapper;
        }

        public void Update(GameTime gameTime)
        {
            try
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
                ExecuteMovement(movement, velocityComponent, positionComponent);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
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
                _inputMapper.HandleInput(state); // Let the InputMapper handle the input and publish the necessary events.
            }
        }

        private void UpdateMovementTimer(bool isAnyMovementKeyPressed, GameTime gameTime)
        {
            if (isAnyMovementKeyPressed)
            {
                _isMoving = true;
                _elapsedTimeSinceLastMove += (float)gameTime.ElapsedGameTime.TotalSeconds;
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

        private void ExecuteMovement(Vector2 movement, VelocityComponent velocityComponent, PositionComponent positionComponent)
        {
            if (movement == Vector2.Zero) return;

            var tileSize = GameConstants.GridSize; // Replace with actual tile size*/
            velocityComponent.Velocity = movement;
            positionComponent.Position += movement * tileSize;

            _elapsedTimeSinceLastMove = 0f;
        }

        private void LogError(Exception ex)
        {

        }
    }
}