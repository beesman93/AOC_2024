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
        private (int x, int y) turnRight((int x, int y) velocity) => (velocity.y, -velocity.x);
        HashSet<(int x, int y)> obsticles;
        private readonly (int x, int y) startingPosition;
        private readonly (int x, int y) startingVelocity;
        bool trackVisited;
        HashSet<(int x, int y)> visitedS1;
        Dictionary
        <
        ((int x, int y) position, (int x, int y) velocity), //start
        ((int x, int y) position, (int x, int y) velocity)  //end
        >
        jumpCache;
        public Day06()
        {
            obsticles = [];
            visitedS1 = [];
            jumpCache = [];
            startingVelocity = (0, -1);
            for (int x = 0; x < _input.Length; x++)
            {
                for (int y = 0; y < _input[0].Length; y++)
                {
                    switch (_input[x][y])
                    {
                        case '#':
                            obsticles.Add((x, y));
                            break;
                        case '^':
                            startingPosition = (x, y);
                            startingVelocity = (-1, 0);
                            break;
                        case '>':
                            startingPosition = (x, y);
                            startingVelocity = (0, 1);
                            break;
                        case 'v':
                            startingPosition = (x, y);
                            startingVelocity = (1, 0);
                            break;
                        case '<':
                            startingPosition = (x, y);
                            startingVelocity = (0, -1);
                            break;
                    }
                }
            }
        }
        private bool moveTillNextObsticleOrExit(ref (int x, int y) position, ref (int x, int y) velocity, (int x, int y) extraObsticle)
        {
            while (!obsticles.Contains((position.x + velocity.x, position.y + velocity.y)))
            {
                if (extraObsticle.x == position.x + velocity.x && extraObsticle.y == position.y + velocity.y)
                {
                    velocity = turnRight(velocity);
                    return true;//hit extra obsticle
                }
                if (trackVisited)
                    visitedS1.Add((position.x, position.y));
                position = (position.x + velocity.x, position.y + velocity.y);
                if(position.x < 0 || position.x >= _input.Length || position.y < 0 || position.y >= _input[0].Length)
                {
                    velocity = (0, 0);//map exit
                    break;
                }
            }
            velocity = turnRight(velocity);
            return false;//didn't hit extra obsticle
        }

        private bool isObsticleInTheJump((int x, int y) start, (int x, int y) end, (int x, int y) obsticle)
        {
            if (start.x == end.x)
            {
                if (start.y < end.y)
                {
                    if (obsticle.x == start.x && obsticle.y >= start.y && obsticle.y <= end.y)
                        return true;
                }
                else
                {
                    if (obsticle.x == start.x && obsticle.y <= start.y && obsticle.y >= end.y)
                        return true;
                }
            }
            else
            {
                if (start.x < end.x)
                {
                    if (obsticle.y == start.y && obsticle.x >= start.x && obsticle.x <= end.x)
                        return true;
                }
                else
                {
                    if (obsticle.y == start.y && obsticle.x <= start.x && obsticle.x >= end.x)
                        return true;
                }
            }
            return false;
        }
        private void jumpToNext(ref (int x, int y) position,
                                ref (int x, int y) velocity,
                                (int x, int y)? extraObsticle = null)
        {
            if (jumpCache.ContainsKey((position, velocity)))
            {
                if (extraObsticle == null
                || !isObsticleInTheJump(
                    position,
                    jumpCache[(position, velocity)].position,
                    ((int x, int y))extraObsticle))
                {
                    (position, velocity) = jumpCache[(position, velocity)];
                    return;
                }
            }
            var start = (position, velocity);
            if(!moveTillNextObsticleOrExit(ref position, ref velocity, extraObsticle??(-1,-1)))
                jumpCache[start] = (position, velocity);
        }
        public override ValueTask<string> Solve_1()
        {
            trackVisited = true;
            (int x, int y) position = (startingPosition.x, startingPosition.y);
            (int x, int y) velocity = (startingVelocity.x, startingVelocity.y);
            while (velocity != (0, 0))
                jumpToNext(ref position, ref velocity);
            trackVisited = false;
            return new($"{visitedS1.Count}");
        }

        public override ValueTask<string> Solve_2()
        {
            int ans = 0;
            foreach (var obsticleTry in visitedS1.Skip(1))
            {
                (int x, int y) position = (startingPosition.x, startingPosition.y);
                (int x, int y) velocity = (startingVelocity.x, startingVelocity.y);
                HashSet<((int x, int y)position,(int x, int y)velocity)> visited = [];
                while (velocity != (0, 0))
                {
                    if (visited.Contains((position,velocity)))
                    { 
                        ans++;//found a loop
                        break;
                    }
                    visited.Add((position, velocity));
                    jumpToNext(ref position, ref velocity, obsticleTry);
                }
            }
            return new($"{ans}");
        }


    }
}

//TURN RIGHT LOGIC
//x are rows, y are columns, increasing top to bottom and left to right
//velocity x=1 --> moving down  (1,0)
//velocity x=-1 --> moving up   (-1,0)
//velocity y=1 --> moving right (0,1)
//velocity y=-1 --> moving left (0,-1)
//turn right is 
// up       ->  right   ->  down    ->  left    ->  up
// (-1,0)   ->  (0,1)   ->  (1,0)   ->  (0,-1)  ->  (-1,0)
//turn right is
// new X = old Y
// new Y = -old X
// so turnRight(x,y) => (y,-x)
