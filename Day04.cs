﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCHelper;

namespace AOC_2024
{
    internal class Day04 : BaseDayWithInput
    {
        List<Tuple<int,int>> directions;
        List<List<char>> field;
        public Day04()
        {
            //traverse 8 directions
            /*
             * left right                   +1 0
             * right left                   -1 0
             * top down                     0 +1
             * down top                     0 -1
             * top left -> bottom right     +1 +1
             * bottom left -> top right     +1 -1
             * top right -> bottom left     -1 +1
             * bottom right -> top left     -1 -1
             */
            directions = new List<Tuple<int, int>>()
            {
               new (1, 0),
               new  ( -1, 0 ),
               new  (0, 1 ),
               new  ( 0, -1 ),
               new  ( 1, 1 ),
               new  (1, -1 ),
               new  (-1, 1 ),
               new  (-1, -1 )
             };
            //ii=row
            //jj = col
            field = new();
            for(int i=0;i< _input.Length; i++)
            {
                field.Add(new List<char>());
                foreach (var c in _input[i])
                {
                    field[i].Add(c);
                }
            }
        }

        private bool IsLetter(List<List<char>> map, int i, int j, char c)
        {
            if (i < 0 || i >= field.Count || j < 0 || j >= field[i].Count)
                return false;
            return field[i][j] == c;
        }
        public override ValueTask<string> Solve_1()
        {
            long ans = 0;
            string find = "XMAS";
            for (int i=0;i< field.Count; i++)
            {
                for (int j = 0; j < field[i].Count; j++)
                {
                    //EVERY START POINT HERE
                    foreach (var dir in directions)
                    {
                        int ii = i;
                        int jj = j;
                        bool found = true;
                        foreach (var c in find)
                        {
                            if (!IsLetter(field, ii, jj, c))
                            {
                                found = false;
                                break;
                            }
                            else
                            {
                                ii+=dir.Item1;
                                jj += dir.Item2;
                            }
                        }
                        if (found)
                        {
                            ans++;
                        }
                    }
                }
            }
            return new($"{ans}");
        }

        public override ValueTask<string> Solve_2()
        {
            long ans = 0;
            //find "A" -- then M one dir S other, both diagonals
            for (int i = 0; i < field.Count; i++)
            {
                for (int j = 0; j < field[i].Count; j++)
                {
                    //EVERY START POINT HERE
                    if (IsLetter(field, i, j, 'A'))
                    {
                        bool oneDiag = false;//++ --
                        if (IsLetter(field, i+1, j+1, 'M'))
                        {
                            if(IsLetter(field, i - 1, j - 1, 'S'))
                            {
                                oneDiag = true;
                            }
                        }
                        else if (IsLetter(field, i + 1, j + 1, 'S'))
                        {
                            if(IsLetter(field, i - 1, j - 1, 'M'))
                            {
                                oneDiag = true;
                            }
                        }
                        bool otherDiag = false;// +- -+
                        if (IsLetter(field, i + 1, j - 1, 'M'))
                        {
                            if (IsLetter(field, i - 1, j + 1, 'S'))
                            {
                                otherDiag = true;
                            }
                        }
                        else if (IsLetter(field, i + 1, j - 1, 'S'))
                        {
                            if (IsLetter(field, i - 1, j + 1, 'M'))
                            {
                                otherDiag = true;
                            }
                        }
                        if (oneDiag && otherDiag)
                        {
                            ans++;
                        }
                    }
                }
            }
            return new($"{ans}");
        }
    }
}
