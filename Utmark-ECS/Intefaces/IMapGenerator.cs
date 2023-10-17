using Utmark_ECS.Map;

namespace Utmark_ECS.Intefaces
{
    public interface IMapGenerator
    {
        Tile[,] Generate(int width, int height, Tile[] availableTiles);
    }
}
