using Microsoft.Xna.Framework.Input;

namespace Utmark_ECS.Systems.EventSystem.EventType
{
    public class InputEventData
    {
        public enum InputState
        {
            Pressed,
            Released
        }

        public Keys Keys { get; }
        public InputState State { get; }

        public InputEventData(Keys keys, InputState state)
        {
            Keys = keys;
            State = state;
        }
    }
}
