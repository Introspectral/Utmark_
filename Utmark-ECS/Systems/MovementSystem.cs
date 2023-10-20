using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Utmark_ECS.Components;
using Utmark_ECS.Entities;
using Utmark_ECS.Enums;
using Utmark_ECS.Intefaces;
using Utmark_ECS.Managers;
using Utmark_ECS.Systems.EventSystem.EventType;

namespace Utmark_ECS.Systems
{
    public class MovementSystem : ISystem
    {
        private readonly ComponentManager _componentManager;
        private readonly SpatialGrid _spatialGrid; // Used for efficient spatial queries.
        private readonly EventManager _eventManager; // Used for publishing or subscribing to events.

        public MovementSystem(ComponentManager componentManager, SpatialGrid spatialGrid, EventManager eventManager)
        {
            _componentManager = componentManager;
            _spatialGrid = spatialGrid;
            _eventManager = eventManager;

            // You might want to subscribe to events related to entity movement here.
            // For example:
            _eventManager.Subscribe<ActionEventData>(OnMoveEntityRequest);
        }

        private void OnMoveEntityRequest(ActionEventData data)
        {
            // Assuming that ActionEventData contains information about the entity trying to move
            // and the action they're trying to perform (in this case, movement actions).
            if (data != null)
            {
                // You might have a method to get the entity based on the data, or the entity itself might be part of the event data.
                Entity entity = GetEntityFromEventData(data); // This is a placeholder. Your method may vary.

                // Assuming you have a way to get the current position of the entity.
                var positionComponent = _componentManager.GetComponent<PositionComponent>(entity);
                if (positionComponent == null) return; // Can't move an entity without a position.

                Vector2 newPosition = positionComponent.Position; // Start with the current position.

                // Determine the new position based on the input action.
                // We're assuming a simple grid-based movement for the sake of this example.
                switch (data.State) // Assuming 'Action' is a property of ActionEventData that specifies the action.
                {
                    case InputAction.MoveUp:
                        newPosition.Y -= 16; // Move up by one unit.
                        break;
                    case InputAction.MoveDown:
                        newPosition.Y += 16; // Move down by one unit.
                        break;
                    case InputAction.MoveLeft:
                        newPosition.X -= 16; // Move left by one unit.
                        break;
                    case InputAction.MoveRight:
                        newPosition.X += 16; // Move right by one unit.
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
                        return; // If it's not a movement action, we don't handle it here.
                }

                // Validate the new position if needed (e.g., check boundaries, collision, etc.)
                //if (IsValidPosition(entity, newPosition)) // This is a hypothetical method; you'd need to implement this.
                //{
                // If valid, move the entity in your system.
                MoveEntity(entity, newPosition); // Assuming MoveEntity is a method in your MovementSystem.
                //}
                //else
                //{
                //    // Handle what happens if the position is not valid.
                //    // Maybe trigger an event or effect to indicate the movement is blocked.
                //}
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
        // This method could be called by an event handler or directly from the game loop/update method.
        public void MoveEntity(Entity entity, Vector2 newPosition)
        {
            // First, get the necessary components. If the entity can't be moved, we do nothing.
            var positionComponent = _componentManager.GetComponent<PositionComponent>(entity);
            if (positionComponent == null) return; // Can't move an entity without a position.

            var oldPosition = positionComponent.Position;

            // You could perform collision detection or rule validation here to determine if the newPosition is valid.
            // ...

            // If everything is okay, then we proceed with the move.
            positionComponent.Position = newPosition; // Update the entity's position.

            // Inform the SpatialGrid or other interested systems that the entity has moved.
            // The SpatialGrid may update its internal data, and other systems might check for interactions at the new position.
            _spatialGrid.MoveEntity(entity, oldPosition, newPosition);
            _eventManager.Publish(new MessagesEvent($"Player Position {newPosition}"));
            // After moving, you could publish an event indicating the entity has moved, allowing other systems to react as necessary.
            _eventManager.Publish(new EntityMovedEvent(entity, oldPosition, newPosition));
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }

        // Other methods and event handlers related to movement could be placed here.
    }

}
