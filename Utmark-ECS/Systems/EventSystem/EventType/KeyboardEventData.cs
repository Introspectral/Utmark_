using Microsoft.Xna.Framework.Input;

namespace Utmark_ECS.Systems.EventSystem.EventType
{
    public class KeyboardEventData
    {
        public Keys PressedKey { get; }

        public KeyboardEventData(Keys pressedKey)
        {
            PressedKey = pressedKey;
        }
    }

}
