using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCHelper;

namespace AOC_2024
{
    internal class Day12 : BaseDayWithInput
    {
        internal class Region
        {
            public char c;
            private HashSet<(int x, int y)> _plots = [];
            private Dictionary<(int x, int y), HashSet<(int x, int y)>> _fence = [];
            private int _perimeter = 0;
            private int _sides = 0;
            public Region(char c)
            {
                this.c = c;
                foreach (var dir in dirrections)
                    _fence.Add(dir, []);
            }
            public static (int x, int y)[] dirrections = new (int x, int y)[] { (0, 1), (0, -1), (1, 0), (-1, 0) };
            public void AddPlot(int x, int y) => _plots.Add((x, y));
            public int Area => _plots.Count;
            public int Perimeter => _perimeter;
            public int Sides => _sides;
            public override string ToString() => $"{c} : {Area} x {Perimeter}";
            public void buildFence()
            {
                foreach (var (x, y) in _plots)
                {
                    foreach (var dir in dirrections)
                    {
                        if (!_plots.Contains((x + dir.x, y + dir.y)))
                        {
                            _fence[dir].Add((x + dir.x, y + dir.y));
                            _perimeter++;
                        }
                    }
                }
            }
            public void countSides()
            {
                foreach(var dir in dirrections)
                {
                    HashSet<(int x, int y)> alreadyChecked = [];
                    foreach (var fencePiece in _fence[dir])
                    {
                        if (alreadyChecked.Contains(fencePiece))
                            continue;
                        alreadyChecked.Add(fencePiece);
                        _sides++;
                        //if dir of fence build is up or down check left and right
                        //if dir of fence build is left or right check up and down
                        int offset;
                        if (dir.x == 0)
                        {
                            offset = 1;
                            while (_fence[dir].Contains((fencePiece.x - offset, fencePiece.y)))
                            {
                                alreadyChecked.Add((fencePiece.x - offset, fencePiece.y));
                                offset++;
                            }
                            offset = 1;
                            while (_fence[dir].Contains((fencePiece.x + offset, fencePiece.y)))
                            {
                                alreadyChecked.Add((fencePiece.x + offset, fencePiece.y));
                                offset++;
                            }
                        }
                        if (dir.y == 0)
                        {
                            offset = 1;
                            while (_fence[dir].Contains((fencePiece.x, fencePiece.y - offset)))
                            {
                                alreadyChecked.Add((fencePiece.x, fencePiece.y - offset));
                                offset++;
                            }
                            offset = 1;
                            while (_fence[dir].Contains((fencePiece.x, fencePiece.y + offset)))
                            {
                                alreadyChecked.Add((fencePiece.x, fencePiece.y + offset));
                                offset++;
                            }
                        }
                    }

                }
            }
        }
        private List<Region> regions;
        private Dictionary<(int x, int y),Region> plotOwners;
        public Day12()
        {
            regions = [];
            plotOwners = [];
            for (int x = 0; x < _input.Length; x++)
                for (int y = 0; y < _input[x].Length; y++)
                    tryRegisterLand(x, y, null);
            foreach (var region in regions)
            {
                region.buildFence();
                region.countSides();
            }
        }
        public void tryRegisterLand(int x, int y, Region? region)
        {
            if(x< 0 || y < 0 || x >= _input.Length || y >= _input[x].Length)
                return;
            if (plotOwners.ContainsKey((x, y)))
                return;
            if (region == null)
            {
                region = new Region(_input[x][y]);
                regions.Add(region);
            }
            if (region.c == _input[x][y])
            {
                region.AddPlot(x, y);
                plotOwners[(x, y)] = region;
                foreach (var dir in Region.dirrections)
                {
                    tryRegisterLand(x + dir.x, y + dir.y, region);
                }
            }
        }
        public override ValueTask<string> Solve_1()
        {
            long ans = 0;
            foreach (var region in regions)
            {
                ans += region.Area * region.Perimeter;
            }
            return new($"{ans}");
        }

        public override ValueTask<string> Solve_2()
        {
            long ans = 0;
            foreach (var region in regions)
            {
                ans += region.Area * region.Sides;
            }
            return new($"{ans}");
        }
    }
}
