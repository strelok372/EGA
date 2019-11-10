using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algorythms.EvolutionGeneticAlgorythm
{
    public class EGA
    {
        private bool elitary;
        private readonly Random r;
        private double g;

        private List<int> bestPath;
        private double bestResult;

        Action InitialForming;
        Action PairChoose;
        Action Reproduce;

        Mutation mutation;
        Breeding breeding;
        double chance;

        List<int[]> parents;
        List<List<double>> values;
        List<List<int>> currentGeneration;
        List<List<int>> nextGeneration;

        public EGA SetupInitialPopulation(Encoding e)
        {
            switch (e)
            {
                case Encoding.ClosestNeighboor:
                    InitialForming = () => ClosestNeighboor();
                    break;
                case Encoding.ClosestTown:
                    InitialForming = () => ClosestTown();
                    break;
            }
            return this;
        }

        public void PrintBestResult()
        {
            var sb = new StringBuilder();
            bestPath.ForEach(x => sb.Append($"{x}->"));
            sb.Remove(sb.Length - 2, 2);
            Console.WriteLine($"Best result is {bestResult}\nWith path: {sb.ToString()}");
        }

        public EGA SetupParents(Pairs p)
        {
            switch (p)
            {
                case Pairs.Quality:
                    PairChoose = () => ParentsChoiceByQuality();
                    break;
                case Pairs.Countity:
                    break;
                case Pairs.Random:
                    break;
            }

            return this;
        }

        public EGA SetupCrossover(Crossover c)
        {
            switch (c)
            {
                case Crossover.Order:
                    Reproduce = () => OrderCrossover();
                    break;
                case Crossover.PartialDisplay:
                    Reproduce = () => PartialDisplayCrossover();
                    break;
            }

            return this;
        }

        public EGA EnableElitary()
        {
            elitary = true;
            return this;
        }

        public EGA SetupBreeding(Breeding b, double g)
        {
            breeding = b;
            this.g = g;
            return this;
        }

        public EGA SetupMutation(double chance, Mutation m)
        {
            mutation = m;
            this.chance = chance;

            return this;
        }

        public EGA(List<List<double>> v)
        {
            values = v;
            parents = new List<int[]>();
            currentGeneration = new List<List<int>>();
            nextGeneration = new List<List<int>>();
            r = new Random(); // change later
        }

        public void DoAlgo(int N)
        {
            // Генерация кодировок на основании значений
            InitialForming();

            while (N > 0)
            {
                // Выбор родительских пар
                PairChoose();

                // Кроссовер
                Reproduce();

                // Мутация
                Mutate();

                // Формирование нового поколения
                PopulationStrategy();

                // Вывод путей и их значений
                PrintCurrentResults();

                N--;
            }
        }

        private void PrintCurrentResults()
        {
            var res = CalcValue(currentGeneration);
            foreach (var r in res)
            {
                Console.WriteLine($"{res.IndexOf(r)}: {r}");
            }

            Console.WriteLine('\n');
        }

        private void FindTheBest()
        {
            var resultValues = CalcValue(nextGeneration);
            var tCurVal = resultValues.Min();
            var tPrevVals = CalcValue(currentGeneration);
            var tPrevVal = tPrevVals.Min();
            var prevGen = tPrevVal > tCurVal;
            bestResult = prevGen ? tPrevVal : tCurVal;
            bestPath = prevGen ? currentGeneration[tPrevVals.IndexOf(tPrevVal)] : nextGeneration[resultValues.IndexOf(tCurVal)];
        }

        private List<double> CalcValue(List<List<int>> encodings)
        {
            var result = new List<double>();

            foreach (var encoding in encodings)
            {
                double localResult = 0;

                foreach (var town in encoding)
                {
                    var last = encoding.Last();
                    if (town == last)
                    {
                        localResult += values[town][encoding[0]];
                    }
                    else
                    {
                        localResult += values[town][encoding[encoding.IndexOf(town) + 1]];
                    }
                }

                result.Add(localResult);
            }

            return result;
        }

        private void PopulationStrategy()
        {
            int parentG = (int)(currentGeneration.Count * g);
            int newPopG = currentGeneration.Count - parentG;
            var newPopulation = new List<List<int>>();

            var fromParents = SelectFromParents(parentG);
            newPopulation.AddRange(fromParents);

            var resultValues = CalcValue(nextGeneration);
            var tempResult = new List<int>();

            if (elitary)
            {
                FindTheBest();
                newPopG--;
                newPopulation.Add(bestPath);
            }
            else
            {
                // здесь надо выбрать лучшего, или чуть позже
            }

            switch (breeding)
            {
                case Breeding.Roulette:
                    tempResult = RouletteBreeding(resultValues, newPopG);
                    break;
                case Breeding.Tournament:
                    tempResult = TournamentBreeding(resultValues, newPopG);
                    break;
            }

            foreach (var r in tempResult)
            {
                newPopulation.Add(nextGeneration[r]);
            }

            currentGeneration = newPopulation;
            nextGeneration.Clear();
        }

        private List<List<int>> SelectFromParents(int g)
        {
            var tempParents = currentGeneration.GetRange(0, currentGeneration.Count);

            while (tempParents.Count != g)
            {
                tempParents.RemoveAt(r.Next(tempParents.Count));
            }

            return tempParents;
        }

        public List<int> RouletteBreeding(List<double> values, int G)
        {
            var result = new List<int>();

            while (result.Count != G)
            {
                var total = values.Sum();
                var temp = total * r.NextDouble();
                double sum = 0;
                int i = 0;

                while (sum < temp)
                {
                    sum += values[i++];
                }

                result.Add(i - 1);
            }

            return result;
        }

        public List<int> TournamentBreeding(List<double> values, int G)
        {
            int beta = 3;
            var result = new List<int>();

            while (result.Count < G)
            {
                var temp = new HashSet<int>();
                while (temp.Count < beta)
                {
                    temp.Add(r.Next(values.Count));
                }

                double res = 1000;
                int index = -1;
                foreach (var x in temp)
                {
                    if (values[x] < res)
                    {
                        index = x;
                        res = values[x];
                    }
                }

                result.Add(index);
            }

            return result;
        }

        public void Mutate()
        {
            if (nextGeneration.Count < 2)
                return;

            var size = nextGeneration[0].Count;

            foreach (var encoding in nextGeneration)
            {
                var tempEncoding = encoding.GetRange(0, encoding.Count);
                var c = r.NextDouble();
                if (c > chance) continue;
                int f = -1;
                int s = -1;

                // Выбор границ мутации
                switch (mutation)
                {
                    case Mutation.Saltation:
                        f = r.Next(size);
                        s = r.Next(size);
                        while (f == s)
                            s = r.Next(size);
                        break;

                    case Mutation.Point:
                        f = r.Next(size);
                        if (f == 0)
                        {
                            s = 1;
                        }
                        else if (f == size)
                        {
                            s = size - 1;
                        }
                        else
                        {
                            if (r.Next(2) == 1)
                                s = f + 1;
                            else
                                s = f - 1;
                        }

                        break;

                    case Mutation.Inversion:
                        f = r.Next(size);
                        var half = size / 2;
                        var lenght = r.Next(half);
                        if (f < half)
                            s = f + lenght;
                        else
                            s = f - lenght;

                        break;
                }

                // Перестановка в рамках выбранных границ
                switch (mutation)
                {
                    case Mutation.Saltation:
                    case Mutation.Point:
                        int a = tempEncoding[f];
                        tempEncoding[f] = tempEncoding[s];
                        tempEncoding[s] = a;
                        break;

                    case Mutation.Inversion:
                        for (int l = f < s ? f : s, r = f < s ? s : f; l <= r; l++, r--)
                        {
                            var x = tempEncoding[l];
                            tempEncoding[l] = tempEncoding[r];
                            tempEncoding[r] = x;
                        }

                        break;
                }
            }
        }

        // not yet implemented
        public void ParentsChoiceByValue(List<List<int>> encodings)
        {
        }

        // not yet implemented
        public void ParentsChoiceByRandom(List<List<int>> encodings)
        {
        }

        public void ParentsChoiceByQuality()
        {
            parents.Clear();
            var remainingEncodings = currentGeneration.GetRange(0, currentGeneration.Count);

            while (remainingEncodings.Count > 1)
            {
                var currentEncoding = remainingEncodings[0];
                int pairingCandidate = -1;
                int candidateScore = -1;

                foreach (var encoding in remainingEncodings)
                {
                        int similarity = 0;

                        foreach (var code in encoding)
                        {
                            if (code == currentEncoding[encoding.IndexOf(code)])
                                similarity++;
                        }

                        if (similarity > candidateScore)
                        {
                            pairingCandidate = currentGeneration.IndexOf(encoding);
                            candidateScore = similarity;
                        }
                }

                parents.Add(new int[] { currentGeneration.IndexOf(currentEncoding), pairingCandidate });
                remainingEncodings.Remove(currentEncoding);
                remainingEncodings.Remove(currentGeneration[pairingCandidate]);
            }

            if (remainingEncodings.Count == 1)
            {
                parents.Add(new int[] { currentGeneration.IndexOf(remainingEncodings[0]), r.Next(currentGeneration.Count) });
            }
        }

        // not yet implemented, need reshuffle encoding 
        public void ReshuffleCrossover(List<List<int>> encodings)
        {
        }

        public void PartialDisplayCrossover()
        {
            foreach (var d in parents)
            {

                int delimiter1 = 5;
                int delimiter2 = 10;
                var aa = currentGeneration[d[0]];
                var bb = currentGeneration[d[1]];
                var aaRange = aa.GetRange(delimiter1, delimiter2);
                var bbRange = bb.GetRange(delimiter1, delimiter2);
                var ab = new List<int>();
                var ba = new List<int>();

                int t;
                var abDict = new Dictionary<int, int>();

                for (int i = 0; i < delimiter1; i++)
                {
                    t = bb[i];
                    while (!aaRange.Contains(t))
                    {
                        t = bbRange[aaRange.IndexOf(t)];
                    }

                    ab.Add(t);
                }

                ab.AddRange(aaRange);

                for (int i = delimiter2; i < aaRange.Count; i++)
                {
                    t = bb[i];
                    while (!aaRange.Contains(t))
                    {
                        t = bbRange[aaRange.IndexOf(t)];
                    }

                    ab.Add(t);
                }

                for (int i = 0; i < delimiter1; i++)
                {
                    t = aa[i];
                    while (!bbRange.Contains(t))
                    {
                        t = aaRange[bbRange.IndexOf(t)];
                    }

                    ba.Add(t);
                }


                ba.AddRange(aaRange);

                for (int i = delimiter2; i < aaRange.Count; i++)
                {
                    t = aa[i];
                    while (!bbRange.Contains(t))
                    {
                        t = aaRange[bbRange.IndexOf(t)];
                    }

                    ba.Add(t);
                }

                nextGeneration.Add(ab);
                nextGeneration.Add(ba);
            }
        }

        public void OrderCrossover()
        {
            foreach (var d in parents)
            {
                var aa = currentGeneration[d[0]];
                var bb = currentGeneration[d[1]];
                int delimiter1 = 5;
                int delimiter2 = 10;
                int afterDelimiter2 = aa.Count - delimiter2;
                var ab = new List<int>();
                var ba = new List<int>();
                var aaRange = aa.GetRange(delimiter1, delimiter2 - delimiter1);
                var bbRange = bb.GetRange(delimiter1, delimiter2 - delimiter1);

                var tempAB = new List<int>();
                tempAB.AddRange(aa.GetRange(delimiter2, afterDelimiter2));
                tempAB.AddRange(aa.GetRange(0, delimiter2));
                tempAB.RemoveAll(x => aaRange.Contains(x));

                ab.AddRange(tempAB.GetRange(0, delimiter1));
                ab.AddRange(aaRange);
                ab.AddRange(tempAB.GetRange(delimiter1, tempAB.Count - delimiter1));

                var tempBA = new List<int>();
                tempBA.AddRange(bb.GetRange(delimiter2, afterDelimiter2));
                tempBA.AddRange(bb.GetRange(0, delimiter2));
                tempBA.RemoveAll(x => bbRange.Contains(x));

                ba.AddRange(tempBA.GetRange(0, delimiter1));
                ba.AddRange(bbRange);
                ba.AddRange(tempBA.GetRange(delimiter1, tempBA.Count - delimiter1));

                nextGeneration.Add(ab);
                nextGeneration.Add(ba);
            }
        }

        public void ClosestTown()
        {
            List<List<int>> result = new List<List<int>>();

            // кол-во итераций == кол-во кодировок
            for (int i = 0; i < values.Count; i++)
            {
                List<int> tempResult = new List<int> { i };
                List<List<double>> tempRoutes = values.GetRange(0, values.Count);
                tempRoutes.RemoveAt(i);

                // генерация кодировки тут
                while (tempRoutes.Count > 0)
                {
                    int tempNode = -1;
                    double tempValue = 1000;
                    foreach (var x in tempResult)
                    {
                        var u = values[x];
                        foreach (var o in u)
                        {
                            int a = u.IndexOf(o);
                            if (o < tempValue && o != 0 && !tempResult.Contains(a))
                            {
                                tempValue = o;
                                tempNode = a;
                            }
                        }
                    }

                    var t = values[tempNode];
                    tempRoutes.Remove(t);
                    tempResult.Add(tempNode);
                }

                result.Add(tempResult);
            }

            currentGeneration = result;
        }

        public void ClosestNeighboor()
        {
            List<List<int>> result = new List<List<int>>();

            for (int n = 0; n < values.Count; n++)
            {
                var start = values[n];
                var p = new List<int>();
                p.Add(n);
                var a = values.GetRange(0, values.Count - 1);

                while (a.Count > 0)
                {
                    a.Remove(start);

                    int index = -1;
                    double value = 1000;

                    for (int i = 0; i < start.Count; i++)
                    {
                        if (start[i] < value && start[i] != 0)
                        {
                            value = start[i];
                            index = i;
                        }
                    }

                    p.Add(index);
                    start = values[index];
                }

                result[n] = p;
            }

            currentGeneration = result;
        }

        // not yet implemented
        private void DiscreteEncoding(TaskEncoding encoding)
        {

            switch (encoding)
            {
                case TaskEncoding.Bagpack:
                    var d = new Dictionary<bool, int>();

                    break;
            }
        }
    }
}
