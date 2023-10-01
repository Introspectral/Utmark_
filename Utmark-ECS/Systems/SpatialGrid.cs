using Microsoft.Xna.Framework;
using Utmark_ECS.Entities;
using Utmark_ECS.Systems.EventHandlers;
using Utmark_ECS.Systems.EventSystem;
using Utmark_ECS.Systems.EventSystem.EventType;

namespace Utmark_ECS.Systems
{
    // Entities are added to the spatialGrid in the CreateEntity method for now.

    public class SpatialGrid
    {
        private readonly int cellSize;
        private readonly Dictionary<Point, List<Entity>> grid = new();
        private TileMap _tileMap;
        private EventManager _eventManager;

        public event Action<Entity, Vector2>? EntityMoved;
        public event Action<Entity, Vector2>? CollisionCheck;
        public event Action<Entity, Vector2>? EntityRemoved;

        public SpatialGrid(int cellSize, EventManager eventManager)
        {
            this.cellSize = cellSize;
            _eventManager = eventManager;

            _eventManager.Subscribe<EntityMovedData>(OnMove);
            _eventManager.Subscribe<InputEventData>(OnInputReceived);

            _eventManager.Publish(new MessageEvent(this, $"This is OnInputRecieved"));
        }
        public void SetTileMap(TileMap tileMap) => _tileMap = tileMap;

        private void OnInputReceived(InputEventData inputData)
        {

            // actions based on input. PickUp, Attack etc

        }


        private void OnMove(EntityMovedData moveData)
        {
            MoveEntity(moveData.Entity, moveData.OldPosition, moveData.NewPosition);
            _tileMap.UpdateEntityTile(moveData.Entity, moveData.NewPosition);

        }

        private void CollisionActivate(Entity entityA, Entity entityB, Vector2 possition)
        {
            _eventManager.Publish(new CollisionEventData(entityA, entityB, possition));

        }

        public void MoveEntity(Entity entity, Vector2 oldPosition, Vector2 newPosition)
        {
            var oldCell = GetCellForPosition(oldPosition);
            var newCell = GetCellForPosition(newPosition);

            // If the entity has indeed moved to a different cell, then remove it from the old cell and add it to the new cell.
            if (oldCell != newCell)
            {
                GetEntitiesInCell(newPosition);

                if (grid.TryGetValue(oldCell, out var entities))
                    entities.Remove(entity); // Remove from the old cell.

                AddEntity(entity, newPosition); // Add to the new cell.

                // Check for collisions after the entity has been added to the new cell.
                var entitiesInNewCell = GetEntitiesInCell(newPosition);
                foreach (var otherEntity in entitiesInNewCell)
                {
                    if (otherEntity != entity) // prevent self-collision
                    {
                        CollisionActivate(entity, otherEntity, newPosition);
                        break; // Stop after the first collision is found.
                    }
                }
            }
        }

        public void AddEntity(Entity entity, Vector2 position)
        {
            var cell = GetCellForPosition(position);
            if (!grid.TryGetValue(cell, out var entities))
                grid[cell] = entities = new List<Entity>();
            entities.Add(entity);
        }
        public void RemoveEntity(Entity entity, Vector2 position)
        {
            var cell = GetCellForPosition(position);

            if (!grid.TryGetValue(cell, out var entities))
                grid[cell] = entities = new List<Entity>();
            entities.Remove(entity);
            EntityRemoved?.Invoke(entity, position);
        }

        public List<Entity> GetEntitiesInRegion(Rectangle region)
        {
            var entities = new List<Entity>();
            for (int x = region.Left / cellSize; x <= region.Right / cellSize; x++)
                for (int y = region.Top / cellSize; y <= region.Bottom / cellSize; y++)
                    entities.AddRange(grid.GetValueOrDefault(new Point(x, y)) ?? Enumerable.Empty<Entity>());
            return entities;
        }

        public List<Entity> GetEntitiesInCell(Vector2 position)
        {
            return grid.GetValueOrDefault(GetCellForPosition(position)) ?? new List<Entity>();
        }


        public List<Entity> GetEntitiesInAdjacentCells(Vector2 position)
        {
            var entities = new List<Entity>();
            var cell = GetCellForPosition(position);

            for (int x = cell.X - 1; x <= cell.X + 1; x++)
                for (int y = cell.Y - 1; y <= cell.Y + 1; y++)
                    if (x != cell.X || y != cell.Y) // Exclude the center cell
                        entities.AddRange(grid.GetValueOrDefault(new Point(x, y)) ?? Enumerable.Empty<Entity>());

            return entities;
        }

        private Point GetCellForPosition(Vector2 position) =>
            new((int)(position.X / cellSize), (int)(position.Y / cellSize));

        public int CountEntities()
        {
            int count = 0;
            foreach (var cell in grid.Values)
            {
                count += cell.Count;
            }
            return count;
        }
    }
}
