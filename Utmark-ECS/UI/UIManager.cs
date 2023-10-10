using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utmark_ECS.Components;

namespace Utmark_ECS.UI
{
    public class UIManager
    {
        private List<UIComponent> _components = new List<UIComponent>();

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
