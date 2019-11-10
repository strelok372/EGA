using Algorythms.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algorythms.RandomAlgorythms
{
    public class HillClimbing : RandomBase, IAlgorythm
    {
        private int iteration = 0;
        private HillClimbingType type;

        public HillClimbing(int L, int S, LandscapeFilling lf, HillClimbingType type) : base(L, S, lf)
        {
            this.type = type;
        }

        public int DoAlgo()
        {
            var tCode = GetUniqueCode();
            maxS = GetByteView(tCode);
            max = Landscape[maxS];

            switch (type)
            {
                case HillClimbingType.Depth:
                    CheckRandomSurroundings();
                    break;
                case HillClimbingType.Width:
                    CheckAllSurroundings();
                    break;
            }

            return max;
        }

        private void CheckRandomSurroundings()
        {
            string curS = maxS;
            var tList = new List<int>();
            int index = r.Next(5);

            while (tList.Count < 5)
            {
                iteration++;

                while (tList.Contains(index))
                {
                    index = r.Next(5);
                }

                tList.Add(index);

                var tArr = curS.ToCharArray();
                tArr[index] = tArr[index] == '1' ? '0' : '1';
                var tCode = new String(tArr);
                var tVal = Landscape[tCode];

                if (max < tVal)
                {
                    PrintInfo(iteration, tVal, tCode);
                    max = tVal;
                    maxS = tCode;
                    tList.Clear();
                    curS = tCode;
                }
            }

            return;
        }

        private void CheckAllSurroundings()
        {
            while (true)
            {
                string curS = maxS;

                iteration++;

                string bestS = string.Empty;
                int bestV = 0;

                for (int i = 0; i < L; i++)
                {
                    var tArr = curS.ToCharArray();
                    tArr[i] = tArr[i] == '1' ? '0' : '1';
                    var tCode = new string(tArr);
                    var tVal = Landscape[tCode];

                    if (bestV < tVal)
                    {
                        bestV = tVal;
                        bestS = tCode;
                    }
                }

                if (max < bestV)
                {
                    PrintInfo(iteration, bestV, bestS);
                    curS = bestS;
                    maxS = bestS;
                    max = bestV;
                }
                else
                {
                    return;
                }
            }
        }
    }
}
