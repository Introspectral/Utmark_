using Utmark_ECS.Intefaces;

namespace Utmark_ECS.Map.MapGenerators
{
    public class RandomMapGenerator : IMapGenerator
    {
        public Tile[,] Generate(int width, int height, Tile[] availableTiles)
        {
            var random = new Random();
            var tiles = new Tile[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int index = random.Next(availableTiles.Length);
                    tiles[x, y] = availableTiles[index];
                }
            }

            return tiles;
        }


    }
}
