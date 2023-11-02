using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Utmark.Engine.Camera
{

    public class Camera2D
    {
        private const float DefaultZoom = 1.6f;
        private const float MinZoom = 1.2f;
        private const float MaxZoom = 2f;
        private readonly Viewport viewport;

        // Direct assignments for constant vectors
        private static readonly Vector3 ZeroVector3 = new Vector3(0, 0, 0);

        public Vector2 Position { get; private set; }
        public float Zoom { get; private set; }

        private bool isDirty = true;  // Set to true initially to ensure matrix gets computed first time
        private Matrix cachedMatrix;

        public Camera2D(Viewport viewport)
        {
            this.viewport = viewport;
            Zoom = DefaultZoom;
        }

        public void SetZoom(float zoom)
        {
            Zoom = MathHelper.Clamp(zoom, MinZoom, MaxZoom);
            isDirty = true;
        }

        public Matrix ViewMatrix
        {
            get
            {
                if (isDirty)
                {
                    var translation = Matrix.CreateTranslation(-Position.X, -Position.Y, 0.0f);
                    var offset = Matrix.CreateTranslation(-viewport.Width * 0.5f, -viewport.Height * 0.5f, 0.0f);
                    var scale = Matrix.CreateScale(Zoom, Zoom, 1);
                    var revertOffset = Matrix.CreateTranslation(viewport.Width * 0.5f, viewport.Height * 0.5f, 0.0f);

                    cachedMatrix = translation * offset * scale * revertOffset;
                    isDirty = false;
                }
                return cachedMatrix;
            }
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
