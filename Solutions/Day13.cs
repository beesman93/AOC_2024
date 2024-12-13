using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCHelper;
using MathNet.Numerics.LinearAlgebra;

namespace AOC_2024
{
    internal class Day13 : BaseDayWithInput
    {

        class Claw
        { public int X { get; set; }
            public int Y { get; set; }
            public (int x, int y) ButtonA { get; set; }
            public (int x, int y) ButtonB { get; set; }

            public Dictionary<(int, int), int> cache = [];
            public int P1 { get; set; }
            public void solveP1()
            {
                P1 = minToTarget(X, Y);
                if (P1 >= int.MaxValue / 2)
                    P1 = 0;
            }
            private int minToTarget(int x, int y)
            {
                if (cache.ContainsKey((x, y)))
                    return cache[(x, y)];
                if(x==0&& y == 0)
                    return 0;
                if (x < 0 || y < 0)
                    return int.MaxValue/2;
                cache.Add(
                    (x, y),
                    Math.Min(
                        minToTarget(x - ButtonA.x, y - ButtonA.y) + 3,
                        minToTarget(x - ButtonB.x, y - ButtonB.y) + 1));
                return cache[(x, y)];
            }
            public long P2(long offset)
            {
                // X+10000000000000 = n*ButtonA.x + m*ButtonB.x
                // Y+10000000000000 = n*ButtonA.y + m*ButtonB.y
                // linear solve, check if n and m are integers
                // pray no input where buttons align linearly
                /*
                 * | ButtonA.x ButtonB.x | | n | = | X+10000000000000 |
                 * | ButtonA.y ButtonB.y | | m | = | Y+10000000000000 |
                 */
                long XX = X + offset;
                long YY = Y + offset;
                var matrix = Matrix<double>.Build.DenseOfArray(new double[,] 
                {
                    { ButtonA.x, ButtonB.x },
                    { ButtonA.y, ButtonB.y }
                });
                var colVector = Vector<double>.Build.Dense(new double[] 
                {
                    XX,
                    YY
                });
                Vector<double> SOLUTIONS = matrix.Solve(colVector);
                SOLUTIONS = SOLUTIONS.Map(x => Math.Round(x));
                (long A, long B) INT_SOLUTIONS = (Convert.ToInt64(SOLUTIONS[0]), Convert.ToInt64(SOLUTIONS[1]));
                if (INT_SOLUTIONS.A * ButtonA.x + INT_SOLUTIONS.B * ButtonB.x == XX
                 && INT_SOLUTIONS.A * ButtonA.y + INT_SOLUTIONS.B * ButtonB.y == YY)
                {
                    return INT_SOLUTIONS.A*3 + INT_SOLUTIONS.B;
                }
                return 0;
            }
        }
        List<Claw> claws;
        public Day13()
        {
            claws = [];
            for(int i=0;i< _input.Length; i+=4)
            {
                Claw claw = new();
                var line1 = _input[i];
                var line2 = _input[i+1];
                var line3 = _input[i+2];
                var A_lr = line1.Split(':')[1].Split(',').Select(x => int.Parse(x[2..])).ToArray();
                claw.ButtonA = (A_lr[0], A_lr[1]);
                var B_lr = line2.Split(':')[1].Split(',').Select(x => int.Parse(x[2..])).ToArray();
                claw.ButtonB = (B_lr[0], B_lr[1]);
                var prize = line3.Split(':')[1].Split(',').Select(x => int.Parse(x[3..])).ToArray();
                claw.X = prize[0];
                claw.Y = prize[1];
                claws.Add(claw);
            }
        }
        public override ValueTask<string> Solve_1()
        {
            long ans = 0;
            foreach(var claw in claws)
            {
                /*claw.solveP1();
                ans += claw.P1;*/
                ans += claw.P2(0);
            }
            return new($"{ans}");
        }

        public override ValueTask<string> Solve_2()
        {
            long ans = 0;
            foreach (var claw in claws)
            {
                ans += claw.P2(10000000000000);
            }
            return new($"{ans}");
        }
    }
}

/* A button costs 3
 * B button costs 1
 */
//  INPUT
/*  Button A: X+94, Y+34
    Button B: X+22, Y+67
    Prize: X=8400, Y=5400

    Button A: X+26, Y+66
    Button B: X+67, Y+21
    Prize: X=12748, Y=12176

    ...
 */
