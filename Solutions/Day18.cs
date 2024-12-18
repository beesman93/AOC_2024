using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCHelper;
using MathNet.Numerics;

namespace AOC_2024
{
    internal class Day18 : BaseDayWithInput
    {
        const int SIZE = 71;
        const int BYTES_FALL = 1024;
        (int x, int y) start = (0, 0);
        (int x, int y) end = (SIZE - 1, SIZE - 1);
        bool[,] grid;
        public Day18()
        {
            grid = new bool[SIZE,SIZE];
            for (int i = 0; i < BYTES_FALL; i++)
            {
                int x = Convert.ToInt32(_input[i].Split(',')[0]);
                int y = Convert.ToInt32(_input[i].Split(',')[1]);
                grid[x, y] = true;
            }

        }
        (int x, int y)[]dirs = new(int x, int y)[]
        {
            (1, 0),
            (0, 1),
            (-1, 0),
            (0, -1)
        };

        public override ValueTask<string> Solve_1()
        {
            long ans = 0;
            int shortestDistance = int.MaxValue;
            PriorityQueue<(int x, int y), int> queue = new();
            queue.Enqueue((start.x, start.y), 0);
            Dictionary<(int x, int y),int> distances = new();
            while (queue.TryDequeue(out var node, out int distance))
            {
                if (distances.ContainsKey(node))
                    continue;
                if (node.x == end.x && node.y == end.y)
                {
                    shortestDistance = distance;
                    break;
                }
                distances[node] = distance;
                foreach(var dir in dirs)
                {
                    int x = node.x + dir.x;
                    int y = node.y + dir.y;
                    if (x < 0 || x >= SIZE || y < 0 || y >= SIZE || grid[x, y])
                        continue;
                    if (distances.ContainsKey((x, y)))
                        continue;
                    queue.Enqueue((x, y), distance + 1);
                }
            }
            return new($"{shortestDistance}");
        }

        public override ValueTask<string> Solve_2()
        {
            for (int i = BYTES_FALL; i < _input.Length; i++)
            {
                int xx = Convert.ToInt32(_input[i].Split(',')[0]);
                int yy = Convert.ToInt32(_input[i].Split(',')[1]);
                grid[xx, yy] = true;

                PriorityQueue<(int x, int y), int> queue = new();
                queue.Enqueue((start.x, start.y), 0);
                Dictionary<(int x, int y), int> distances = new();
                bool solved = false;
                while (queue.TryDequeue(out var node, out int distance))
                {
                    if (distances.ContainsKey(node))
                        continue;
                    if (node.x == end.x && node.y == end.y)
                    {
                        solved = true;
                        break;
                    }
                    distances[node] = distance;
                    foreach (var dir in dirs)
                    {
                        int x = node.x + dir.x;
                        int y = node.y + dir.y;
                        if (x < 0 || x >= SIZE || y < 0 || y >= SIZE || grid[x, y])
                            continue;
                        if (distances.ContainsKey((x, y)))
                            continue;
                        queue.Enqueue((x, y), distance + 1);
                    }
                }
                if (!solved)
                    return new($"{xx},{yy}");
            }
            return new("no solution");
        }

        public void printGrid()
        {
            for (int y = 0; y < SIZE; y++)
            {
                for (int x = 0; x < SIZE; x++)
                {
                    Console.Write(grid[x, y] ? '#' : '.');
                }
                Console.WriteLine();
            }
        }
    }
}
