using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Utmark_ECS.Systems.Render
{
    public class Canvas : IDisposable
    {
        public readonly RenderTarget2D _target;
        private readonly GraphicsDevice _graphicsDevice;
        public Rectangle _destinationRectangle;
        private SpriteBatch _spriteBatch;

        public Canvas(GraphicsDevice graphicsDevice, int width, int height)
        {
            _graphicsDevice = graphicsDevice ?? throw new ArgumentNullException(nameof(graphicsDevice));
            _spriteBatch = new SpriteBatch(_graphicsDevice);
            _target = new RenderTarget2D(_graphicsDevice, width, height);
        }

        public void SetDestinationRectangle()
        {
            var screenSize = _graphicsDevice.PresentationParameters.Bounds;

            float scaleX = (float)screenSize.Width / _target.Width;
            float scaleY = (float)screenSize.Height / _target.Height;
            float scale = Math.Min(scaleX, scaleY);

            int newWidth = (int)(_target.Width * scale);
            int newHeight = (int)(_target.Height * scale);

            int positionX = (screenSize.Width - newWidth) / 2;
            int positionY = (screenSize.Height - newHeight) / 2;

            _destinationRectangle = new Rectangle(positionX, positionY, newWidth, newHeight);
        }

        public void Draw(Action<SpriteBatch> drawAction)
        {
            // Set the render target to the canvas
            _graphicsDevice.SetRenderTarget(_target);

            _graphicsDevice.Clear(Color.Transparent); // Clearing with transparent, adjust if needed

            _spriteBatch.Begin();

            drawAction?.Invoke(_spriteBatch); // Render everything encapsulated by the provided action

            _spriteBatch.End();

            // Resetting the render target to draw on the screen.
            _graphicsDevice.SetRenderTarget(null);

            // Now draw the Canvas content to the screen using the destination rectangle
            //_spriteBatch.Begin();
            //_spriteBatch.Draw(_target, _destinationRectangle, Color.White);
            //_spriteBatch.End();
        }

        public void Update()
        {
            // Implement any update logic here if required
        }

        public void Dispose()
        {
            _target?.Dispose();
            _spriteBatch?.Dispose();
        }
    }

}
