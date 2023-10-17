using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Utmark_ECS.Components;

namespace Utmark_ECS.UI
{
    public class UIManager
    {
        private List<UIComponent> _components = new List<UIComponent>();

        public UIManager()
        {
            _components = new List<UIComponent>();
            // Initialize the context menu here or from outside via a method.
        }

        public void AddComponent(UIComponent component)
        {
            _components.Add(component);
        }
        public void Update(GameTime gameTime)
        {
            foreach (var component in _components)
            {
                component.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var component in _components)
            {
                component.Draw(spriteBatch);
            }
        }
    }
}
