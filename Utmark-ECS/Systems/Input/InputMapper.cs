using Microsoft.Xna.Framework.Input;
using Utmark_ECS.Systems.EventHandlers;
using Utmark_ECS.Systems.EventSystem;
using Utmark_ECS.Systems.EventSystem.EventType;
using static Utmark_ECS.Enums.EventTypeEnum;
using static Utmark_ECS.Enums.InputActionEnum;

namespace Utmark_ECS.Systems.Input
{
    public class InputMapper
    {
        private readonly EventManager _eventManager;

        public InputMapper(EventManager eventManager)
        {
            _eventManager = eventManager;
        }

        public void HandleInput(KeyboardState state)
        {
            // You may have a more complex logic here to handle multiple key presses, etc.
            if (state.IsKeyDown(Keys.U)) HandleAction(InputAction.Use);
            if (state.IsKeyDown(Keys.T)) HandleAction(InputAction.Throw);
            if (state.IsKeyDown(Keys.G)) HandleAction(InputAction.PickUp);

        }


        private void HandleAction(InputAction inputAction)
        {

            switch (inputAction)
            {
                case InputAction.Use:
                    _eventManager.Publish(new MessageEvent(this, $"ActionHandler - Handled Use Action"));
                    // Handle use action here
                    break;
                case InputAction.PickUp:
                    _eventManager.Publish(new MessageEvent(this, $"ActionHandler - Handled PickUp Action"));
                    // Handle pick up action here
                    break;
                case InputAction.Throw:
                    _eventManager.Publish(new MessageEvent(this, $"ActionHandler - Handled Throw Action"));
                    // Handle throw action here
                    break;
            }
        }

    }
}


