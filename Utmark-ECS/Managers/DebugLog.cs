using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Utmark_ECS.Systems.EventSystem;
using Utmark_ECS.Systems.EventSystem.EventType;

namespace Utmark_ECS.Managers
{
    public class DebugLog
    {
        private List<string> messages = new List<string>();
        private SpriteFont font;
        private int maxMessages = 10;
        private EventManager _eventManager;

        public DebugLog(SpriteFont font, EventManager eventManager)
        {
            this.font = font;
            _eventManager = eventManager;

            _eventManager.Subscribe(Enums.EventTypeEnum.EventTypes.Message, AddMessage);

        }


        public void AddMessage(EventData data)
        {
            var message = data.Data.ToString();
            messages.Add(message);

            if (messages.Count > maxMessages)
            {
                messages.RemoveAt(0);
            }

        }

        public void Clear()
        {
            messages.Clear();
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            for (int i = 0; i < messages.Count; i++)
            {
                var messagePosition = new Vector2(position.X, position.Y + i * font.LineSpacing);
                spriteBatch.DrawString(font, messages[i], messagePosition, Color.White);
            }
        }
    }
}
