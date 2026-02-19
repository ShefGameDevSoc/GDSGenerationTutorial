using System;

namespace DefaultNamespace
{
    public class RandomSpawn
    {
        public static Occupancy[,] Run(float prob)
        {
            int[,] tiles = new int[64, 64];
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    tiles[i,j] = UnityEngine.Random.Range(0f, 1f)>prob ? 0 : 1;
                }    
            }

            return OccupancyToTilemap.ConvertToTiles(tiles);
        }
    }
}