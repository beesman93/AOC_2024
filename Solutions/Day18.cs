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
        const int BYTES_FALL_P1 = 1024;
        (int x, int y) start = (0, 0);
        (int x, int y) end = (SIZE - 1, SIZE - 1);
        List<(int x, int y)> obsticles;
        bool[,] grid;
        public Day18()
        {
            obsticles = [];
            grid = new bool[SIZE, SIZE];
            for (int i = 0; i < _input.Length; i++)
            {
                int x = Convert.ToInt32(_input[i].Split(',')[0]);
                int y = Convert.ToInt32(_input[i].Split(',')[1]);
                obsticles.Add((x, y));
            }

        }
        (int x, int y)[]dirs = new(int x, int y)[]
        {
            (1, 0),
            (0, 1),
            (-1, 0),
            (0, -1)
        };

        public int solve(int bitCount)
        {
            grid= new bool[SIZE, SIZE];
            for(int i=0;i<bitCount;i++)
                grid[obsticles[i].x, obsticles[i].y] = true;
            PriorityQueue<(int x, int y), int> queue = new();
            queue.Enqueue((start.x, start.y), 0);
            Dictionary<(int x, int y), int> distances = new();
            while (queue.TryDequeue(out var node, out int distance))
            {
                if (distances.ContainsKey(node))
                    continue;
                if (node.x == end.x && node.y == end.y)
                {
                    return distance;
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
            return -1;
        }

        public override ValueTask<string> Solve_1()
        {
            return new($"{solve(BYTES_FALL_P1)}");
        }

        public override ValueTask<string> Solve_2()
        {
            int WORKING_OBS_CNT = BYTES_FALL_P1;
            int NOT_WORKING_OBS_CNT = _input.Length;
            while(WORKING_OBS_CNT + 1 < NOT_WORKING_OBS_CNT)
            {
                int mid = (WORKING_OBS_CNT + NOT_WORKING_OBS_CNT) / 2;
                if (solve(mid) == -1)
                {
                    NOT_WORKING_OBS_CNT = mid;
                }
                else
                {
                    WORKING_OBS_CNT = mid;
                }
            }
            return new($"{obsticles[NOT_WORKING_OBS_CNT-1].x},{obsticles[NOT_WORKING_OBS_CNT - 1].y}");
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
