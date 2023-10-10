using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Utmark.Engine.Settings.Screen
{
    public class ScreenSettings
    {
        private GraphicsDeviceManager graphics;

        public static int PreferredBackBufferWidth { get; private set; }
        public static int PreferredBackBufferHeight { get; private set; }

        public ScreenSettings(GraphicsDeviceManager graphics)
        {

            this.graphics = graphics;
            InitializeDefaults();
        }

        public void InitializeDefaults()
        {
            float aspectRatio = 16.0f / 9.0f;
            //PreferredBackBufferWidth = 800;
            //PreferredBackBufferHeight = 600;
            PreferredBackBufferWidth = (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.8f);
            PreferredBackBufferHeight = (int)(PreferredBackBufferWidth / aspectRatio);
            graphics.IsFullScreen = false;
            ApplyChanges();
        }

        public void SetResolution(int width, int height)
        {
            PreferredBackBufferWidth = width;
            PreferredBackBufferHeight = height;
            ApplyChanges();
        }

        public void ApplyChanges()
        {
            graphics.PreferredBackBufferWidth = PreferredBackBufferWidth;
            graphics.PreferredBackBufferHeight = PreferredBackBufferHeight;
            graphics.ApplyChanges();
        }

        public (int width, int height) GetCurrentScreenResolution()
        {
            return (graphics.PreferredBackBufferWidth,
                   graphics.PreferredBackBufferHeight);
        }

    }

}
