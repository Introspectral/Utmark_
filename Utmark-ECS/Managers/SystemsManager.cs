﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Utmark_ECS.Intefaces;

namespace Utmark_ECS.Managers
{

    public class SystemManager
    {
        private readonly List<ISystem> _systems;

        public SystemManager()
        {
            _systems = new List<ISystem>();
        }

        public void AddSystem(ISystem system)
        {
            _systems.Add(system);
        }

        public void RemoveSystem(ISystem system)
        {
            _systems.Remove(system);
        }

        public void UpdateSystems(GameTime gameTime)
        {
            foreach (var system in _systems)
            {
                system.Update(gameTime);
            }
        }

        public void DrawSystems(SpriteBatch spriteBatch)
        {
            foreach (var system in _systems)
            {
                system.Draw(spriteBatch);
            }
        }

        public void ClearSystems()
        {
            _systems.Clear();
        }
    }
}
