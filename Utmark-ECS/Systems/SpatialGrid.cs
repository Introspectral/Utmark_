using Microsoft.Xna.Framework;
using Utmark_ECS.Components;
using Utmark_ECS.Entities;
using Utmark_ECS.Managers;
using Utmark_ECS.Systems.EventHandlers;
using Utmark_ECS.Systems.EventSystem;
using Utmark_ECS.Systems.EventSystem.EventType;
using static Utmark_ECS.Enums.InputActionEnum;

namespace Utmark_ECS.Systems
{
    // Entities are added to the spatialGrid in the CreateEntity method for now.

    public class SpatialGrid
    {
        private readonly int cellSize;
        private ComponentManager _componentManager;
        private readonly Dictionary<Point, List<Entity>> grid = new();
        private TileMap _tileMap;
        private EventManager _eventManager;

        RandomMessagePicker messagePicker = new RandomMessagePicker();
        public event Action<Entity, Vector2>? EntityMoved;
        public event Action<Entity, Vector2>? CollisionCheck;
        public event Action<Entity, Vector2>? EntityRemoved;

        public SpatialGrid(int cellSize, EventManager eventManager)
        {
            this.cellSize = cellSize;
            _eventManager = eventManager;

            _eventManager.Subscribe<EntityMovedData>(OnMove);
            _eventManager.Subscribe<ActionRequestEvent>(OnActionReceived);
            _eventManager.Subscribe<EntityRemoveData>(OnEntityRemoved);

        }
        private void OnEntityRemoved(EntityRemoveData removeData)
        {
            RemoveEntity(removeData.Entity, removeData.Position);
        }
        public void SetComponentManager(ComponentManager componentManager)
        {
            _componentManager = componentManager;
        }
        public void SetTileMap(TileMap tileMap) => _tileMap = tileMap;



        private void OnActionReceived(ActionRequestEvent actionData)
        {
            var currentPossition = GetPlayerCell();
            var playerEntity = GetPlayerEntity();
            var otherEntity = GetEntitiesInCell(currentPossition);
            if (actionData != null)
            {

                switch (actionData.State)
                {
                    case InputAction.Use:
                        _eventManager.Publish(new MessageEvent(this, $"SpatialGrid - Handled Use Action"));
                        //_eventManager.Publish(new ActionRequestEvent(actionData));
                        // Handle use action here
                        break;
                    case InputAction.PickUp:

                        foreach (var entity in otherEntity)
                        {
                            if (IsItem(entity))
                            {
                                _eventManager.Publish(new PickUpActionEvent(playerEntity, entity, currentPossition));
                                break;
                            }
                            else if (entity != playerEntity)
                            {
                                _eventManager.Publish(new MessageEvent(this, $"[color=red]You cannot do that[/color]"));
                                break;
                            }
                            else
                            {
                                _eventManager.Publish(new MessageEvent(this, messagePicker.GetRandomMessage()));
                            }
                        }
                        break;
                    case InputAction.Throw:
                        _eventManager.Publish(new MessageEvent(this, $"SpatialGrid - Handled Throw Action"));
                        //_eventManager.Publish(new ActionRequestEvent(actionData));
                        // Handle throw action here
                        break;
                }
            }
            // actions based on input. PickUp, Attack etc
        }
        private bool IsItem(Entity entity) =>
        _componentManager.GetComponentsForEntity(entity).Any(component => component is ItemComponent);
        private Vector2 GetPlayerCell()
        {
            var playerEntity = GetPlayerEntity();
            var playerPossition = _componentManager.GetComponent<PositionComponent>(playerEntity);
            //var currentCell = GetCellForPosition(playerPossition.Position);
            return playerPossition.Position;
        }
        private Entity GetPlayerEntity() =>
            _componentManager.GetEntitiesWithComponents(typeof(InputComponent)).FirstOrDefault();
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
