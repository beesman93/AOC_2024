using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCHelper;

namespace AOC_2024
{
    internal class Day08 : BaseDayWithInput
    {
        record Coord
        {
            public int col;
            public int row;

            public Coord() => (col, row) = (0, 0);
            public Coord(int col, int row)//col, row constructor
                => (this.col, this.row) = (col, row);
            public Coord(Coord c)//copy constructor
                => (col, row) = (c.col, c.row);
            //adding, subtracting
            public static Coord operator +(Coord a, Coord b)
                => new(a.col + b.col, a.row + b.row);
            public static Coord operator -(Coord a, Coord b)
                 => new(a.col - b.col, a.row - b.row);
        }

        Dictionary<char, HashSet<Coord>> nodes;
        Coord mapSize;
        public Day08()
        {
            nodes = [];
            mapSize = new Coord(_input[0].Length, _input.Length);
            for (int row = 0; row < _input.Length; row++)
            {   
                for(int col = 0; col < _input[row].Length; col++)
                {
                    var c = _input[row][col];
                    if (c == '.')
                        continue;
                    if(!nodes.ContainsKey(c))
                        nodes.Add(c, []);
                    nodes[c].Add(new Coord(col, row));
                }
            }
        }
        public override ValueTask<string> Solve_1()
        {
            HashSet<Coord> antinodes = [];
            foreach (var nodeKV in nodes)
            {
                char c = nodeKV.Key;
                HashSet<Coord> coords = nodeKV.Value;
                for (int i = 0; i < coords.Count; i++)
                {
                    for (int j = i + 1; j < coords.Count; j++)
                    {
                        Coord distance = coords.ElementAt(j) - coords.ElementAt(i);
                        Coord firstAntinode = coords.ElementAt(i) - distance;
                        Coord secondAntinode = coords.ElementAt(j) + distance;
                        if (firstAntinode.col >= 0 && firstAntinode.col < mapSize.col
                        && firstAntinode.row >= 0 && firstAntinode.row < mapSize.row)
                            antinodes.Add(firstAntinode);
                        if (secondAntinode.col >= 0 && secondAntinode.col < mapSize.col
                        && secondAntinode.row >= 0 && secondAntinode.row < mapSize.row)
                            antinodes.Add(secondAntinode);
                    }
                }
            }
            return new($"{antinodes.Count}");
        }

        public override ValueTask<string> Solve_2()
        {
            HashSet<Coord> antinodes = [];
            foreach (var nodeKV in nodes)
            {
                char c = nodeKV.Key;
                HashSet<Coord> coords = nodeKV.Value;
                for (int i = 0; i < coords.Count; i++)
                {
                    for (int j = i + 1; j < coords.Count; j++)
                    {
                        Coord distance = coords.ElementAt(j) - coords.ElementAt(i);
                        Coord firstAntinode = coords.ElementAt(i);
                        Coord secondAntinode = coords.ElementAt(j);
                        antinodes.Add(firstAntinode);
                        antinodes.Add(secondAntinode);
                        Coord move = new(firstAntinode);
                        while (move.col >= 0 && move.col < mapSize.col
                            && move.row >= 0 && move.row < mapSize.row)
                        {
                            antinodes.Add(new(move));
                            move -= distance;
                        }
                        move = new(secondAntinode);
                        while (move.col >= 0 && move.col < mapSize.col
                            && move.row >= 0 && move.row < mapSize.row)
                        {
                            antinodes.Add(new(move));
                            move += distance;
                        }
                    }
                }
            }
            return new($"{antinodes.Count}");
        }
    }
}
