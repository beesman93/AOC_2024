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
        const bool VISUALIZE = false;
        const int SLOWMO_CHANGE_TRESHOLD = 15;
        enum Tile
        {
            None = 0,
            Wall = 1,
            Box = 2,
            Robot = 3,
            LeftBox = 4,
            RightBox = 5,
        }
        Tile[,] map1;
        Tile[,] map2;
        List<(int x, int y)> instructions;
        (int x, int y) robotInitPos;
        public Day15()
        {
            int lineSplit = 0;
            while (_input[lineSplit] != "") lineSplit++;
            map1 = new Tile[_input[0].Length, lineSplit];
            map2 = new Tile[_input[0].Length*2, lineSplit];
            instructions = [];
            for(int y=0; y<lineSplit; y++)
            {
                for(int x = 0; x < _input[y].Length; x++)
                {
                    switch (_input[y][x])
                    {
                        case '#':
                            map1[x, y] = Tile.Wall;
                            map2[x * 2, y] = Tile.Wall;
                            map2[x * 2 + 1 , y] = Tile.Wall;
                            break;
                        case '.':
                            map1[x, y] = Tile.None;
                            map2[x * 2, y] = Tile.None;
                            map2[x * 2 + 1, y] = Tile.None;
                            break;
                        case 'O':
                            map1[x, y] = Tile.Box;
                            map2[x * 2, y] = Tile.LeftBox;
                            map2[x * 2 + 1, y] = Tile.RightBox;
                            break;
                        case '@':
                            map1[x, y] = Tile.Robot;
                            map2[x * 2, y] = Tile.Robot;
                            map2[x * 2 + 1, y] = Tile.None;
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
        public override ValueTask<string> Solve_1() => new($"{solve(ref map1, robotInitPos.x, robotInitPos.y, visualize: VISUALIZE)}");
        public override ValueTask<string> Solve_2() => new($"{solve(ref map2, robotInitPos.x*2, robotInitPos.y, visualize: VISUALIZE)}");

        private long solve(ref Tile[,] map, int robotX, int robotY, bool visualize = false)
        {
            if (visualize) Console.Clear();
            if (visualize) Console.CursorVisible = false;
            if (visualize) printMap(map);
            Tile[,] mapClone=new Tile[0,0];
            int instrCount = 0;
            long ans = 0;
            foreach (var (x, y) in instructions)
            {
                if (visualize) mapClone = (Tile[,])map.Clone();
                if (move(ref map, robotX, robotY, x, y))
                {
                    robotX += x;
                    robotY += y;

                }

                if (visualize)
                {
                    int changeCount = 0;
                    for (int xx = 0; xx < map.GetLength(0); xx++)
                    {
                        for (int yy = 0; yy < map.GetLength(1); yy++)
                        {
                            if (map[xx, yy] != mapClone[xx, yy])
                            {
                                changeCount++;
                            }
                        }
                    }
                    if (changeCount > SLOWMO_CHANGE_TRESHOLD)
                        Thread.Sleep(1000);
                    for (int xx = 0; xx < map.GetLength(0); xx++)
                    {
                        for (int yy = 0; yy < map.GetLength(1); yy++)
                        {
                            if (map[xx, yy] != mapClone[xx, yy])
                            {
                                Console.SetCursorPosition(xx, yy);
                                switch (map[xx, yy])
                                {
                                    case Tile.None:
                                        Console.Write(' ');
                                        break;
                                    case Tile.Wall:
                                        Console.ForegroundColor = ConsoleColor.DarkRed;
                                        Console.Write('█');
                                        Console.ForegroundColor = ConsoleColor.White;
                                        break;
                                    case Tile.Box:
                                        Console.Write('O');
                                        break;
                                    case Tile.Robot:
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.Write('@');
                                        Console.ForegroundColor = ConsoleColor.White;
                                        break;
                                    case Tile.LeftBox:
                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                        Console.Write('[');
                                        Console.ForegroundColor = ConsoleColor.White;
                                        break;
                                    case Tile.RightBox:
                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                        Console.Write(']');
                                        Console.ForegroundColor = ConsoleColor.White;
                                        break;
                                }
                            }
                        }
                    }
                    if (changeCount > SLOWMO_CHANGE_TRESHOLD)
                        Thread.Sleep(1000);
                    if (instrCount < 2 || instrCount > instructions.Count - 3)
                        Thread.Sleep(500);
                    else if (instrCount < 10 || instrCount > instructions.Count - 10)
                        Thread.Sleep(100);
                    else if(instrCount < 50 || instrCount > instructions.Count - 50)
                        Thread.Sleep(50);
                    else if (instrCount < 100 || instrCount > instructions.Count - 100)
                        Thread.Sleep(1);
                    instrCount++;
                }
            }
            for (int x = 0; x < map.GetLength(0); x++)
                for (int y = 0; y < map.GetLength(1); y++)
                    if (map[x, y] == Tile.Box || map[x,y] == Tile.LeftBox)
                        ans += x + 100 * y;
            if (visualize) Console.Clear();
            return ans;
        }

        private bool move(ref Tile[,]map, int x, int y, int dx, int dy)
        {
            Tile T = map[x, y];
            if (T == Tile.Wall) return false;
            if (T == Tile.None) return true;
            if (T == Tile.Robot || T == Tile.Box)
            {
                if (move(ref map, x + dx, y + dy, dx, dy))
                {
                    map[x + dx, y + dy] = T;
                    map[x, y] = Tile.None;
                    return true;
                }
                return false;
            }
            //(T == tile.LeftBox || T == tile.RightBox) -- part 2
            int xLeftBox = x;
            int yLeftBox = y;
            int xRightBox = x;
            int yRightBox = y;
            if (T == Tile.RightBox) xLeftBox--;
            if (T == Tile.LeftBox) xRightBox++;
            if (dx == 0)//up and down, just check both box parts can move
            {
                /*
                 * if one side fails, the other side should not move, revert changes down the line
                 */
                var mapCopy = (Tile[,])map.Clone();
                if (move(ref map,xLeftBox+dx, yLeftBox+dy, dx, dy) && move(ref map,xRightBox+dx, yRightBox+dy, dx, dy))
                {
                    map[xLeftBox + dx, yLeftBox + dy] = Tile.LeftBox;
                    map[xRightBox + dx, yRightBox + dy] = Tile.RightBox;
                    map[xLeftBox, yLeftBox] = Tile.None;
                    map[xRightBox, yRightBox] = Tile.None;
                    return true;
                }
                map = mapCopy;
            }
            else if (dx > 0)//right --> check for RightBox like it was robot
            {
                if (move(ref map, xRightBox+dx, yRightBox+dy, dx, dy))
                {
                    map[xLeftBox + dx, yLeftBox + dy] = Tile.LeftBox;
                    map[xRightBox + dx, yRightBox + dy] = Tile.RightBox;
                    map[x, y] = Tile.None;
                    return true;
                }
            }
            else if(dx<0)//left --> check for LeftBox like it was robot
            {
                if (move(ref map, xLeftBox + dx, yLeftBox + dy, dx, dy))
                {
                    map[xLeftBox + dx, yLeftBox + dy] = Tile.LeftBox;
                    map[xRightBox + dx, yRightBox + dy] = Tile.RightBox;
                    map[x, y] = Tile.None;
                    return true;
                }
            }
            return false;
        }
        private void printMap(Tile[,] map)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    switch (map[x, y])
                    {
                        case Tile.None:
                            Console.Write(' ');
                            break;
                        case Tile.Wall:
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.Write('█');
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                        case Tile.Box:
                            Console.Write('O');
                            break;
                        case Tile.Robot:
                            Console.Write('@');
                            break;
                        case Tile.LeftBox:
                            Console.Write('[');
                            break;
                        case Tile.RightBox:
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
