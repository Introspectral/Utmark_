using Microsoft.Xna.Framework;
using Utmark_ECS.Entities;
using Utmark_ECS.Managers;
using Utmark_ECS.Systems.EventSystem.EventType;
using Utmark_ECS.Systems.EventSystem.EventType.ActionEvents;

namespace Utmark_ECS.Systems
{
    public class SpatialGrid
    {
        private readonly int _cellSize;
        private readonly Dictionary<Point, List<Entity>> _grid;
        private readonly EventManager? _eventManager;
        public event Action<Entity, Vector2>? EntityMoved;
        public event Action<Entity, Vector2>? CollisionCheck;
        public event Action<Entity, Vector2>? EntityRemoved;
        public event Action<Entity, Vector2>? TileSearch;
        public SpatialGrid(int cellSize, EventManager eventManager)
        {
            _cellSize = cellSize;
            _eventManager = eventManager;
            _grid = new Dictionary<Point, List<Entity>>();
            SubscribeToEvents();

        }

        private void SubscribeToEvents()
        {
            _eventManager.Subscribe<RemoveEntityEventData>(OnEntityRemoved);
            _eventManager.Subscribe<LookRequestEventData>(OnLookRequest);
            _eventManager.Subscribe<SearchRequestEventData>(OnSearchRequest);
            _eventManager.Subscribe<UseRequestEventData>(OnUseRequest);
            _eventManager.Subscribe<PickUpRequestEventData>(OnPickUpRequest);

        }



        private void OnPickUpRequest(PickUpRequestEventData data)
        {
            // TODO: OnPickUpRequest - This will need a pop-up menu down the line that displays what items are pressent at the location so that the player can chose how to interact with the items. 
            // examples may be: Use, Hide, PickUp, Place etc.

            var itemsInCell = GetEntitiesInCell(data.Position);
            // We will use this flag to determine if an item has been found.
            bool itemFound = false;
            foreach (var item in itemsInCell)
            {
                if (item != data.Entity)
                {
                    var pickUpData = new PickUpActionEventData(data.Entity, item, data.Position);
                    _eventManager.Publish(pickUpData);
                    itemFound = true;
                    RemoveEntity(item, data.Position);
                    break;
                }
            }

            if (!itemFound)
            {
                _eventManager.Publish(new MessageEventData(this, $"Nothing here to pick up"));
            }
        }

        private void OnLookRequest(LookRequestEventData data)
        {
            // Retrieve entities in adjacent cells.
            var entitiesInCell = GetEntitiesInCell(data.Position);

            if (entitiesInCell.Count <= 1)
            {
                // No entities found in adjacent cells. You might want to notify the player.
                _eventManager.Publish(new MessageEventData(this, "You do not see anything here"));
            }

            else
            {
                // Entities found. We create a LookActionEventData with the list of entities.
                var lookInCellRequestData = new LookActionEventData(data.Entity, entitiesInCell, data.Position);
                // Publishing the event with the relevant data.
                _eventManager.Publish(lookInCellRequestData);
            }
        }

        private void OnSearchRequest(SearchRequestEventData data)
        {
            var position = GetCellForPosition(data.Position);
            TileSearch.Invoke(data.Entity, position.ToVector2());

            // TODO: OnSearchRequest - This will be used to uncover Hidden things, and will be matched against perception. the player may get a hint based on their intuition and wether they know of the thing being hidden 

        }

        private void OnUseRequest(UseRequestEventData data)
        {
            _eventManager.Publish(new MessageEventData(this, $"You atempt to use something"));
        }

        public List<Entity> GetEntitiesInAdjacentCells(Vector2 position)
        {
            var entities = new List<Entity>();
            var cell = GetCellForPosition(position);

            for (int x = cell.X - 1; x <= cell.X + 1; x++)
                for (int y = cell.Y - 1; y <= cell.Y + 1; y++)
                    if (x != cell.X || y != cell.Y) // Exclude the center cell
                        entities.AddRange(_grid.GetValueOrDefault(new Point(x, y)) ?? Enumerable.Empty<Entity>());
            return entities;
        }


        public List<Entity> GetEntitiesInCell(Vector2 position)
        {
            return _grid.GetValueOrDefault(GetCellForPosition(position)) ?? new List<Entity>();
        }

        private void OnEntityRemoved(RemoveEntityEventData eventData)
        {
            RemoveEntity(eventData.Entity, eventData.Position);
        }
        public void MoveEntity(Entity entity, Vector2 oldPosition, Vector2 newPosition)
        {
            var oldCell = GetCellForPosition(oldPosition);
            var newCell = GetCellForPosition(newPosition);

            // If the entity has indeed moved to a different cell, then remove it from the old cell and add it to the new cell.
            if (oldCell != newCell)
            {
                GetEntitiesInCell(newPosition);

                if (_grid.TryGetValue(oldCell, out var entities))
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
            if (!_grid.TryGetValue(cell, out var entities))
            {
                entities = new List<Entity>();
                _grid[cell] = entities;
            }
            entities.Add(entity);
        }
        public void RemoveEntity(Entity entity, Vector2 position)
        {
            var cell = GetCellForPosition(position);
            if (_grid.TryGetValue(cell, out var entities))
            {
                entities.Remove(entity);
                //_eventManager.Publish(new RemoveComponentsEventData(entity));
                EntityRemoved?.Invoke(entity, position);
            }
        }
        private Point GetCellForPosition(Vector2 position) => new Point((int)(position.X / _cellSize), (int)(position.Y / _cellSize));

        // Public methods to interact with the grid
        public List<Entity> GetEntitiesInRegion(Rectangle region)
        {
            var entities = new List<Entity>();
            for (int x = region.Left / _cellSize; x <= region.Right / _cellSize; x++)
                for (int y = region.Top / _cellSize; y <= region.Bottom / _cellSize; y++)
                    entities.AddRange(_grid.GetValueOrDefault(new Point(x, y)) ?? Enumerable.Empty<Entity>());


            return entities;
        }
        public int CountEntities()
        {
            int count = 0;
            foreach (var cell in _grid.Values)
            {
                count += cell.Count;
            }
            return count;
        }
        public void CollisionActivate(Entity entityA, Entity entityB, Vector2 possition)
        {
            _eventManager.Publish(new CollisionEventData(entityA, entityB, possition));
        }
    }
}
