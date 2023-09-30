using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utmark_ECS.Systems.EventSystem;
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
            if (state.IsKeyDown(Keys.U)) MapAndPublishAction(InputAction.Use);
            else if (state.IsKeyDown(Keys.T)) MapAndPublishAction(InputAction.Throw);
            else if (state.IsKeyDown(Keys.G)) MapAndPublishAction(InputAction.PickUp);
                                                                           // Add other keys and their corresponding actions as necessary.
        }

        private void MapAndPublishAction(InputAction inputAction)
        {
            _eventManager.Publish(EventTypes.ActionEvent, this, inputAction);
        }
    }
}

