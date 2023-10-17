using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Utmark.Engine.Camera;
using Utmark_ECS.Components;
using Utmark_ECS.Managers;
using Utmark_ECS.UI;

namespace Utmark_ECS.Systems.Render
{
    public class RenderSystem
    {
        private ComponentManager _componentManager;
        private SpriteBatch _spriteBatch;
        private TileMap _tileMap;
        private ResourceManager _resourceManager;
        private UIManager _uiManager;
        private RenderTarget2D _renderTarget;
        private GraphicsDevice _graphicsDevice;
        private float _scale;
        private int _scaledWidth, _scaledHeight, _positionX, _positionY;
        private Rectangle _destRect;


        // Adjusted constructor to take TileMap as a parameter
        private Camera2D _camera;

        public RenderSystem(ComponentManager componentManager, SpriteBatch spriteBatch, TileMap tileMap, Camera2D camera, ResourceManager resourceManager, UIManager uIManager, GraphicsDevice graphicsDevice, int virtualWidth, int virtualHeight)
        {
            _componentManager = componentManager ?? throw new ArgumentNullException(nameof(componentManager));
            _spriteBatch = spriteBatch ?? throw new ArgumentNullException(nameof(spriteBatch));
            _tileMap = tileMap ?? throw new ArgumentNullException(nameof(tileMap));
            _camera = camera ?? throw new ArgumentNullException(nameof(camera)); // Changed to accept Camera2D object.
            _resourceManager = resourceManager ?? throw new ArgumentNullException();
            _uiManager = uIManager;
            _graphicsDevice = graphicsDevice;
            _renderTarget = new RenderTarget2D(graphicsDevice, virtualWidth, virtualHeight);
        }

        private void UpdateScalingValues()
        {
            var screenSize = _graphicsDevice.PresentationParameters.Bounds;
            _scale = Math.Min((float)screenSize.Width / _renderTarget.Width, (float)screenSize.Height / _renderTarget.Height);
            _scaledWidth = (int)(_renderTarget.Width * _scale);
            _scaledHeight = (int)(_renderTarget.Height * _scale);
            _positionX = (screenSize.Width - _scaledWidth) / 2;
            _positionY = (screenSize.Height - _scaledHeight) / 2;
            _destRect = new Rectangle(_positionX, _positionY, _scaledWidth, _scaledHeight);
        }

        public void Draw()
        {
            _graphicsDevice.SetRenderTarget(_renderTarget);
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, transformMatrix: _camera.ViewMatrix);

            var entities = _componentManager.GetEntitiesWithComponents(typeof(RenderComponent), typeof(PositionComponent));

            _tileMap.Draw(_spriteBatch, _resourceManager.SpriteSheet, _resourceManager.Sprites);

            foreach (var entity in entities)
            {
                if (_componentManager.TryGetComponents(entity, out RenderComponent renderComponent, out PositionComponent positionComponent))
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
            }
            _spriteBatch.End();
            // Draw the UI directly to the Canvas' RenderTarget
            _spriteBatch.Begin();
            _uiManager.Draw(_spriteBatch);
            _spriteBatch.End();
            _graphicsDevice.SetRenderTarget(null);
            _graphicsDevice.Clear(Color.DarkGray); // Or whatever your background color is

            // Draw the off-screen buffer (_renderTarget) to the screen, scaled as needed
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            UpdateScalingValues();
            _spriteBatch.Draw(_renderTarget, _destRect, Color.White);
            _spriteBatch.End();

        }


    }
}
