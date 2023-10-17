using Microsoft.Xna.Framework;
using Utmark_ECS.Components;
using Utmark_ECS.Entities;
using Utmark_ECS.Managers;
using Utmark_ECS.Systems.EventSystem;
using Utmark_ECS.Systems.EventSystem.EventType;

namespace Utmark_ECS.Systems
{
    public class SpatialGrid
    {
        private ComponentManager? _componentManager;
        private TileMap? _tileMap;
        private readonly int _cellSize;
        private readonly Dictionary<Point, List<Entity>> _grid;
        private readonly EventManager? _eventManager;
        private readonly RandomMessagePicker? _messagePicker;

        public event Action<Entity, Vector2>? EntityMoved;
        public event Action<Entity, Vector2>? CollisionCheck;
        public event Action<Entity, Vector2>? EntityRemoved;

        public SpatialGrid(int cellSize, EventManager eventManager)
        {
            _cellSize = cellSize;
            _eventManager = eventManager;
            _grid = new Dictionary<Point, List<Entity>>();
            _messagePicker = new RandomMessagePicker();
            SubscribeToEvents();
        }

        public void SetComponentManager(ComponentManager componentManager) => _componentManager = componentManager;
        public void SetTileMap(TileMap tileMap) => _tileMap = tileMap;

        private void SubscribeToEvents()
        {
            //_eventManager.Subscribe<MoveEntityEventData>(OnMove);
            //_eventManager.Subscribe<ActionEventData>(OnActionReceived);
            _eventManager.Subscribe<RemoveEntityEventData>(OnEntityRemoved);

        }
        //private void OnMove(MoveEntityEventData moveData)
        //{
        //    MoveEntity(moveData.Entity, moveData.OldPosition, moveData.NewPosition);
        //    _tileMap.UpdateEntityTile(moveData.Entity, moveData.NewPosition);

        //}


        private void OnEntityRemoved(RemoveEntityEventData eventData)
        {
            RemoveEntity(eventData.Entity, eventData.Position);
        }
        //private void OnActionReceived(ActionEventData actionData)
        //{
        //    var currentPossition = GetPlayerCell();
        //    var playerEntity = GetPlayerEntity();
        //    var otherEntity = GetEntitiesInCell(currentPossition);
        //    if (actionData != null)
        //    {

        //        switch (actionData.State)
        //        {
        //            case InputAction.Use:
        //                _eventManager.Publish(new MessageEvent(this, $"SpatialGrid - Handled Use Action"));
        //                //_eventManager.Publish(new ActionRequestEvent(actionData));
        //                // Handle use action here
        //                break;
        //            case InputAction.PickUp:

        //                foreach (var entity in otherEntity)
        //                {
        //                    if (IsItem(entity))
        //                    {
        //                        _eventManager.Publish(new PickUpActionEventData(playerEntity, entity, currentPossition));
        //                        break;
        //                    }
        //                    else if (entity != playerEntity)
        //                    {
        //                        _eventManager.Publish(new MessageEvent(this, $"[color=red]You cannot do that[/color]"));
        //                        break;
        //                    }
        //                    else
        //                    {
        //                        _eventManager.Publish(new MessageEvent(this, _messagePicker.GetRandomMessage()));
        //                    }
        //                }
        //                break;
        //            case InputAction.Throw:
        //                _eventManager.Publish(new MessageEvent(this, $"SpatialGrid - Handled Throw Action"));
        //                //_eventManager.Publish(new ActionRequestEvent(actionData));
        //                // Handle throw action here
        //                break;
        //        }
        //    }
        //    // actions based on input. PickUp, Attack etc
        //}

        // Core functionality methods
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
                EntityRemoved?.Invoke(entity, position);
            }
        }
        private Entity GetPlayerEntity()
        {
            return _componentManager.GetEntitiesWithComponents(typeof(InputComponent)).FirstOrDefault();
        }
        //private Vector2 GetPlayerCell()
        //{
        //    var playerEntity = GetPlayerEntity();
        //    var playerPossition = _componentManager.GetComponent<PositionComponent>(playerEntity);
        //    return playerPossition.Position;
        //}
        //private bool IsItem(Entity entity) => _componentManager.GetComponentsForEntity(entity).Any(component => component is ItemComponent);
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
        public List<Entity> GetEntitiesInCell(Vector2 position)
        {
            return _grid.GetValueOrDefault(GetCellForPosition(position)) ?? new List<Entity>();
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
