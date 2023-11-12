using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Utmark_ECS.Entities;
using Utmark_ECS.Intefaces;
using Utmark_ECS.Managers;
using Utmark_ECS.Map;
using Utmark_ECS.Systems.EventSystem.EventType;

namespace Utmark_ECS.Systems
{

    public class TileMap
    {
        public int Width { get; }
        public int Height { get; }

        private readonly IMapGenerator _mapGenerator;
        private readonly Tile[,] _tiles;
        private readonly SpatialGrid _spatialGrid;
        private readonly EventManager _eventManager;
        public event Action<Entity, Vector2>? FoundOnTile;

        public TileMap(int width, int height, SpatialGrid spatialGrid, IMapGenerator mapGenerator, Tile[] availableTiles, EventManager eventManager)
        {
            Width = width;
            Height = height;
            _spatialGrid = spatialGrid;
            _mapGenerator = mapGenerator;
            _eventManager = eventManager;

            _tiles = _mapGenerator.Generate(width, height, availableTiles);

            _spatialGrid.EntityMoved += UpdateEntityTile;
            _spatialGrid.EntityRemoved += UpdateEntityTile;
            _spatialGrid.TileSearch += SearchTile;

        }
        public void UpdateEntityTile(Entity entity, Vector2 newPosition)
        {
            SetEntityToTile(entity, newPosition);
        }


        private bool IsValidTileCoordinate(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }

        public Tile GetTile(int x, int y)
        {
            if (IsValidTileCoordinate(x, y))
            {
                return _tiles[x, y];
            }

            throw new ArgumentOutOfRangeException($"Coordinates ({x}, {y}) are out of bounds.");
        }

        public void SetTile(int x, int y, Tile tile)
        {
            if (IsValidTileCoordinate(x, y))
            {
                _tiles[x, y] = tile;
            }
            else
            {
                throw new ArgumentOutOfRangeException($"Coordinates ({x}, {y}) are out of bounds.");
            }
        }
        public void SetEntityToTile(Entity entity, Vector2 tilePosition)
        {
            int x = (int)tilePosition.X;
            int y = (int)tilePosition.Y;

            if (IsValidTileCoordinate(x, y))
            {
                Tile targetTile = _tiles[x, y];

                if (targetTile.IsOccupied)
                {
                    Console.WriteLine($"Warning: Tile at position ({x}, {y}) is already occupied by entity {targetTile.OccupyingEntity}.");
                    return;
                }

                targetTile.IsOccupied = true;
                targetTile.OccupyingEntity = entity;
            }
            else
            {
                Console.WriteLine($"Error: Attempted to set entity to invalid tile position ({x}, {y}).");
            }
        }

        public void RemoveEntity(Entity entity, Vector2 tilePosition)
        {
            int x = (int)tilePosition.X;
            int y = (int)tilePosition.Y;

            if (IsValidTileCoordinate(x, y))
            {
                Tile targetTile = _tiles[x, y];


                if (targetTile.IsOccupied && ReferenceEquals(targetTile.OccupyingEntity, entity))
                {
                    targetTile.IsOccupied = false;
                    targetTile.OccupyingEntity = null;
                }
                else
                {
                    Console.WriteLine($"Warning: Tile at position ({x}, {y}) is either not occupied or is occupied by a different entity.");
                }
            }
            else
            {
                Console.WriteLine($"Error: Attempted to remove entity from invalid tile position ({x}, {y}).");
            }
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D spriteSheet, Dictionary<string, Rectangle> sprites)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var tile = GetTile(x, y);
                    var tileRect = new Rectangle(x * 16, y * 16, 16, 16);

                    spriteBatch.Draw(
                        spriteSheet,
                        tileRect.Location.ToVector2(),
                        sprites[tile.SpriteName],
                        tile.color
                    );
                }
            }
        }
        private void SearchTile(Entity entity, Vector2 vector)
        {
            var _tilePosition = GetTile((int)vector.X, (int)vector.Y);
            var tileEntities = _tilePosition.EntitiesOnTile;
            foreach (var tileEntity in tileEntities)
            {
                _eventManager.Publish(new PopulateEntityEventData(tileEntity, vector));
            }
        }

    }

}
