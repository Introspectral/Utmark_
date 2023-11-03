using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Utmark_ECS.Components;
using Utmark_ECS.Entities;
using Utmark_ECS.Managers;

namespace Utmark_ECS.UI;
public class InventoryDisplayScreen : UIComponent
{
    private SpriteFont font;
    private readonly EventManager _eventManager;
    private InventoryComponent currentInventory;
    private readonly ComponentManager _componentManager;

    private Rectangle _rectangle;
    private Color _backgroundColor = new Color(0, 0, 0, 200);  // RGBA values
    private Texture2D _pixel;

    public InventoryDisplayScreen(SpriteFont font, EventManager eventManager, ComponentManager componentManager, int x, int y, int width, int height, Texture2D pixel)
    {
        this.font = font;
        _eventManager = eventManager;
        _componentManager = componentManager;

        Position = new Vector2(x, y);
        _rectangle = new Rectangle((int)Position.X, (int)Position.Y, width, height);
        _pixel = pixel;
    }

    public void SetCurrentEntityInventory(Entity entity)
    {
        currentInventory = _componentManager.GetComponent<InventoryComponent>(entity);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_pixel, _rectangle, _backgroundColor);

        int borderWidth = 2; // Set the width of the border

        // Draw the left border
        spriteBatch.Draw(_pixel, new Rectangle(_rectangle.X, _rectangle.Y, borderWidth, _rectangle.Height), Color.Gray);

        spriteBatch.DrawString(font, $"-: Inventory :-", new Vector2(Position.X + 16, Position.Y + 16), Color.White);

        if (currentInventory != null)
        {
            for (int i = 0; i < currentInventory.Items.Count; i++)
            {
                var itemPosition = new Vector2(Position.X + 32, (Position.Y + 48) + i * font.LineSpacing);
                var itemName = _componentManager.GetComponent<NameComponent>(currentInventory.Items[i]).Name;
                spriteBatch.DrawString(font, itemName, itemPosition, Color.White);
            }
        }
    }

    public override void Update(GameTime gameTime)
    {
        // Placeholder: Logic to handle UI updates if needed
    }
}



