using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class PerlinSpawn
    {
        public static Occupancy[,] Run(float offset)
        {
            float xoffset = UnityEngine.Random.Range(0f, 50f);
            float yoffset = UnityEngine.Random.Range(0f, 50f);
            int[,] tiles = new int[64, 64];
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    tiles[i,j] = Mathf.RoundToInt(Mathf.PerlinNoise(xoffset + (float)i/10, yoffset +(float)j/10) + offset);
                }    
            }

            return OccupancyToTilemap.ConvertToTiles(tiles);
        }
    }
}