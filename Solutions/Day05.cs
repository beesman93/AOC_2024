using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCHelper;

namespace AOC_2024
{
    internal class Day05 : BaseDayWithInput
    {
        class Page : IComparable<Page>
        {
            public int id;
            HashSet<int> largerNumeros = new HashSet<int>();
            HashSet<int> smallerNumeros = new HashSet<int>();
            public Page(int id)
            {
                this.id = id;
            }
            public void AddLarger(int n)
            {
                largerNumeros.Add(n);
            }
            public void AddSmaller(int n)
            {
                smallerNumeros.Add(n);
            }

            public int CompareTo(Page? other)
            {
                if (other == null)
                    return 1;
                if (largerNumeros.Contains(other.id))
                    return -1;
                if (smallerNumeros.Contains(other.id))
                    return 1;
                return 0;
            }
        }

        Dictionary<int, Page> numeros = [];
        List<List<Page>> lists = [];
        public Day05()
        {
            //read rules first
            int index = 0;
            while(index< _input.Length)
            {
                if (_input[index] == "")
                    break;
                var parts = _input[index].Split("|");
                int left = Convert.ToInt32(parts[0]);
                int right = Convert.ToInt32(parts[1]);

                if (!numeros.ContainsKey(left))
                    numeros[left] = new Page(left);
                if (!numeros.ContainsKey(right))
                    numeros[right] = new Page(right);

                numeros[left].AddLarger(right);
                numeros[right].AddSmaller(left);
                index++;
            }
            index++;
            //read lists
            while (index < _input.Length)
            {
                var parts = _input[index].Split(",");
                lists.Add(new List<Page>());
                foreach (var part in parts)
                {
                    lists.Last().Add(numeros[Convert.ToInt32(part)]);
                }
                index++;
            }
        }
        public override ValueTask<string> Solve_1()
        {
            long ans = 0;
            foreach (var list in lists)
            {
                var lCopy = new List<Page>(list);
                lCopy.Sort(new Comparison<Page>((a, b) => a.CompareTo(b)));
                if (lCopy.SequenceEqual(list))
                    ans += list[list.Count() / 2].id;
            }
            return new($"{ans}");
        }

        public override ValueTask<string> Solve_2()
        {
            long ans = 0;
            foreach (var list in lists)
            {
                var lCopy = new List<Page>(list);
                lCopy.Sort(new Comparison<Page>((a, b) => a.CompareTo(b)));
                if (!lCopy.SequenceEqual(list))
                    ans += lCopy[lCopy.Count() / 2].id;
            }
            return new($"{ans}");
        }
    }
}
