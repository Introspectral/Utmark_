using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Utmark_ECS.Entities;
using static Utmark_ECS.Enums.TileTypeEnum;

namespace Utmark_ECS.Map
{
    public class Tile
    {

        public Color color { get; set; }
        public TileType Type { get; set; }
        public Texture2D? Texture { get; set; }
        public Entity? OccupyingEntity { get; set; }
        public bool IsOccupied { get; set; }
        public int Size { get; set; }
        public List<Entity> EntitiesOnTile { get; set; }

        public string SpriteName { get; set; } // to identify which sprite to use

        public Tile(TileType type, string spriteName, Color color, Entity occupyingEntity)
        {
            Type = type;
            SpriteName = spriteName;
            this.color = color;

            OccupyingEntity = occupyingEntity;
            IsOccupied = occupyingEntity != null;
            EntitiesOnTile = new List<Entity>();
        }
    }
}
