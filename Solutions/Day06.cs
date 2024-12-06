using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCHelper;

namespace AOC_2024
{
    internal class Day06 : BaseDayWithInput
    {

        public Day06()
        {

        }
        public override ValueTask<string> Solve_1()
        {
            long ans = 0;


            Tuple<int, int> velocity = new(0, -1);
            Tuple<int, int> position = new(0, 0);

            //x is columns, y is rows
            int xMax = _input[0].Length;
            int yMax = _input.Length;

            //max 130 per direction
            HashSet<long> obsticles = []; // pos.x * 1'000 + pos.y
            HashSet<long> visitedWithoutDir = []; // pos.x * 1'000 + pos.y
            HashSet<long> visited = []; // velocityIdx *1'000'000 pos.x * 1'000 + pos.y

            for(int row =0; row < yMax; row++)
            {
                for (int col = 0; col < xMax; col++)
                {
                    if (_input[row][col] == '#')
                    {
                        obsticles.Add(col * 1_000 + row);
                    }
                    if (_input[row][col] == '^')
                    {
                        position = new Tuple<int, int>(col, row);
                    }
                }
            }

            while (true)
            {
                /*if (visited.Contains(velocityToInt(in velocity) + position.Item1 * 1_000 + position.Item2))
                    break;*/
                /*
                 * Looking for guard to leave map, not loop OOOOOMGGGGGGG
                 */

                visitedWithoutDir.Add(position.Item1 * 1_000 + position.Item2);
                visited.Add(velocityToInt(in velocity) + position.Item1 * 1_000 + position.Item2);

                Tuple<int, int> nextPosition = new(position.Item1 + velocity.Item1, position.Item2 + velocity.Item2);
                if (obsticles.Contains(nextPosition.Item1 * 1_000 + nextPosition.Item2))
                    turnRight(ref velocity);
                else
                    position = nextPosition;

                if( nextPosition.Item1 < 0 || nextPosition.Item1 >= yMax
                || nextPosition.Item2 < 0 || nextPosition.Item2 >= xMax)
                {
                    break;
                }
            }

            return new($"{visitedWithoutDir.Count}");
        }

        public override ValueTask<string> Solve_2()
        {
            //x is columns, y is rows
            int xMax = _input[0].Length;
            int yMax = _input.Length;

            Tuple<int, int> velocityOG = new(0, -1);
            Tuple<int, int> positionOG = new(0, 0);

            //max 130 per direction
            HashSet<long> obsticlesOG = []; // pos.x * 1'000 + pos.y

            List<Tuple<int, int>> posNewObsticles = [];

            for (int row = 0; row < yMax; row++)
            {
                for (int col = 0; col < xMax; col++)
                {
                    if (_input[row][col] == '#')
                    {
                        obsticlesOG.Add(col * 1_000 + row);
                    }
                    if (_input[row][col] == '^')
                    {
                        positionOG = new Tuple<int, int>(col, row);
                    }
                    else
                    {
                        posNewObsticles.Add(new Tuple<int, int>(col, row));
                    }
                }
            }

            int ans = 0;
            foreach (var obsticle in posNewObsticles)
            {
                HashSet<long> visitedWithoutDir = []; // pos.x * 1'000 + pos.y
                HashSet<long> visited = []; // velocityIdx *1'000'000 pos.x * 1'000 + pos.y
                var velocity = velocityOG;
                var position = positionOG;
                var obsticles = new HashSet<long>(obsticlesOG);
                obsticles.Add(obsticle.Item1 * 1_000 + obsticle.Item2);
                bool looptyLoop = false;
                while (true)
                {
                    if (visited.Contains(velocityToInt(in velocity) + position.Item1 * 1_000 + position.Item2))
                    {
                        looptyLoop = true;
                        ans++;
                        break;
                    }

                    visitedWithoutDir.Add(position.Item1 * 1_000 + position.Item2);
                    visited.Add(velocityToInt(in velocity) + position.Item1 * 1_000 + position.Item2);

                    Tuple<int, int> nextPosition = new(position.Item1 + velocity.Item1, position.Item2 + velocity.Item2);
                    if (obsticles.Contains(nextPosition.Item1 * 1_000 + nextPosition.Item2))
                        turnRight(ref velocity);
                    else
                        position = nextPosition;

                    if (nextPosition.Item1 < 0 || nextPosition.Item1 >= yMax
                    || nextPosition.Item2 < 0 || nextPosition.Item2 >= xMax)
                    {
                        break;
                    }
                }
            }

            return new($"{ans}");
        }

        public int velocityToInt(in Tuple<int, int> velocity)
        {
            int output = 0;
            if(velocity.Item1 == 0 && velocity.Item2 == -1)
            {
                output = 1;
            }
            else if (velocity.Item1 == 1 && velocity.Item2 == 0)
            {
                output = 2;
            }
            else if (velocity.Item1 == 0 && velocity.Item2 == 1)
            {
                output = 3;
            }
            else if (velocity.Item1 == -1 && velocity.Item2 == 0)
            {
                output = 4;
            }
            return output*1_000_000;
        }
        public void turnRight(ref Tuple<int, int> velocity)
        {
            //up, right, down, left
            /*  0,-1
             *  1,0
             *  0,1
             *  -1,0
             */
            if (velocity.Item1 == 0 && velocity.Item2 == -1)
            {
                velocity = new Tuple<int, int>(1, 0);
            }
            else if (velocity.Item1 == 1 && velocity.Item2 == 0)
            {
                velocity = new Tuple<int, int>(0, 1);
            }
            else if (velocity.Item1 == 0 && velocity.Item2 == 1)
            {
                velocity = new Tuple<int, int>(-1, 0);
            }
            else if (velocity.Item1 == -1 && velocity.Item2 == 0)
            {
                velocity = new Tuple<int, int>(0, -1);
            }
        }
    }
}
