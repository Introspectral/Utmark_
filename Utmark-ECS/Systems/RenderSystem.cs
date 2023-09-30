using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Utmark.Engine.Camera;
using Utmark_ECS.Components;
using Utmark_ECS.Managers;

namespace Utmark_ECS.Systems
{
    public class RenderSystem
    {
        private ComponentManager _componentManager;
        private SpriteBatch _spriteBatch;
        private TileMap _tileMap; // Added a TileMap instance to the RenderSystem
        private ResourceManager _resourceManager;


        // Adjusted constructor to take TileMap as a parameter
        private Camera2D _camera;

        public RenderSystem(ComponentManager componentManager, SpriteBatch spriteBatch, TileMap tileMap, Camera2D camera, ResourceManager resourceManager)
        {
            _componentManager = componentManager ?? throw new ArgumentNullException(nameof(componentManager));
            _spriteBatch = spriteBatch ?? throw new ArgumentNullException(nameof(spriteBatch));
            _tileMap = tileMap ?? throw new ArgumentNullException(nameof(tileMap));
            _camera = camera ?? throw new ArgumentNullException(nameof(camera)); // Changed to accept Camera2D object.
            _resourceManager = resourceManager ?? throw new ArgumentNullException();
        }
        public void Draw()
        {
            _spriteBatch.Begin(transformMatrix: _camera.GetViewMatrix());
            _tileMap.Draw(_spriteBatch, _resourceManager.SpriteSheet, _resourceManager.Sprites);

            var entities = _componentManager.GetEntitiesWithComponents(typeof(RenderComponent), typeof(PositionComponent));

            foreach (var entity in entities)
            {
                if (_componentManager.TryGetComponent(entity, out RenderComponent renderComponent) &&
                    _componentManager.TryGetComponent(entity, out PositionComponent positionComponent))
                {
                    _spriteBatch.Draw(
                        renderComponent.Texture,
                        positionComponent.Position,
                        renderComponent.SourceRectangle,
                        renderComponent.Tint,
                        renderComponent.Rotation,
                        new Vector2(renderComponent.SourceRectangle.Width / 2, renderComponent.SourceRectangle.Height / 2),
                        1f, // Scale
                        SpriteEffects.None,
                        renderComponent.LayerDepth
                    );
                }
                else
                {
                    Console.WriteLine($"Warning: Entity {entity} has a missing or null RenderComponent or PositionComponent");
                }
            }
            _spriteBatch.End();
        }
    }
}
