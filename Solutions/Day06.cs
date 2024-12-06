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
        HashSet<coord> obsticles;
        private readonly coord startingPosition;
        private readonly coord startingVelocity;
        Dictionary<coord, HashSet<coord>> visitedS1;
        public Day06()
        {
            obsticles = [];
            startingVelocity = new coord(0, -1);
            for (int row = 0; row < _input.Length; row++)
            {
                for (int col = 0; col < _input[0].Length; col++)
                {
                    if (_input[row][col] == '#')
                        obsticles.Add(new coord(col, row));
                    if (_input[row][col] == '^')
                        startingPosition = new coord(col, row);
                }
            }
        }
        public override ValueTask<string> Solve_1()
        {
            coord velocity = new coord(startingVelocity);
            coord position = new coord(startingPosition);
            Dictionary<coord, HashSet<coord>> visited = [];//pos -> velocities we entered the position with
            while (move(ref position, ref velocity, ref visited) == MoveResult.Ok) ;
            visitedS1 = visited;
            return new($"{visited.Count}");
        }

        public override ValueTask<string> Solve_2()
        {
            int ans = 0;
            int entries = 0;
            int exits = 0;
            //optimization1: limit search space to obsticles that are in the way of guard path
            //(we know those from visited set from first solution and velocities in each path)
            HashSet<coord> possibleExtraObsticles = [];
            foreach (var kvPair in visitedS1.Skip(1))
            {
                foreach (var velocity in kvPair.Value)
                {
                    //we don't want double obsticling
                    //so we can just Add and remove to instanced obsticles later
                    if (!obsticles.Contains(kvPair.Key))
                        possibleExtraObsticles.Add(kvPair.Key);
                }
            }
            possibleExtraObsticles.Remove(startingPosition);//in case it got added

            foreach (coord key in possibleExtraObsticles)
            {
                coord position = new(startingPosition);
                coord velocity = new(startingVelocity);
                Dictionary<coord, HashSet<coord>> visited = [];
                obsticles.Add(key);
                while (move(ref position, ref velocity, ref visited) == MoveResult.Ok) ;
                var x = move(ref position, ref velocity, ref visited);
                if (x == MoveResult.enteredLoop)
                    ans++;
                if (x == MoveResult.exitedMap)
                    exits++;
                obsticles.Remove(key);
                entries++;
            }

            return new($"{ans}");
        }

        record coord
        {
            public int col;
            public int row;

            public coord(coord c)//copy constructor
                => (col, row) = (c.col, c.row);
            public void turnRight()
            {
                (col, row) = (row, col);
                col *= -1;
            }
            /*          ^^^^^^^^^^^
             *      moving UP        col  0, row -1
             *      moving RIGHT     col +1, row  0
             *      moving DOWN      col  0, row +1
             *      moving LEFT      col -1, row  0
             *      so turning right => new column speed is -old row speed
             *      new row speed is old column speed
             */
            public coord(int col, int row)
                => (this.col, this.row) = (col, row);
            public static coord operator +(coord a, coord b)
                => new(a.col + b.col, a.row + b.row);
        }
        enum MoveResult
        {
            Ok,
            exitedMap,
            enteredLoop
        }
        private MoveResult move(ref coord position, ref coord velocity, ref Dictionary<coord, HashSet<coord>> visited)
        {
            if (position.col < 0 || position.col >= _input[0].Length
            || position.row < 0 || position.row >= _input.Length)
                return MoveResult.exitedMap;
            if (visited.ContainsKey(position))
            {
                if (visited[position].Contains(velocity))
                    return MoveResult.enteredLoop;
                visited[position].Add(velocity);
            }
            else
            {
                visited.Add(position, new HashSet<coord> { velocity });
            }
            if (this.obsticles.Contains(position + velocity))
                velocity.turnRight();//we would hit an obsticle
            else
                position += velocity;
            return MoveResult.Ok;
        }
    }
}
