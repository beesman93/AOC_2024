using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCHelper;
using Spectre.Console;

namespace AOC_2024
{
    internal class Day14 : BaseDayWithInput
    {
        const int BATHROOM_WIDTH = 101;
        const int BATHROOM_HEIGHT = 103;
        enum quadrant
        {
            top_left,
            top_right,
            bottom_left,
            bottom_right,
            middle
        }
        private class Robot
        {
            private static int XX = BATHROOM_WIDTH;
            private static int YY = BATHROOM_HEIGHT;
            public int X { get; set; }
            public int Y { get; set; }

            private readonly (int x, int y) _velocity;

            public quadrant Quadrant
            {
                get
                {
                    if (X < XX / 2 && Y < YY / 2)
                    {
                        return quadrant.top_left;
                    }
                    else if (X > XX / 2 && Y < YY / 2)
                    {
                        return quadrant.top_right;
                    }
                    else if (X < XX / 2 && Y > YY / 2)
                    {
                        return quadrant.bottom_left;
                    }
                    else if (X > XX / 2 && Y > YY / 2)
                    {
                        return quadrant.bottom_right;
                    }
                    else
                    {
                        return quadrant.middle;
                    }
                }
            }
            public Robot(string init)
            {
                var p_v = init.Split(' ');
                var p = p_v[0].Split('=')[1];
                var v = p_v[1].Split('=')[1];
                this.X = int.Parse(p.Split(',')[0]);
                this.Y = int.Parse(p.Split(',')[1]);
                this._velocity = (int.Parse(v.Split(',')[0]), int.Parse(v.Split(',')[1]));
            }
            public Robot(int X, int Y, (int x, int y) velocity) => (this.X, this.Y, this._velocity) = (X, Y, velocity);

            public override string ToString()
            {
                return $"{X}:{Y} --- ({_velocity.ToString()})";
            }
            public void Move()
            {
                X = (X+_velocity.x)%XX;
                Y = (Y + _velocity.y) % YY;
                if(X < 0) X += XX;
                if (Y < 0) Y += YY;
            }
        }
        List<Robot> robots;
        public Day14()
        {
            robots = [];
            foreach(var line in _input)
            {
                robots.Add(new Robot(line));
            }
        }
        public override async ValueTask<string> Solve_1()
        {
            for(int i =0;i<100;i++) robots.ForEach(robot => robot.Move());
            Dictionary<quadrant, int> quad_count = [];
            foreach(quadrant quad in Enum.GetValues(typeof(quadrant)))
                quad_count[quad] = 0;
            foreach (var robot in robots)
                quad_count[robot.Quadrant]++;
            return new($"{quad_count[quadrant.top_left]
                * quad_count[quadrant.top_right]
                * quad_count[quadrant.bottom_right]
                * quad_count[quadrant.bottom_left]
                }");
        }

        public override ValueTask<string> Solve_2()
        {
            // the robots forming tree means none are overlapping turned out
            for (int n = 100; n < 500_000; n++)
            {
                HashSet<(int x, int y)> robotClusters = [];
                foreach (var robot in robots)
                {
                    if(robotClusters.Contains((robot.X, robot.Y)))
                        break;
                    robotClusters.Add((robot.X, robot.Y));
                }
                if(robotClusters.Count == robots.Count)
                {
                    //printRobots();
                    return new($"{n}");
                }
                robots.ForEach(robot => robot.Move());
            }
            return new($"No tree I guess.");
        }

        public void printRobots()
        {
            bool[,] bathroom = new bool[BATHROOM_HEIGHT,BATHROOM_WIDTH];
            foreach (var robot in robots)
            {
                bathroom[robot.Y, robot.X] = true;
            }
            for(int i=0;i<BATHROOM_HEIGHT; i++)
            {
                for (int j = 0; j < BATHROOM_WIDTH; j++)
                {
                    if (bathroom[i, j])
                    {
                        Console.Write('#');
                    }
                    else
                    {
                        Console.Write('.');
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
