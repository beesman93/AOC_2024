using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCHelper;
using MathNet.Numerics.Distributions;

namespace AOC_2024
{
    internal class Day17 : BaseDayWithInput
    {
        internal class Chronospacial
        {
            enum opcode
            {
                ADV = 0,
                BDV = 6,
                CDV = 7,

                BXL = 1,
                BST = 2,
                JNZ = 3,
                BXC = 4,
                OUT = 5
            }
            public enum retcode
            {
                OK = 0,
                HALT = 1,
                OUTPUTS = 2
            }
            public readonly List<int> instructions;
            public List<int> OutTape { get; set; }
            private int INSTR_PTR;
            long REG_A { get; set; }
            long REG_B { get; set; }
            long REG_C { get; set; }

            public Chronospacial(int A, int B, int C, List<int> instructions)
            {
                this.instructions = instructions;
                this.OutTape = [];
                INSTR_PTR = 0;
                REG_A = A;
                REG_B = B;
                REG_C = C;
            }
            public void reset(long A)
            {
                this.REG_A = A;
                this.INSTR_PTR = 0;
                this.OutTape = [];
            }

            long getComboOperand(int operand)
            {
                return operand switch
                {
                    0 or 1 or 2 or 3 => operand,
                    4 => REG_A,
                    5 => REG_B,
                    6 => REG_C,
                    7 => throw new ArgumentOutOfRangeException("reserved operand."),
                    _ => throw new ArgumentOutOfRangeException("out of scope operand.")
                };
            }
            public retcode tick()
            {
                if (INSTR_PTR >= instructions.Count)
                    return retcode.HALT;
                opcode OPCODE = (opcode)instructions[INSTR_PTR];
                int LITERAL_OPERAND = instructions[INSTR_PTR + 1];

                switch(OPCODE)
                {
                    case opcode.ADV: REG_A = DIV_POW2(REG_A, getComboOperand(LITERAL_OPERAND)); break;
                    case opcode.BDV: REG_B = DIV_POW2(REG_A, getComboOperand(LITERAL_OPERAND)); break;
                    case opcode.CDV: REG_C = DIV_POW2(REG_A, getComboOperand(LITERAL_OPERAND)); break;
                    case opcode.BXL: REG_B ^= LITERAL_OPERAND; break;
                    case opcode.BST: REG_B = getComboOperand(LITERAL_OPERAND) & 0b111; break;
                    case opcode.JNZ: if (REG_A != 0) INSTR_PTR = LITERAL_OPERAND-2; break;
                    case opcode.BXC: REG_B ^= REG_C; break;
                    case opcode.OUT: OutTape.Add((int)getComboOperand(LITERAL_OPERAND)&0b111); INSTR_PTR += 2; return retcode.OUTPUTS;
                }
                INSTR_PTR += 2;
                return retcode.OK;
            }
            public override string ToString()
            {
                return $"{INSTR_PTR} : {REG_A} {REG_B} {REG_C}";
            }

            static long DIV_POW2(long N, long D)
            {
                if(D<int.MaxValue)
                    return N >>(int)D;
                return N / (long)Math.Pow(D, 2);
            }
        }
        Chronospacial chronospacial;
        public Day17()
        {
            int A = Convert.ToInt32(_input[0].Split(":")[1].Trim());
            int B = Convert.ToInt32(_input[1].Split(":")[1].Trim());
            int C = Convert.ToInt32(_input[2].Split(":")[1].Trim());
            List<int> instructions = _input[4].Split(":")[1].Trim().Split(",").Select(x => Convert.ToInt32(x)).ToList();
            chronospacial = new Chronospacial(A, B, C, instructions);
        }
        public override ValueTask<string> Solve_1()
        {
            while (chronospacial.tick() != Chronospacial.retcode.HALT)
            {
                //Console.WriteLine(chronospacial);
            }
            return new($"{string.Join(',',chronospacial.OutTape)}");
        }
        public override ValueTask<string> Solve_2()
        {
            bool PRINT_HINTS = false;
            ulong knownLastDigits = 0b0;
            ulong incrementBy = 0b1;
            int wantedOutTapeCount = 4;
            int sampleSize = 5;
            while (true)
            {
                if (PRINT_HINTS) Console.WriteLine($"-----wantedOutTapeCount = {wantedOutTapeCount}");
                HashSet<ulong> answersForCurrentOutTapeCount = new();
                for (ulong ans = knownLastDigits; ans < long.MaxValue; ans += incrementBy)
                {
                    chronospacial.reset((long)ans);
                    Chronospacial.retcode ret = Chronospacial.retcode.OK;
                    while (ret != Chronospacial.retcode.HALT)
                    {
                        ret = chronospacial.tick();
                        if (ret == Chronospacial.retcode.OUTPUTS)
                        {
                            if (chronospacial.OutTape.Count > chronospacial.instructions.Count)
                                break;
                            if (chronospacial.instructions[chronospacial.OutTape.Count - 1] != chronospacial.OutTape.Last())
                                break;
                            if (chronospacial.OutTape.Count == wantedOutTapeCount)
                            {
                                if (PRINT_HINTS) Console.WriteLine($"0b{Convert.ToString((long)ans, 2)}");
                                answersForCurrentOutTapeCount.Add(ans);
                            }
                            if (chronospacial.OutTape.Count == chronospacial.instructions.Count)
                                return new($"{ans}");
                        }
                    }
                    if (answersForCurrentOutTapeCount.Count >= sampleSize)
                    {
                        knownLastDigits = answersForCurrentOutTapeCount.First();
                        foreach (var item in answersForCurrentOutTapeCount)
                        {
                            ulong xnor = (ulong)~(knownLastDigits ^ item);
                            ulong mask = 0;
                            while (xnor%2==1)
                            {
                                mask = mask << 1;
                                mask += 1;
                                xnor >>= 1;
                            }
                            knownLastDigits &= mask;
                        }
                        var incByShifts = Convert.ToString((long)knownLastDigits, 2).Length;
                        incrementBy = 0b1;
                        for(int i = 0; i < incByShifts; i++)
                        {
                            incrementBy = incrementBy << 1;
                        }
                        if (PRINT_HINTS) Console.WriteLine($"===new knownLastDigits = \t  0b{Convert.ToString((long)knownLastDigits, 2)}");
                        if (PRINT_HINTS) Console.WriteLine($"===new incrementBy     = \t 0b{Convert.ToString((long)incrementBy, 2)}");
                        break;
                    }
                }
                wantedOutTapeCount+=2;
            }
            throw new Exception("Didn't find ans.");
        }
    }
}
