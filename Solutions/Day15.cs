using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AoCHelper;

namespace AOC_2024
{
    internal class Day15 : BaseDayWithInput
    {
        enum tile
        {
            None = 0,
            Wall = 1,
            Box = 2,
            Robot = 3,
            BoxButt = 4,
        }
        tile[,] map;
        tile[,] map2;
        List<(int x, int y)> instructions;
        (int x, int y) robotInitPos;
        public Day15()
        {
            int Ymax = 0;
            foreach(var line in _input)
            {
                if (line == "")
                    break;
                Ymax++;
            }
            map = new tile[_input[0].Length, Ymax];
            map2 = new tile[_input[0].Length*2, Ymax];
            instructions = [];
            bool firstPart = true;
            int y = 0;
            foreach (var line in _input)
            {
                if(line=="")
                {
                    firstPart = false;
                    continue;
                }
                if (firstPart)
                {
                    int x = 0;
                    foreach (var c in line)
                    {
                        switch (c)
                        {
                            case '#':
                                map[x, y] = tile.Wall;
                                map2[x * 2, y] = tile.Wall;
                                map2[x * 2 + 1 , y] = tile.Wall;
                                break;
                            case '.':
                                map[x, y] = tile.None;
                                map2[x * 2, y] = tile.None;
                                map2[x * 2 + 1, y] = tile.None;
                                break;
                            case 'O':
                                map[x, y] = tile.Box;
                                map2[x * 2, y] = tile.Box;
                                map2[x * 2 + 1, y] = tile.BoxButt;
                                break;
                            case '@':
                                map[x, y] = tile.Robot;
                                map2[x * 2, y] = tile.Robot;
                                map2[x * 2 + 1, y] = tile.None;
                                robotInitPos = (x, y);
                                break;
                            default:
                                throw new InvalidDataException("Invalid map.");
                        }
                        x++;
                    }
                    y++;
                }
                else
                {
                    foreach (char c in line)
                    {
                        switch (c)
                        {
                            case '^':
                                instructions.Add((0, -1));
                                break;
                            case 'v':
                                instructions.Add((0, 1));
                                break;
                            case '<':
                                instructions.Add((-1, 0));
                                break;
                            case '>':
                                instructions.Add((1, 0));
                                break;
                        }
                    }
                }

            }
        }
        public override ValueTask<string> Solve_1()
        {
            long ans = 0;
            int robotX = robotInitPos.x;
            int robotY = robotInitPos.y;
            //printMap(false);
            foreach (var (x, y) in instructions)
            {
                if (move(robotX, robotY, x, y))
                {
                    robotX += x;
                    robotY += y;
                }
                //printMap(false);
            }
            //printMap(false);
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    if (map[x, y] == tile.Box)
                        ans+=x+100*y;
                }
            }
            return new($"{ans}");
        }

        public override ValueTask<string> Solve_2()
        {
            long ans = 0;
            int robotX = robotInitPos.x * 2;
            int robotY = robotInitPos.y;
            //printMap(true);
            int instNum = 1;
            foreach (var (x, y) in instructions)
            {
                if (move2(robotX, robotY, x, y))
                {
                    robotX += x;
                    robotY += y;
                }
                //printMap(true);
                instNum++;
            }
            //printMap(true);
            for (int x = 0; x < map2.GetLength(0); x++)
            {
                for (int y = 0; y < map2.GetLength(1); y++)
                {
                    if (map2[x, y] == tile.Box)
                        ans += x + 100 * y;
                }
            }
            return new($"{ans}");
        }

        private bool move2(int x, int y, int dx, int dy)
        {
            tile T = map2[x, y];
            if (T == tile.Wall)
                return false;
            if (T == tile.None)
                return true;
            if (T == tile.Robot)
            {
                if (move2(x + dx, y + dy, dx, dy))
                {
                    map2[x + dx, y + dy] = T;
                    map2[x, y] = tile.None;
                    return true;
                }
            }
            //if (T == tile.Box || T == tile.BoxButt)
            int xBox = x;
            int yBox = y;
            int xBoxButt = x;
            int yBoxButt = y;
            if (T == tile.BoxButt) xBox--;
            if (T == tile.Box) xBoxButt++;
            if (dx == 0)//up and down, just check both box parts can move
            {
                /*
                 * if one side fails, the other side should not move, revert changes down the line
                 */
                var map2Copy = (tile[,])map2.Clone();
                if (move2(xBox+dx, yBox+dy, dx, dy) && move2(xBoxButt+dx, yBoxButt+dy, dx, dy))
                {
                    map2[xBox + dx, yBox + dy] = tile.Box;
                    map2[xBoxButt + dx, yBoxButt + dy] = tile.BoxButt;
                    map2[xBox, yBox] = tile.None;
                    map2[xBoxButt, yBoxButt] = tile.None;
                    return true;
                }
                map2 = map2Copy;
                return false;
            }
            if (dx > 0)//right --> check for boxButt like it was robot
            {
                if (move2(xBoxButt+dx, yBoxButt+dy, dx, dy))
                {
                    map2[xBox + dx, yBox + dy] = tile.Box;
                    map2[xBoxButt + dx, yBoxButt + dy] = tile.BoxButt;
                    map2[x, y] = tile.None;
                    return true;
                }
                return false;
            }
            if(dx<0)//left --> check for box like it was robot
            {
                if (move2(xBox + dx, yBox + dy, dx, dy))
                {
                    map2[xBox + dx, yBox + dy] = tile.Box;
                    map2[xBoxButt + dx, yBoxButt + dy] = tile.BoxButt;
                    map2[x, y] = tile.None;
                    return true;
                }
                return false;
            }
            return false;
        }

        private bool move(int x, int y, int dx, int dy)
        {
            tile T = map[x, y];
            if (T == tile.Wall)
                return false;
            if (T == tile.None)
                return true;
            //if (T == tile.Box || T == tile.Robot)
            if (move(x + dx, y + dy, dx, dy))
            {
                map[x + dx, y + dy] = T;
                map[x, y] = tile.None;
                return true;
            }
            return false;
        }

        private void printMap(bool p2)
        {
            var pMap = p2 ? this.map2 : this.map;
            for (int y = 0; y < pMap.GetLength(1); y++)
            {
                for (int x = 0; x < pMap.GetLength(0); x++)
                {
                    switch (pMap[x, y])
                    {
                        case tile.None:
                            Console.Write('.');
                            break;
                        case tile.Wall:
                            Console.Write('#');
                            break;
                        case tile.Box:
                            Console.Write(p2?'[':'O');
                            break;
                        case tile.Robot:
                            Console.Write('@');
                            break;
                        case tile.BoxButt:
                            Console.Write(']');
                            break;
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine("---");
        }

    }
}
