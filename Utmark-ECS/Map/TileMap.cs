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
        // Properties
        public int Width { get; }
        public int Height { get; }

        // Fields
        private readonly IMapGenerator _mapGenerator;
        private readonly Tile[,] _tiles;
        private readonly SpatialGrid _spatialGrid; // It's good practice to keep a reference if needed later
        private readonly EventManager _eventManager;
        public event Action<Entity, Vector2>? FoundOnTile;

        // Constructor and event subscription
        public TileMap(int width, int height, SpatialGrid spatialGrid, IMapGenerator mapGenerator, Tile[] availableTiles, EventManager eventManager)
        {
            Width = width;
            Height = height;
            _spatialGrid = spatialGrid;
            _mapGenerator = mapGenerator;
            _eventManager = eventManager;


            // Use the provided generator to create the tile layout.
            _tiles = _mapGenerator.Generate(width, height, availableTiles);

            _spatialGrid.EntityMoved += UpdateEntityTile;
            _spatialGrid.EntityRemoved += UpdateEntityTile;
            _spatialGrid.TileSearch += SearchTile;

        }
        public void UpdateEntityTile(Entity entity, Vector2 newPosition)
        {
            // Update the tile based on the new position of the entity
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
            // Check if tilePosition is within the bounds of the TileMap
            int x = (int)tilePosition.X;
            int y = (int)tilePosition.Y;

            if (IsValidTileCoordinate(x, y))
            {
                Tile targetTile = _tiles[x, y];

                // Optionally, check if the tile is already occupied
                if (targetTile.IsOccupied)
                {
                    Console.WriteLine($"Warning: Tile at position ({x}, {y}) is already occupied by entity {targetTile.OccupyingEntity}.");
                    return;
                }

                targetTile.IsOccupied = true; // Mark the tile as occupied
                targetTile.OccupyingEntity = entity; // Set the entity to the tile
            }
            else
            {
                // Log an error message if the position is out of bounds
                Console.WriteLine($"Error: Attempted to set entity to invalid tile position ({x}, {y}).");
            }
        }

        public void RemoveEntity(Entity entity, Vector2 tilePosition)
        {
            // Check if tilePosition is within the bounds of the TileMap
            int x = (int)tilePosition.X;
            int y = (int)tilePosition.Y;

            if (IsValidTileCoordinate(x, y))
            {
                Tile targetTile = _tiles[x, y];

                // Check if the tile is occupied by the specified entity
                if (targetTile.IsOccupied && ReferenceEquals(targetTile.OccupyingEntity, entity))
                {
                    targetTile.IsOccupied = false; // Mark the tile as unoccupied
                    targetTile.OccupyingEntity = null; // Clear the entity from the tile
                }
                else
                {
                    Console.WriteLine($"Warning: Tile at position ({x}, {y}) is either not occupied or is occupied by a different entity.");
                }
            }
            else
            {
                // Log an error message if the position is out of bounds
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
                    var tileRect = new Rectangle(x * 16, y * 16, 16, 16); // Consider making 16 a constant or configurable property

                    spriteBatch.Draw(
                        spriteSheet,
                        tileRect.Location.ToVector2(),
                        sprites[tile.SpriteName],
                        tile.color // Ensure this property matches the naming convention in your Tile class
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
