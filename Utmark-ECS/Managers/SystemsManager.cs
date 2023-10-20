using Microsoft.Xna.Framework;
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
            // Optional: Check for null and whether the system already exists.
            _systems.Add(system);
        }

        public void RemoveSystem(ISystem system)
        {
            // Optional: Check for null.
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

        // If your systems need to handle events, you can add a method for that.
        //public void HandleEvent(EventType eventType, params object[] args)
        //{
        //    foreach (var system in _systems)
        //    {
        //        // Assuming systems have a method like 'HandleEvent'.
        //        // system.HandleEvent(eventType, args);
        //    }
        //}

        // Method to clear all systems, useful for cleanup.
        public void ClearSystems()
        {
            _systems.Clear();
        }
    }

    // Usage:
    // var systemManager = new SystemManager();
    // systemManager.AddSystem(new RenderSystem());
    // ... other systems ...
    // systemManager.UpdateSystems(gameTime); // In your game loop update.
    // systemManager.DrawSystems(spriteBatch); // In your game loop draw.
}
