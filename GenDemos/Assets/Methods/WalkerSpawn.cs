using UnityEngine;

namespace DefaultNamespace
{
    public class WalkerSpawn
    {
        private static int x;
        private static int y;
        static int[,] tiles = new int[64, 64];
        
        public static Occupancy[,] Setup()
        {
            tiles = new int[64, 64];
            x = UnityEngine.Random.Range(0, 64);
            y = UnityEngine.Random.Range(0, 64);
            
            CreateRoom(UnityEngine.Random.Range(2, 9));

            return OccupancyToTilemap.ConvertToTiles(tiles);
        }
        
        public static Occupancy[,] Walk()
        {
            bool xy = UnityEngine.Random.Range(0, 2) == 0;
            int dir = UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1;
            int amt = UnityEngine.Random.Range(5, 15);

            if (xy && x + amt * dir > 64 || x + amt * dir < 0)
            {
                dir *= -1;
            }
            
            if (!xy && y + amt * dir > 64 || y + amt * dir < 0)
            {
                dir *= -1;
            }
            
            Walk(amt, xy, dir );
            CreateRoom(UnityEngine.Random.Range(2, 4));
            return OccupancyToTilemap.ConvertToTiles(tiles);
        }

        static void CreateRoom(int size)
        {
            for (int i = x - size; i <= x + size; i++)
            {
                for (int j = y - size; j <= y + size; j++)
                {
                    TrySetOccupancy(i,j);
                }
            }
        }
        static void Walk(int size, bool xy, int dir)
        {
            for (int i = 0; i < size; i++)
            {
                if (xy)
                {
                    x += 1 * dir;
                }
                else
                {
                    y += 1 * dir;
                }
                
                for (int j = 0; j < 3; j++)
                {
                    if (xy)
                    {
                        TrySetOccupancy(x, y+j);    
                    }
                    else
                    {
                        TrySetOccupancy(x+j,y);
                    }
                    
                }
            }
        }

        static void TrySetOccupancy(int xx, int yy)
        {
            Debug.Log($"Trying to set occupancy at ({xx}, {yy})");
            if (xx < 0 || xx >= 64 || yy < 0 || yy >= 64) return;

            tiles[xx, yy] = 1;
        }
        
    }
}