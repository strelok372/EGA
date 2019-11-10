using Algorythms.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algorythms.RandomAlgorythms
{
    public class MonteCarlo : RandomBase, IAlgorythm
    {
        private LandscapeFilling landscapeFilling;
        private readonly int N;

        public MonteCarlo(int L, int S, int N, LandscapeFilling l) : base (L, S, l)
        {
            landscapeFilling = l;
            this.N = N;
        }        

        public int DoAlgo()
        {
            for (int i = 0; i < N; i++)
            {
                int tempIndex = GetUniqueCode();
                var tempCode = GetByteView(tempIndex);
                var tempValue = Landscape[tempCode];                

                if (max < tempValue)
                {
                    if (i > 0)
                        PrintInfo(i, tempValue, tempCode);

                    max = tempValue;
                    maxS = tempCode;
                }
            }

            return max;
        }
    }
}
