using System;
using System.Collections.Generic;
using System.Text;

namespace Algorythms.RandomAlgorythms
{
    public class RandomBase
    {
        protected readonly int L;
        protected int max = 0;
        protected string maxS;
        protected int S;

        protected Random r;
        protected Dictionary<string, int> Landscape;
        protected HashSet<int> UsedCods;

        private RandomBase()
        {
            Landscape = new Dictionary<string, int>();
            r = new Random();
            UsedCods = new HashSet<int>();
        }

        protected RandomBase(int L, int S, LandscapeFilling lf)
            : this()
        {
            this.L = L;
            this.S = S;
            FillLandscape(lf);
        }

        protected string GetByteView(int a)
        {
            var j = Convert.ToString(a, toBase: 2);
            var b = j.Length;

            if (b < L)
            {
                j = new String('0', L - b) + j;
            }

            return j;
        }        

        protected int GetUniqueCode()
        {
            bool b = true;
            int a = -1;
            while (b)
            {
                a = r.Next(0, S);
                b = !UsedCods.Add(a);
            }

            return a;
        }

        private void FillLandscapeRandom(int h)
        {
            var check = new HashSet<int>();
            bool j;

            j = true;
            int y = 0;
            while (j)
            {
                y = r.Next(0, S);
                j = !check.Add(y);
            }

            Landscape[GetByteView(h)] = y;
        }

        private void FillLandscapeValues(int h)
        {
            Landscape[GetByteView(h)] = h;
        }

        private void FillLandscapeQuadratic(int h)
        {
            var pow = Math.Pow(2, L - 1);
            int b = (int)Math.Pow(h - pow, 2);
            Landscape[GetByteView(h)] = b;
        }

        private void FillLandscape(LandscapeFilling lf)
        {
            Action<int> fill = null;

            switch (lf)
            {
                case LandscapeFilling.QuadraticFunction:
                    fill = FillLandscapeQuadratic;
                    break;
                case LandscapeFilling.ValueOfBinary:
                    fill = FillLandscapeValues;
                    break;
                case LandscapeFilling.Random:
                    fill = FillLandscapeRandom;
                    break;
            }

            if (fill == null) return;

            for (int i = 0; i < S; i++)
            {
                fill(i);
            }
        }

        protected void PrintInfo(int iteration, int choosingVal, string choosingCode)
        {
            Console.WriteLine($"Iteration: {iteration}\n   Current: {maxS} = {max}\n   Compare: {choosingCode} = {choosingVal}\n");
        }

        // может быть потом...
        private int GrayCode(int a)
        {
            var u = a & (a >> 1);
            return u;
        }
    }    
}
