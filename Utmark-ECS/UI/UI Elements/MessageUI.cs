using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text.RegularExpressions;
using Utmark_ECS.Components;
using Utmark_ECS.Managers;
using Utmark_ECS.Systems.EventHandlers;

namespace Utmark_ECS.UI.UI_Elements
{
    public class MessageUI : UIComponent
    {
        private List<List<Tuple<string, Color>>> messages = new List<List<Tuple<string, Color>>>();
        private SpriteFont font;
        private int maxMessages = 11;
        private readonly EventManager _eventManager;

        private Rectangle _rectangle;
        private Color _backgroundColor = new Color(0, 0, 0, 200);  // RGBA values
        private Texture2D _pixel;


        public MessageUI(SpriteFont font, EventManager eventManager, int x, int y, int width, int height, Texture2D pixel)
        {
            this.font = font;
            _eventManager = eventManager;
            _eventManager.Subscribe<MessageEvent>(HandleMessageEvent);

            Position = new Vector2(x, y);
            _rectangle = new Rectangle((int)Position.X, (int)Position.Y, width, height);
            _pixel = pixel;
        }

        private void HandleMessageEvent(MessageEvent messageEvent)
        {
            AddMessage(messageEvent.Message);
        }

        public void AddMessage(string message)
        {
            messages.Add(ParseMessage(message));

            if (messages.Count > maxMessages)
            {
                messages.RemoveAt(0);
            }
        }

        private List<Tuple<string, Color>> ParseMessage(string message)
        {
            var segments = new List<Tuple<string, Color>>();
            var regex = new Regex(@"\[color=(?<color>\w+)\](?<text>.*?)\[/color\]");
            var matches = regex.Matches(message);

            int lastEndPosition = 0;
            foreach (Match match in matches)
            {
                string beforeTagText = message.Substring(lastEndPosition, match.Index - lastEndPosition);
                segments.Add(Tuple.Create(beforeTagText, Color.White));

                Color color = Color.White;
                switch (match.Groups["color"].Value.ToLower())
                {
                    case "red": color = new Color(205, 92, 92); break;       // IndianRed
                    case "blue": color = new Color(100, 149, 237); break;    // CornflowerBlue
                    case "green": color = new Color(60, 179, 113); break;    // MediumSeaGreen
                    case "yellow": color = new Color(238, 232, 170); break;  // PaleGoldenrod
                    case "orange": color = new Color(255, 165, 79); break;   // SandyBrown
                    case "purple": color = new Color(147, 112, 219); break;  // MediumPurple
                    case "pink": color = new Color(255, 182, 193); break;    // LightPink
                    case "brown": color = new Color(160, 82, 45); break;     // Sienna
                    case "cyan": color = new Color(72, 209, 204); break;     // MediumTurquoise
                    case "lime": color = new Color(50, 205, 50); break;      // LimeGreen
                    case "indigo": color = new Color(75, 0, 130); break;     // Indigo (lightened a bit)
                    case "gold": color = new Color(218, 165, 32); break;     // Goldenrod
                    case "olive": color = new Color(107, 142, 35); break;    // OliveDrab
                    case "beige": color = new Color(245, 245, 220); break;   // Beige
                    case "teal": color = new Color(0, 128, 128); break;      // Teal
                    case "navy": color = new Color(0, 0, 128); break;        // Navy (standard navy is very dark)
                    default: color = Color.White; break;                     // Default to white if no matching color found
                }

                string coloredText = match.Groups["text"].Value;
                segments.Add(Tuple.Create(coloredText, color));

                lastEndPosition = match.Index + match.Length;
            }
            string afterTagText = message.Substring(lastEndPosition);
            segments.Add(Tuple.Create(afterTagText, Color.White));

            return segments;
        }

        public void Clear()
        {
            messages.Clear();
        }

        public override void Update(GameTime gameTime)
        {
            // Update messages or fade out old ones here
        }

        // Helper method to compute fade factor based on the message index
        private float ComputeFadeFactor(int index)
        {
            int reversedIndex = messages.Count - 1 - index; // Calculate reversed index to make newest messages (at the end) brighter

            const int fullColorCount = 5;
            if (reversedIndex < fullColorCount)
            {
                return 1.0f;  // Full color for the first 3 messages
            }
            else
            {
                // Gradually fade out older messages. Adjust the divisor for a quicker or slower fade.
                float fadeFactor = 1.0f - (reversedIndex - fullColorCount + 1) * 0.1f;
                return MathHelper.Clamp(fadeFactor, 0.2f, 1.0f); // Clamp to ensure it doesn't go below a certain threshold
            }
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_pixel, _rectangle, _backgroundColor);
            int borderWidth = 2; // Set the width of the border
            spriteBatch.Draw(_pixel, new Rectangle(_rectangle.X, _rectangle.Y, _rectangle.Width, borderWidth), Color.Gray);
            spriteBatch.DrawString(font, $"-: Messages :-", new Vector2(Position.X + 16, Position.Y + 16), Color.White);

            for (int i = 0; i < messages.Count; i++)
            {
                var fadeFactor = ComputeFadeFactor(i);
                var messagePosition = new Vector2(Position.X + 32, (Position.Y + 48) + i * font.LineSpacing);

                foreach (var segment in messages[i])
                {
                    var text = segment.Item1;
                    var color = segment.Item2 * fadeFactor; // Apply fade factor to the color (this multiplies each channel by the fade factor)
                    if (color == Color.White * fadeFactor)
                    {
                        // Special handling for white, make it go gray instead
                        color = new Color(fadeFactor, fadeFactor, fadeFactor);
                    }
                    spriteBatch.DrawString(font, text, messagePosition, color);
                    messagePosition.X += font.MeasureString(text).X;
                }
            }
        }

    }


}
