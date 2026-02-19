using UnityEngine;

namespace DefaultNamespace
{
    public class OccupancyToTilemap
    {
        public static Occupancy[,] ConvertToTiles(int[,] building)
        {
            int width = building.GetLength(0);
            int height = building.GetLength(1);

            Occupancy[,] tiles = new Occupancy[width, height];

            for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                // Outside stays outside
                if (building[x, y] == 0)
                {
                    tiles[x, y] = new Occupancy(){isOutside = true};
                    continue;
                }

                bool north = !IsInside(building, x, y + 1);
                bool east  = !IsInside(building, x + 1, y);
                bool south = !IsInside(building, x, y - 1);
                bool west  = !IsInside(building, x - 1, y);

                tiles[x,y] = new Occupancy(){north = north, east = east,south = south,west = west};
            }

            return tiles;
        }
    
        static bool IsInside(int[,] grid, int x, int y)
        {
            int w = grid.GetLength(0);
            int h = grid.GetLength(1);

            if (x < 0 || y < 0 || x >= w || y >= h)
                return false;

            return grid[x, y] == 1;
        }
    }
}