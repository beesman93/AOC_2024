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
            LeftBox = 4,
            RightBox = 5,
        }
        tile[,] map1;
        tile[,] map2;
        List<(int x, int y)> instructions;
        (int x, int y) robotInitPos;
        public Day15()
        {
            int lineSplit = 0;
            while (_input[lineSplit] != "") lineSplit++;
            map1 = new tile[_input[0].Length, lineSplit];
            map2 = new tile[_input[0].Length*2, lineSplit];
            instructions = [];
            for(int y=0; y<lineSplit; y++)
            {
                for(int x = 0; x < _input[y].Length; x++)
                {
                    switch (_input[y][x])
                    {
                        case '#':
                            map1[x, y] = tile.Wall;
                            map2[x * 2, y] = tile.Wall;
                            map2[x * 2 + 1 , y] = tile.Wall;
                            break;
                        case '.':
                            map1[x, y] = tile.None;
                            map2[x * 2, y] = tile.None;
                            map2[x * 2 + 1, y] = tile.None;
                            break;
                        case 'O':
                            map1[x, y] = tile.Box;
                            map2[x * 2, y] = tile.LeftBox;
                            map2[x * 2 + 1, y] = tile.RightBox;
                            break;
                        case '@':
                            map1[x, y] = tile.Robot;
                            map2[x * 2, y] = tile.Robot;
                            map2[x * 2 + 1, y] = tile.None;
                            robotInitPos = (x, y);
                            break;
                        default:
                            throw new InvalidDataException("Invalid map.");
                    }
                }
            }
            for (int y=lineSplit;y<_input.Length;y++)
            {
                foreach (char c in _input[y])
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
        public override ValueTask<string> Solve_1() => new($"{solve(ref map1, robotInitPos.x, robotInitPos.y)}");
        public override ValueTask<string> Solve_2() => new($"{solve(ref map2, robotInitPos.x*2, robotInitPos.y)}");

        private long solve(ref tile[,] map, int robotX, int robotY)
        {
            long ans = 0;
            foreach (var (x, y) in instructions)
            {
                if (move(ref map, robotX, robotY, x, y))
                {
                    robotX += x;
                    robotY += y;
                }
            }
            for (int x = 0; x < map.GetLength(0); x++)
                for (int y = 0; y < map.GetLength(1); y++)
                    if (map[x, y] == tile.Box || map[x,y] == tile.LeftBox)
                        ans += x + 100 * y;
            return ans;
        }

        private bool move(ref tile[,]map, int x, int y, int dx, int dy)
        {
            tile T = map[x, y];
            if (T == tile.Wall) return false;
            if (T == tile.None) return true;
            if (T == tile.Robot || T == tile.Box)
            {
                if (move(ref map, x + dx, y + dy, dx, dy))
                {
                    map[x + dx, y + dy] = T;
                    map[x, y] = tile.None;
                    return true;
                }
                return false;
            }
            //(T == tile.LeftBox || T == tile.RightBox) -- part 2
            int xLeftBox = x;
            int yLeftBox = y;
            int xRightBox = x;
            int yRightBox = y;
            if (T == tile.RightBox) xLeftBox--;
            if (T == tile.LeftBox) xRightBox++;
            if (dx == 0)//up and down, just check both box parts can move
            {
                /*
                 * if one side fails, the other side should not move, revert changes down the line
                 */
                var mapCopy = (tile[,])map.Clone();
                if (move(ref map,xLeftBox+dx, yLeftBox+dy, dx, dy) && move(ref map,xRightBox+dx, yRightBox+dy, dx, dy))
                {
                    map[xLeftBox + dx, yLeftBox + dy] = tile.LeftBox;
                    map[xRightBox + dx, yRightBox + dy] = tile.RightBox;
                    map[xLeftBox, yLeftBox] = tile.None;
                    map[xRightBox, yRightBox] = tile.None;
                    return true;
                }
                map = mapCopy;
            }
            else if (dx > 0)//right --> check for RightBox like it was robot
            {
                if (move(ref map, xRightBox+dx, yRightBox+dy, dx, dy))
                {
                    map[xLeftBox + dx, yLeftBox + dy] = tile.LeftBox;
                    map[xRightBox + dx, yRightBox + dy] = tile.RightBox;
                    map[x, y] = tile.None;
                    return true;
                }
            }
            else if(dx<0)//left --> check for LeftBox like it was robot
            {
                if (move(ref map, xLeftBox + dx, yLeftBox + dy, dx, dy))
                {
                    map[xLeftBox + dx, yLeftBox + dy] = tile.LeftBox;
                    map[xRightBox + dx, yRightBox + dy] = tile.RightBox;
                    map[x, y] = tile.None;
                    return true;
                }
            }
            return false;
        }
        private void printMap(tile[,] map)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    switch (map[x, y])
                    {
                        case tile.None:
                            Console.Write('.');
                            break;
                        case tile.Wall:
                            Console.Write('#');
                            break;
                        case tile.Box:
                            Console.Write('O');
                            break;
                        case tile.Robot:
                            Console.Write('@');
                            break;
                        case tile.LeftBox:
                            Console.Write('[');
                            break;
                        case tile.RightBox:
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
