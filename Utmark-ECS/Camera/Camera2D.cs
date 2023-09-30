using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Utmark.Engine.Camera
{
    // ToDo: MAke the camera center on the player position when zoomed

    public class Camera2D
    {
        private const float DefaultZoom = 1.5f;
        private const float MinZoom = 1.2f;
        private const float MaxZoom = 2f;
        private readonly Viewport viewport;
        public Vector2 Position { get; private set; }
        public float Zoom { get; private set; }
        private bool isDirty; // a flag indicating whether the matrix needs to be recalculated

        public Camera2D(Viewport viewport)
        {
            this.viewport = viewport;
            Zoom = DefaultZoom;
        }

        private Matrix cachedMatrix;
        public void SetZoom(float zoom)
        {
            Zoom = MathHelper.Clamp(zoom, MinZoom, MaxZoom);
            isDirty = true;
        }

        public Matrix GetViewMatrix()
        {
            if (isDirty)
            {
                cachedMatrix =
                Matrix.CreateTranslation(new Vector3(-Position, 0.0f)) *
                Matrix.CreateTranslation(new Vector3(-viewport.Width * 0.5f, -viewport.Height * 0.5f, 0.0f)) *
                Matrix.CreateScale(Zoom, Zoom, 1) *
                Matrix.CreateTranslation(new Vector3(viewport.Width * 0.5f, viewport.Height * 0.5f, 0.0f));
                isDirty = false; // reset the flag after recalculating the matrix
            }
            return cachedMatrix;
        }

        public void Move(Vector2 offset)
        {
            Position += offset;
            isDirty = true;
        }

        public void SetPosition(Vector2 newPosition)
        {
            Position = newPosition - new Vector2(viewport.Width * 0.5f, viewport.Height * 0.5f);
            isDirty = true;
        }
    }
}
