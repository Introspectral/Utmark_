using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Utmark_ECS.Intefaces
{
    public interface ISystem
    {
        void Draw(SpriteBatch spriteBatch);
        void Update(GameTime gameTime); // For game logic.

    }
}
