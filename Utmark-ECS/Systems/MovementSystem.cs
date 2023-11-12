using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Utmark_ECS.Components;
using Utmark_ECS.Entities;
using Utmark_ECS.Enums;
using Utmark_ECS.Intefaces;
using Utmark_ECS.Managers;
using Utmark_ECS.Systems.EventSystem.EventType;
using Utmark_ECS.Systems.EventSystem.EventType.ActionEvents;

namespace Utmark_ECS.Systems
{
    public class MovementSystem : ISystem
    {
        private readonly ComponentManager _componentManager;
        private readonly SpatialGrid _spatialGrid;
        private readonly EventManager _eventManager;

        public MovementSystem(ComponentManager componentManager, SpatialGrid spatialGrid, EventManager eventManager)
        {
            _componentManager = componentManager;
            _spatialGrid = spatialGrid;
            _eventManager = eventManager;


            _eventManager.Subscribe<ActionEventData>(OnMoveEntityRequest);
        }

        private void OnMoveEntityRequest(ActionEventData data)
        {

            if (data != null)
            {

                Entity entity = GetEntityFromEventData(data);

                var positionComponent = _componentManager.GetComponent<PositionComponent>(entity);
                if (positionComponent == null) return;

                Vector2 newPosition = positionComponent.Position;


                switch (data.State)
                {
                    case InputAction.MoveUp:
                        newPosition.Y -= 16;
                        break;
                    case InputAction.MoveDown:
                        newPosition.Y += 16;
                        break;
                    case InputAction.MoveLeft:
                        newPosition.X -= 16;
                        break;
                    case InputAction.MoveRight:
                        newPosition.X += 16;
                        break;
                    case InputAction.MoveUpLeft:
                        newPosition.Y -= 16;
                        newPosition.X -= 16;
                        break;
                    case InputAction.MoveUpRight:
                        newPosition.Y -= 16;
                        newPosition.X += 16;
                        break;
                    case InputAction.MoveDownLeft:
                        newPosition.Y += 16;
                        newPosition.X -= 16;
                        break;
                    case InputAction.MoveDownRight:
                        newPosition.Y += 16;
                        newPosition.X += 16;
                        break;
                    default:
                        return;
                }
                MoveEntity(entity, newPosition);
            }
        }

        private bool IsValidPosition(Entity entity, Vector2 newPosition)
        {
            throw new NotImplementedException();
        }

        private Entity GetEntityFromEventData(ActionEventData data)
        {
            return data.Entity;
        }
        public void MoveEntity(Entity entity, Vector2 newPosition)
        {
            var positionComponent = _componentManager.GetComponent<PositionComponent>(entity);
            if (positionComponent == null) return;
            var oldPosition = positionComponent.Position;

            positionComponent.Position = newPosition;

            _spatialGrid.MoveEntity(entity, oldPosition, newPosition);

            _eventManager.Publish(new EntityMovedEvent(entity, oldPosition, newPosition));
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
