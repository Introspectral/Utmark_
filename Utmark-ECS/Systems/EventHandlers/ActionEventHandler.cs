using Utmark_ECS.Systems.EventSystem;
using Utmark_ECS.Systems.EventSystem.EventType;
using Utmark_ECS.Systems.Input;
using static Utmark_ECS.Enums.EventTypeEnum;
using static Utmark_ECS.Enums.InputActionEnum;

namespace Utmark_ECS.Systems.EventHandlers
{
    public class ActionHandler
    {
        private readonly EventManager _eventManager;
        private readonly InputMapper _inputMapper;

        public ActionHandler(EventManager eventManager, InputMapper inputMapper)
        {
            _eventManager = eventManager;
            _inputMapper = inputMapper;
            _eventManager.Subscribe(EventTypes.ActionEvent, HandleAction);
        }

        private void HandleAction(EventData eventData)
        {
            if (eventData.Data is InputAction inputAction)
            {
                switch (inputAction)
                {
                    case InputAction.Use:
                        _eventManager.Publish(EventTypes.Message, this, $"ActionHandler - Handled Use Action");
                        // Handle use action here
                        break;
                    case InputAction.PickUp:
                        // Handle pick up action here
                        _eventManager.Publish(EventTypes.Message, this, $"ActionHandler - Handled Pick Up Action");
                        break;
                    case InputAction.Throw:
                        // Handle throw action here
                        _eventManager.Publish(EventTypes.Message, this, $"ActionHandler - Handled Throw Action");
                        break;
                }
            }

        }
    }
}


// Test