using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCHelper;

namespace AOC_2024
{
    internal class Day09 : BaseDayWithInput
    {
        List<int> uncompressed;
        public Day09()
        {
            string compressed = _input[0];
            uncompressed = [];
            bool isData = true;//data or free space
            for (int idFile = 0; idFile < compressed.Length; idFile++)
            {
                int occurance = compressed[idFile] - '0';
                for (int i = 0; i < occurance; i++)
                {
                    uncompressed.Add(isData ? idFile / 2 : -1);
                }
                isData = !isData;
            }
        }
        public override ValueTask<string> Solve_1()
        {
            long ans = 0;
            //Console.WriteLine(string.Join(',',uncompressed));
            int pLeft = 0;
            int pRight = uncompressed.Count - 1;

            while (pLeft < pRight)
            {
                if (uncompressed[pLeft] == -1)
                {
                    while (uncompressed[pRight] == -1)
                        pRight--;
                    if (pRight <= pLeft)
                    {
                        throw new Exception("Seek back went past seek right - oopsie popsie somewhere");
                    }
                    int a = pLeft * uncompressed[pRight];
                    //Console.WriteLine($"{pLeft}*{uncompressed[pRight]}={a}");
                    ans += a;
                    pRight--;
                }
                else
                {
                    int a = pLeft * uncompressed[pLeft];
                    //Console.WriteLine($"{pLeft}*{uncompressed[pLeft]}={a}");
                    ans += a;
                }
                pLeft++;
            }
            int aLast = pLeft * uncompressed[pLeft];
            //Console.WriteLine($"{pLeft}*{uncompressed[pLeft]}={aLast}");
            ans += aLast;

            return new($"{ans}");
        }

        class memBlock
        {
            public int start;
            public int end;
            public int? value;
            public memBlock(int s, int e, int? v)
            {
                start = s;
                end = e;
                value = v;
            }
            public memBlock(memBlock o)
            {
                this.start = o.start;
                this.end = o.end;
                this.value = o.value;
            }

            public int Size() => end - start + 1;
            public string ToString()
            {
                return $"{start}::{end} ({this.Size()}) = {value}";
            }
        }
        public override ValueTask<string> Solve_2()
        {
            long ans = 0;
            List<memBlock> memBlocks = [];

            int i = 0;
            while (i < uncompressed.Count())
            {
                int start = i;
                int? val = uncompressed[i];
                while (i < uncompressed.Count() && val == uncompressed[i])
                    i++;
                if (val == -1)
                    val = null;

                memBlocks.Add(new(start, i - 1, val));
            }


            HashSet<int> toDefrag = [];
            for(int ii = memBlocks.Count() - 1; ii >= 0; ii--)
            {
                if (memBlocks[ii].value != null)
                    toDefrag.Add(memBlocks[ii].value.Value);
            }

            foreach (int val in toDefrag)
            {
                defragBlockWithVal(ref memBlocks, val);
            }

            int idx = 0;
            foreach (var mb in memBlocks)
            {
                for(int j =0;j<mb.Size(); j++)
                {
                    if (mb.value != null)
                        ans += idx*mb.value.Value;
                    idx++;
                }
            }

            return new($"{ans}");
        }

        static int seekBackFrom = -1;
        static int seekForwardFrom = 0;
        private static void defragBlockWithVal(ref List<memBlock> memBlocks,int val)
        {
            if(seekBackFrom == -1)
                seekBackFrom = memBlocks.Count() - 1;
            for (int i = seekBackFrom; i >= 0; i--)
            {
                if(memBlocks[i].value == val)
                {
                    seekBackFrom = i;
                    //found the defrag block
                    bool updatedFirstEmpty = false;
                    for(int j = seekForwardFrom; j < i; j++)//just go upto i to not defrag after og block
                    {
                        if (memBlocks[j].value == null)
                        {
                            if (!updatedFirstEmpty)
                            {
                                seekForwardFrom = j;
                                updatedFirstEmpty = true;
                            }
                            if (memBlocks[j].Size() >= memBlocks[i].Size())
                            {
                                memBlock moving = new(memBlocks[i]);
                                memBlocks[j].end -= moving.Size();
                                //memBlocks.RemoveAt(i);
                                memBlocks[i].value = null;
                                memBlocks.Insert(j, moving);
                                return;
                            }
                        }
                    }
                    return;
                }
            }
        }
    }
}
