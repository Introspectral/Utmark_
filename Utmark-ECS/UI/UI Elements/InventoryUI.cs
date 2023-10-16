using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Utmark_ECS.Components;
using Utmark_ECS.Entities;
using Utmark_ECS.Systems.EventSystem;
using Utmark_ECS.Systems.EventSystem.EventType;
using static System.Net.Mime.MediaTypeNames;

namespace Utmark_ECS.UI.UI_Elements
{
    public class InventoryUI : UIComponent
    {
        private List<ItemComponent> _items = new List<ItemComponent>();
        private Vector2 _position { get; set; }   
        private Rectangle _rectangle;
        private Color _backgroundColor = Color.Black;
        private Texture2D _pixel;
        private SpriteFont _font;
        private EventManager _eventManager;
        public InventoryUI(int width, int height, Texture2D pixel, SpriteFont font, Vector2 position, Entity entity, EventManager eventManager)
        {
            _rectangle = new Rectangle(0, 0, width, height);
            _pixel = pixel;
            _font = font;
            _position = position;
            _eventManager = eventManager;


            _eventManager.Subscribe<InventoryUpdate>(OnInventoryUpdate);
        }

        private void OnInventoryUpdate(InventoryUpdate update)
        {
            _items.Add(update.Item);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(_pixel, _rectangle, _backgroundColor);
            int borderWidth = 2; // Set the width of the border
            spriteBatch.Draw(_pixel, new Rectangle(_rectangle.X, _rectangle.Y + _rectangle.Height - borderWidth, _rectangle.Width, borderWidth), Color.Gray);
            var messagePosition = new Vector2(Position.X + 16, (Position.Y + 16) * _font.LineSpacing);
            spriteBatch.DrawString(_font, $"Inventory", new Vector2(messagePosition.X, messagePosition.Y - 32), Color.White);
            foreach (var item in _items) 
            {
                spriteBatch.DrawString(_font, $"{item.Name}", messagePosition,  Color.White);
                messagePosition.Y += _font.MeasureString(item.Name).Y;
            }


        }

        public override void Update(GameTime gameTime)
        {
            //throw new NotImplementedException();
        }
    }
}
