using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Utmark_ECS.Entities;
using Utmark_ECS.Map;

namespace Utmark_ECS.Systems
{

    public class TileMap
    {
        public int Width { get; }
        public int Height { get; }
        private Tile[,] _tiles;
        public TileMap(int width, int height, SpatialGrid spatialGrid, params Tile[] tiles)
        {
            Width = width;
            Height = height;
            _tiles = new Tile[width, height];

            Random random = new Random();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int index = random.Next(tiles.Length);
                    _tiles[x, y] = tiles[index];
                }
            }

            spatialGrid.EntityMoved += UpdateEntityTile;
            spatialGrid.EntityRemoved += UpdateEntityTile;
        }
        public void UpdateEntityTile(Entity entity, Vector2 newPosition)
        {
            // Update the tile based on the new position of the entity
            SetEntityToTile(entity, newPosition);
        }


        public Tile GetTile(int x, int y)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                return _tiles[x, y];
            }
            else
            {
                throw new ArgumentOutOfRangeException("Coordinates out of bounds");
            }
        }

        public void SetTile(int x, int y, Tile tile)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                _tiles[x, y] = tile;
            }
            else
            {
                throw new ArgumentOutOfRangeException("Coordinates out of bounds");
            }
        }
        public void SetEntityToTile(Entity entity, Vector2 tilePosition)
        {
            // Check if tilePosition is within the bounds of the TileMap
            int x = (int)tilePosition.X;
            int y = (int)tilePosition.Y;

            if (x >= 0 && x < Width && y >= 0 && y < Height)
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

            if (x >= 0 && x < Width && y >= 0 && y < Height)
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
                    var tileRect = new Rectangle(x * 16, y * 16, 16, 16);

                    spriteBatch.Draw
                    (
                        spriteSheet, // Assume all tiles are in a single texture atlas
                        tileRect.Location.ToVector2(),
                        sprites[tile.SpriteName], // Get Source rectangle from the sprite dictionary
                        tile.color // Use the color property of the Tile class
                    );
                }
            }
        }
    }
}
