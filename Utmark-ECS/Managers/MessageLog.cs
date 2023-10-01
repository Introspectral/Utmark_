using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Utmark_ECS.Systems.EventHandlers;
using Utmark_ECS.Systems.EventSystem;

namespace Utmark_ECS.Managers
{
    public class MessageLog
    {
        private List<string> messages = new List<string>();
        private SpriteFont font;
        private int maxMessages = 10;
        private readonly EventManager _eventManager;

        public MessageLog(SpriteFont font, EventManager eventManager)
        {
            this.font = font;
            _eventManager = eventManager;

            _eventManager.Subscribe<MessageEvent>(HandleMessageEvent);
        }
        private void HandleMessageEvent(MessageEvent messageEvent)
        {
            AddMessage(messageEvent.Message);
        }
        public void AddMessage(string message)
        {
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
