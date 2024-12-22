using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AoCHelper;

namespace AOC_2024
{
    internal class Day20 : BaseDayWithInput
    {
        int XX;
        int YY;
        bool[,] walls;
        (int x, int y) start;
        (int x, int y) end;
        Dictionary<(int x, int y),int> distancesToEndWithoutCheating;
        public Day20()
        {
            XX = _input[0].Length;
            YY = _input.Length;
            walls = new bool[XX,YY];
            for (int y = 0; y < YY; y++)
            {
                for (int x = 0; x < XX; x++)
                {
                    switch(_input[y][x])
                    {
                        case '#':
                            walls[x, y] = true;
                            break;
                        case 'S':
                            start = (x, y);
                            break;
                        case 'E':
                            end = (x, y);
                            break;
                    }
                }
            }
            distancesToEndWithoutCheating = [];
            PriorityQueue<(int x, int y),int> q = new();
            q.Enqueue(end, 0);
            while (q.TryDequeue(out var node, out var dist))
            {
                if (distancesToEndWithoutCheating.ContainsKey(node))
                    continue;
                if(node.x < 0 || node.x >= XX || node.y < 0 || node.y >= YY)
                    continue;
                if (walls[node.x, node.y])
                    continue;
                distancesToEndWithoutCheating[node] = dist;
                q.Enqueue((node.x + 1, node.y), dist + 1);
                q.Enqueue((node.x - 1, node.y), dist + 1);
                q.Enqueue((node.x, node.y + 1), dist + 1);
                q.Enqueue((node.x, node.y - 1), dist + 1);
            }
        }

        private long successCheats(int maxCheat, int neededSave)
        {
            long ans = 0;
            int startToEndFullDistance = distancesToEndWithoutCheating[start];
            foreach (var startPos in distancesToEndWithoutCheating)
            {
                int travelBeforeCheat = startToEndFullDistance - distancesToEndWithoutCheating[startPos.Key];
                var cheats = cheatEndPositionsToTry(startPos.Key, maxCheat);
                foreach (var cheatEndPos in cheats)
                {
                    int cheatTraveled = Math.Abs(startPos.Key.x - cheatEndPos.x) + Math.Abs(startPos.Key.y - cheatEndPos.y);
                    int cheatEndDistToGo = distancesToEndWithoutCheating[cheatEndPos];
                    if(travelBeforeCheat + cheatTraveled + cheatEndDistToGo <= startToEndFullDistance-neededSave)
                    {
                        ans++;
                    }

                }
            }
            return ans;
        }
        private List<(int x, int y)> cheatEndPositionsToTry((int x, int y) cheatStartPos, int maxCheat)
        {
            List<(int x, int y)> endPos = [];
            for (int x = cheatStartPos.x - maxCheat; x <= cheatStartPos.x + maxCheat; x++)
            {
                for (int y = cheatStartPos.y - maxCheat; y <= cheatStartPos.y + maxCheat; y++)
                {
                    if (distancesToEndWithoutCheating.ContainsKey((x, y)))
                    {
                        int travel = Math.Abs(cheatStartPos.x - x) + Math.Abs(cheatStartPos.y - y);
                        if (travel<=maxCheat)
                        {
                            endPos.Add((x, y));
                        }
                    }
                }
            }
            return endPos;
        }
        public override ValueTask<string> Solve_1() => new($"{successCheats(2, 100)}");

        public override ValueTask<string> Solve_2() => new($"{successCheats(20, 100)}");
    }
}
