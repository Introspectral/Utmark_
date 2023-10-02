﻿using Microsoft.Xna.Framework.Input;
using Utmark_ECS.Systems.EventSystem;
using Utmark_ECS.Systems.EventSystem.EventType;
using static Utmark_ECS.Enums.InputActionEnum;

namespace Utmark_ECS.Systems.Input
{
    public class InputMapper
    {
        private readonly EventManager _eventManager;
        private readonly CooldownManager _cooldownManager = new();
        public InputMapper(EventManager eventManager)
        {
            _eventManager = eventManager;

        }

        public void HandleInput(KeyboardState state)
        {
            const string actionId = "PlayerActionInput";
            if (_cooldownManager.IsCooldownExpired(actionId))
            {
                // You may have a more complex logic here to handle multiple key presses, etc.
                if (state.IsKeyDown(Keys.U)) HandleAction(InputAction.Use);
                if (state.IsKeyDown(Keys.T)) HandleAction(InputAction.Throw);
                if (state.IsKeyDown(Keys.G)) HandleAction(InputAction.PickUp);
            }
            _cooldownManager.ActivateCooldown(actionId, 0.05f);

        }


        private void HandleAction(InputAction inputAction)
        {

            switch (inputAction)
            {
                case InputAction.Use:
                    //_eventManager.Publish(new MessageEvent(this, $"ActionHandler - Handled Use Action"));
                    _eventManager.Publish(new ActionRequestEvent(inputAction));
                    // Handle use action here
                    break;
                case InputAction.PickUp:
                    //_eventManager.Publish(new MessageEvent(this, $"ActionHandler - Handled PickUp Action"));
                    _eventManager.Publish(new ActionRequestEvent(inputAction));
                    // Handle pick up action here
                    break;
                case InputAction.Throw:
                    //_eventManager.Publish(new MessageEvent(this, $"ActionHandler - Handled Throw Action"));
                    _eventManager.Publish(new ActionRequestEvent(inputAction));
                    // Handle throw action here
                    break;
            }
        }

    }
}


