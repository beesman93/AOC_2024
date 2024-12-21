using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCHelper;

namespace AOC_2024
{
    internal class Day21 : BaseDayWithInput
    {
        abstract class Keypad
        {
            public Keypad? next { get; set; }
            private Dictionary<(char start, char endWithPress), long> cache;
            public Keypad()
            {
                cache = [];
            }
            public virtual long CostOfString(string s)
            {
                long totalCost = 0;
                char prevC = 'A';//initial state of pads which they also return to after pressing button keypad further
                foreach (var c in s)
                {
                    totalCost += Cost(prevC, c);
                    prevC = c;
                }
                return totalCost;
            }
            protected virtual long Cost(char start, char endWithPress)
            {
                if(cache.ContainsKey((start, endWithPress)))
                    return cache[(start, endWithPress)];
                cache[(start, endWithPress)] = CalculateCost(start, endWithPress);
                return cache[(start, endWithPress)];
            }
            protected virtual long CalculateCost(char start, char endWithPress)
            {
                long minCost = long.MaxValue;
                foreach(var path in GetPaths(start, endWithPress))
                {
                    minCost = long.Min(minCost,next!=null?next.CostOfString(path):path.Length);
                }
                return minCost;
            }
            protected abstract List<string> GetPaths(char start, char endWithPress);
        }

        class NumPad : Keypad
        {
            private static (int row, int col) GetPos(char c) => c switch
            {
                /*
                { '7', '8', '9' },
                { '4', '5', '6' },
                { '1', '2', '3' },
                { '#', '0', 'A' }
                */
                '7' => (0, 0),
                '8' => (0, 1),
                '9' => (0, 2),
                '4' => (1, 0),
                '5' => (1, 1),
                '6' => (1, 2),
                '1' => (2, 0),
                '2' => (2, 1),
                '3' => (2, 2),
                '0' => (3, 1),
                'A' => (3, 2),
                _ => throw new ArgumentException()
            };
            protected override List<string> GetPaths(char start, char endWithPress)
            {
                var startPos = GetPos(start);
                var endPos = GetPos(endWithPress);
                int offsetCol = endPos.col - startPos.col;
                int offsetRow = endPos.row - startPos.row;
                string colPath = offsetCol switch
                {
                    0 => "",
                    > 0 => new string('>', offsetCol),
                    < 0 => new string('<', -offsetCol)
                };
                string rowPath = offsetRow switch
                {
                    0 => "",
                    > 0 => new string('v', offsetRow),
                    < 0 => new string('^', -offsetRow)
                };
                string colRowPath = colPath + rowPath + 'A';
                string rowColPath = rowPath + colPath + 'A';
                if (startPos.row == 3 && endPos.col == 0)
                    return new List<string> { rowColPath };
                if (startPos.col == 0 && endPos.row == 3)
                    return new List<string> { colRowPath };
                if (colRowPath == rowColPath)
                    return new List<string> { colRowPath };
                return new List<string> {colRowPath,rowColPath};
            }
        }
        class DirPad : Keypad
        {
            private static (int row, int col) GetPos(char c) => c switch
            {
                /*
                { '#', '^', 'A' },
                { '<', 'v', '>' }
                */
                '^' => (0, 1),
                'A' => (0, 2),
                '<' => (1, 0),
                'v' => (1, 1),
                '>' => (1, 2),
                _ => throw new ArgumentException()
            };
            protected override List<string> GetPaths(char start, char endWithPress)
            {
                var startPos = GetPos(start);
                var endPos = GetPos(endWithPress);
                int offsetCol = endPos.col - startPos.col;
                int offsetRow = endPos.row - startPos.row;
                string colPath = offsetCol switch
                {
                    0 => "",
                    > 0 => new string('>', offsetCol),
                    < 0 => new string('<', -offsetCol)
                };
                string rowPath = offsetRow switch
                {
                    0 => "",
                    > 0 => new string('v', offsetRow),
                    < 0 => new string('^', -offsetRow)
                };
                string colRowPath = colPath + rowPath + 'A';
                string rowColPath = rowPath + colPath + 'A';
                if (start=='<')
                    return new List<string> { colRowPath };
                if (endWithPress=='<')
                    return new List<string> { rowColPath };
                if (colRowPath == rowColPath)
                    return new List<string> { colRowPath };
                return new List<string> { colRowPath, rowColPath };
            }
        }
        public override ValueTask<string> Solve_1() => new($"{SolveDoorWithXNumPads(2)}");
        public override ValueTask<string> Solve_2() => new($"{SolveDoorWithXNumPads(25)}");
        private long SolveDoorWithXNumPads(ushort x)
        {
            NumPad door = new();
            Keypad current = door;
            for (int i = 0; i < x; i++)
            {
                current.next = new DirPad();
                current = current.next;
            }
            long ans = 0;
            foreach (var line in _input) ans += Convert.ToInt32(line[..^1]) * door.CostOfString(line);
            return ans;
        }
    }
}
