using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCHelper;

namespace AOC_2024
{
    internal class Day10 : BaseDayWithInput
    {
        public Day10()
        {

        }

        public override ValueTask<string> Solve_1()
        {
            long ans = 0;
            for (int i = 0; i < _input.Length; i++)
                for (int j = 0; j < _input[i].Length; j++)
                    if (_input[i][j] == '9')//count reachable 0s, add them to score
                        ans += hikeDown(i, j);
            return new($"{ans}");
        }

        private int hikeDown(int x, int y)
        {
            int ans = 0;
            Queue<(int, int)> q = new();
            q.Enqueue((x, y));
            HashSet<(int, int)> visited = new();
            while (q.Count > 0)
            {
                (int, int) coordinates = q.Dequeue();
                if (visited.Contains(coordinates))
                    continue;
                visited.Add(coordinates);
                if (_input[coordinates.Item1][coordinates.Item2] == '0')//trailed down
                    ans++;
                foreach ((int, int) dirrection in new (int, int)[] { (1, 0), (-1, 0), (0, 1), (0, -1) })
                {
                    (int,int) nextCoordinates =
                        (coordinates.Item1 + dirrection.Item1, coordinates.Item2 + dirrection.Item2);
                    if (nextCoordinates.Item1 < 0 || nextCoordinates.Item2 < 0 ||
                        nextCoordinates.Item1 >= _input.Length || nextCoordinates.Item2 >= _input[0].Length)
                        continue;
                    if (_input[coordinates.Item1][coordinates.Item2]-_input[nextCoordinates.Item1][nextCoordinates.Item2]==1)
                        q.Enqueue(nextCoordinates);//keep trailing down
                }
            }
            return ans;
        }

        public override ValueTask<string> Solve_2()
        {
            long ans = 0;
            int[,,] heatmap = new int[10, _input.Length, _input[0].Length];//num, x, y
            for (int i = 0; i < _input.Length; i++)
                for (int j = 0; j < _input[i].Length; j++)
                    if (_input[i][j] == 9 + '0')
                        heatmap[9, i, j] = 1;
            for (int num = 8; num >= 0; num--)
                for (int i = 0; i < _input.Length; i++)
                    for (int j = 0; j < _input[i].Length; j++)
                        if (_input[i][j] == num + '0')
                        {
                            heatmap[num, i, j] =
                                getIfAble(heatmap, num + 1, i + 1, j) +
                                getIfAble(heatmap, num + 1, i - 1, j) +
                                getIfAble(heatmap, num + 1, i, j + 1) +
                                getIfAble(heatmap, num + 1, i, j - 1);
                            if(num == 0)
                                ans += heatmap[0, i, j];
                        }
            return new($"{ans}");
        }
        private static int getIfAble(int[,,] heat, int num, int x, int y)
        {
            if (x < 0 || y < 0 || x >= heat.GetLength(1) || y >= heat.GetLength(2))
                return 0;
            return heat[num, x, y];
        }
    }
}
