using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Utmark_ECS.Components;
using Utmark_ECS.Entities;
using Utmark_ECS.Enums;
using Utmark_ECS.Intefaces;
using Utmark_ECS.Managers;
using Utmark_ECS.Systems.EventHandlers;
using Utmark_ECS.Systems.EventSystem.EventType;
using Utmark_ECS.Systems.EventSystem.EventType.ActionEvents;

namespace Utmark_ECS.Systems.Input
{
    public class InputMapper : ISystem
    {
        private readonly EventManager _eventManager;
        private readonly ComponentManager _componentManager;
        // Maps game actions to keys
        private Dictionary<InputAction, Keys> _keyBindings;

        public InputMapper(EventManager eventManager, ComponentManager componentManager)
        {
            _eventManager = eventManager;
            _componentManager = componentManager;

            _keyBindings = new Dictionary<InputAction, Keys>
            {
                { InputAction.MoveUp, Keys.W },
                { InputAction.MoveDown, Keys.S },
                { InputAction.MoveLeft, Keys.A },
                { InputAction.MoveRight, Keys.D },
                { InputAction.MoveUpLeft, Keys.Q },
                { InputAction.MoveUpRight, Keys.E },
                { InputAction.MoveDownLeft, Keys.Z },
                { InputAction.MoveDownRight, Keys.C },
                { InputAction.Search, Keys.R },
                { InputAction.PickUp, Keys.G },
                { InputAction.Look, Keys.F },
                { InputAction.Use, Keys.T },
             };

            _eventManager.Subscribe<KeyboardEventData>(HandleInput);
        }

        public void ChangeKeyBinding(InputAction action, Keys newKey)
        {
            // Change the key associated with an action
            if (_keyBindings.ContainsKey(action))
            {
                _keyBindings[action] = newKey;
            }
            else
            {
                _keyBindings.Add(action, newKey);
            }
        }
        public Entity GetPlayerEntity() =>
            _componentManager.GetEntitiesWithComponents(typeof(InputComponent)).FirstOrDefault();

        public void HandleInput(KeyboardEventData eventData)
        {
            var playerEntity = GetPlayerEntity();
            // Check if the pressed key is associated with any action
            var action = _keyBindings.FirstOrDefault(kvp => kvp.Value == eventData.PressedKey).Key;
            if (!action.Equals(default(InputAction)))
            {
                // If the key corresponds to an action, handle it accordingly
                // For now, we're just publishing a message, but you could invoke more complex functionality
                _eventManager.Publish(new ActionEventData(action, playerEntity));
            }
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}


