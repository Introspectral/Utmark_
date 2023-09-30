using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Utmark_ECS.Managers
{
    public class MessageLog
    {
        private List<string> messages = new List<string>();
        private SpriteFont font;
        private int maxMessages = 10;


        public MessageLog(SpriteFont font)
        {
            this.font = font;
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
