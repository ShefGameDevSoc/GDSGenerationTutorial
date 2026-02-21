using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    //WARNING: Vibe coded nonsense
    //Thanks to chatGPT for this, I CBA to implement WFC from scratch AGAIN
    //Works well enough for the demo
    
    public static class BoundaryWFCSpawn
    {
        private const int SIZE = 64;
        

        private struct BoundaryTile
        {
            public bool N;
            public bool E;
            public bool S;
            public bool W;

            public BoundaryTile(bool n, bool e, bool s, bool w)
            {
                N = n;
                E = e;
                S = s;
                W = w;
            }
        }

        // TRUE = outside on that side
        private static readonly BoundaryTile[] Tiles =
        {
            new(false,false,false,false), // 0 fully inside

            new(true,false,false,false),  // 1 N
            new(false,true,false,false),  // 2 E
            new(false,false,true,false),  // 3 S
            new(false,false,false,true),  // 4 W

            new(true,true,false,false),   // 5 NE
            new(false,true,true,false),   // 6 ES
            new(false,false,true,true),   // 7 SW
            new(true,false,false,true),   // 8 WN

            new(true,true,true,true),     // 9 isolated
        };

        // Bias toward interior mass
        private static float[] Weights =
        {
            8f, // inside
            1f,1f,1f,1f,
            1f,1f,1f,1f,
            0.2f
        };
        
        static List<int>[,] wave = new List<int>[SIZE, SIZE];
        

        public static Occupancy[,] Setup(float insideWeight)
        {
            float[] LocalWeights =
            {
                8f * insideWeight, // inside
                1f * insideWeight,1f * insideWeight,1f * insideWeight,1f * insideWeight,
                1f * insideWeight,1f * insideWeight,1f * insideWeight,1f * insideWeight,
                0.2f
            };
        
            Weights = LocalWeights;
            
            

            // Initialize superposition
            for (int x = 0; x < SIZE; x++)
            for (int y = 0; y < SIZE; y++)
            {
                wave[x, y] = new List<int>();

                for (int i = 0; i < Tiles.Length; i++)
                    wave[x, y].Add(i);

                ApplyBorderConstraints(wave[x, y], x, y);
            }
            
            Worker();

            return ConvertToOccupancy(wave);
        }

        static bool Worker()
        {
            Vector2Int cell = FindLowestEntropy(wave);
            if (cell.x < 0) return false;

            Collapse(wave, cell);
            Propagate(wave, cell);

            return true;
        }

        public static Occupancy[,] RunOnce()
        {
            Worker();
            return ConvertToOccupancy(wave);
        }

        public static Occupancy[,] RunToEnd()
        {
            while (true)
            {
                if (!Worker()) break;
            }

            return ConvertToOccupancy(wave);
        }
        
        

        private static Vector2Int FindLowestEntropy(List<int>[,] wave)
        {
            int best = int.MaxValue;
            Vector2Int pos = new(-1, -1);

            for (int x = 0; x < SIZE; x++)
            for (int y = 0; y < SIZE; y++)
            {
                int c = wave[x, y].Count;
                if (c > 1 && c < best)
                {
                    best = c;
                    pos = new Vector2Int(x, y);
                }
            }

            return pos;
        }

        private static void Collapse(List<int>[,] wave, Vector2Int p)
        {
            var options = wave[p.x, p.y];

            float total = 0f;
            foreach (var o in options)
                total += Weights[o];

            float r = Random.value * total;

            foreach (var o in options)
            {
                r -= Weights[o];
                if (r <= 0f)
                {
                    wave[p.x, p.y] = new List<int> { o };
                    return;
                }
            }

            wave[p.x, p.y] = new List<int> { options[0] };
        }

        private static void Propagate(List<int>[,] wave, Vector2Int start)
        {
            Queue<Vector2Int> queue = new();
            queue.Enqueue(start);

            Vector2Int[] dirs =
            {
                Vector2Int.up,
                Vector2Int.down,
                Vector2Int.left,
                Vector2Int.right
            };

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                var source = wave[current.x, current.y];

                foreach (var dir in dirs)
                {
                    Vector2Int n = current + dir;
                    if (!Inside(n)) continue;

                    bool changed = false;
                    var neighbour = wave[n.x, n.y];

                    for (int i = neighbour.Count - 1; i >= 0; i--)
                    {
                        var candidate = Tiles[neighbour[i]];
                        bool valid = false;

                        foreach (var s in source)
                        {
                            if (Compatible(Tiles[s], candidate, dir))
                            {
                                valid = true;
                                break;
                            }
                        }

                        if (!valid)
                        {
                            neighbour.RemoveAt(i);
                            changed = true;
                        }
                    }

                    if (changed)
                        queue.Enqueue(n);
                }
            }
        }

        private static bool Compatible(BoundaryTile a, BoundaryTile b, Vector2Int dir)
        {
            if (dir == Vector2Int.up) return a.N == b.S;
            if (dir == Vector2Int.down) return a.S == b.N;
            if (dir == Vector2Int.right) return a.E == b.W;
            if (dir == Vector2Int.left) return a.W == b.E;

            return false;
        }
        

        private static Occupancy[,] ConvertToOccupancy(List<int>[,] wave)
        {
            Occupancy[,] result = new Occupancy[SIZE, SIZE];

            for (int x = 0; x < SIZE; x++)
            for (int y = 0; y < SIZE; y++)
            {
                var tile = Tiles[wave[x, y][0]];

                result[x, y] = new Occupancy
                {
                    // outside only if completely exposed
                    isOutside = tile.N && tile.E && tile.S && tile.W,

                    north = tile.N,
                    east  = tile.E,
                    south = tile.S,
                    west  = tile.W
                };
            }

            return result;
        }
        

        private static void ApplyBorderConstraints(List<int> options, int x, int y)
        {
            for (int i = options.Count - 1; i >= 0; i--)
            {
                var t = Tiles[options[i]];

                if (x == 0 && !t.W) options.RemoveAt(i);
                else if (x == SIZE - 1 && !t.E) options.RemoveAt(i);
                else if (y == 0 && !t.S) options.RemoveAt(i);
                else if (y == SIZE - 1 && !t.N) options.RemoveAt(i);
            }
        }

        private static bool Inside(Vector2Int p)
        {
            return p.x >= 0 && p.y >= 0 &&
                   p.x < SIZE && p.y < SIZE;
        }
        
    }
}