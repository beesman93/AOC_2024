using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCHelper;

namespace AOC_2024
{
    internal class Day16 : BaseDayWithInput
    {
        const int TURN_PENTALTY = 1000;
        enum Tile
        {
            empty,
            wall,
            start,
            end
        }
        private static Tile CharToTile(char c) => c switch
        {
            '.' => Tile.empty,
            '#' => Tile.wall,
            'S' => Tile.start,
            'E' => Tile.end,
            _ => throw new InvalidDataException("Invalid character.")
        };
        private static char TileToChar(Tile t) => t switch
        {
            Tile.empty => '.',
            Tile.wall => '#',
            Tile.start => 'S',
            Tile.end => 'E',
            _ => throw new InvalidDataException("Unknown tile.")
        };

        private static (int x, int y)[] dirrections = new(int x, int y)[]{(0,-1),(1,0),(0,1),(-1,0)};//up,right,down,left
        private static int turnLeft(int dirrectionsIdx) => (dirrectionsIdx + 3) % 4;
        private static int turnRight(int dirrectionsIdx) => (dirrectionsIdx + 1) % 4;
        private static int turnBack(int dirrectionsIdx) => (dirrectionsIdx + 2) % 4;

        Tile[,] maze;//x,y x is column (0->N left to right) y is row (0->M top to bottom)
        (int x, int y) start;
        (int x, int y) end;
        Dictionary<(int x, int y, int dir), int> distancesToEnd;
        Dictionary<(int x, int y, int dir), int> distancesToStart;
        int shortestDistance;

        public Day16()
        {
            maze = new Tile[_input[0].Length, _input.Length];
            for (int y = 0; y < _input.Length; y++)
            {
                for (int x = 0; x < _input[y].Length; x++)
                {
                    maze[x, y] = CharToTile(_input[y][x]);
                    if (maze[x, y] == Tile.start)   start = (x, y);
                    if (maze[x, y] == Tile.end)     end = (x, y);
                }
            }
            distancesToEnd = [];
            distancesToStart = [];
        }
        public override ValueTask<string> Solve_1()
        {
            shortestDistance = int.MaxValue;
            distancesToEnd = new();
            PriorityQueue<(int x, int y, int dir), int> queue = new();
            queue.Enqueue((start.x, start.y, 1), 0);//starts facing east (right)
            while (queue.TryDequeue(out var node, out int distance))
            {
                if (distancesToEnd.ContainsKey(node))
                    continue;
                if (node.x == end.x && node.y == end.y && distance < shortestDistance)
                    shortestDistance = distance;
                distancesToEnd[node] = distance;
                int newX = node.x + dirrections[node.dir].x;
                int newY = node.y + dirrections[node.dir].y;
                if (newX >= 0 && newX < maze.GetLength(0) && newY >= 0 && newY < maze.GetLength(1) && maze[newX, newY] != Tile.wall)
                    queue.Enqueue((newX, newY, node.dir), distance + 1);
                queue.Enqueue((node.x, node.y, turnRight(node.dir)), distance + TURN_PENTALTY);
                queue.Enqueue((node.x, node.y, turnLeft(node.dir)), distance + TURN_PENTALTY);
            }
            return new($"{shortestDistance}");
        }

        public override ValueTask<string> Solve_2()
        {
            int shortestToStart = int.MaxValue;
            distancesToStart = new();
            PriorityQueue<(int x, int y, int dir), int> queue = new();
            queue.Enqueue((end.x, end.y, 0), 0);
            queue.Enqueue((end.x, end.y, 1), 0);
            queue.Enqueue((end.x, end.y, 2), 0);
            queue.Enqueue((end.x, end.y, 3), 0);
            while (queue.TryDequeue(out var node, out int distance))
            {
                if (distancesToStart.ContainsKey(node))
                    continue;
                if (node.x == start.x && node.y == start.y && distance < shortestToStart)
                    shortestToStart = distance;
                distancesToStart[node] = distance;
                int newX = node.x + dirrections[turnBack(node.dir)].x;
                int newY = node.y + dirrections[turnBack(node.dir)].y;
                if (newX >= 0 && newX < maze.GetLength(0) && newY >= 0 && newY < maze.GetLength(1) && maze[newX, newY] != Tile.wall)
                    queue.Enqueue((newX, newY, node.dir), distance + 1);
                queue.Enqueue((node.x, node.y, turnRight(node.dir)), distance + TURN_PENTALTY);
                queue.Enqueue((node.x, node.y, turnLeft(node.dir)), distance + TURN_PENTALTY);
            }
            HashSet<(int x, int y)> partOfOptimalPaths = [];
            foreach (var node in distancesToEnd.Keys)
            {
                if (distancesToEnd.ContainsKey(node))
                {
                    if (distancesToStart[node]+distancesToEnd[node] == shortestDistance)
                    {
                        partOfOptimalPaths.Add((node.x, node.y));
                    }
                }
            }
            return new($"{partOfOptimalPaths.Count}");
        }

        public void PrintMaze()
        {
            for (int y = 0; y < maze.GetLength(1); y++)
            {
                for (int x = 0; x < maze.GetLength(0); x++)
                {
                    Console.Write(TileToChar(maze[x, y]));
                }
                Console.WriteLine();
            }
        }
    }
}
