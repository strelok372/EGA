using Algorythms.RandomAlgorythms;
using System;

namespace Algorythms
{
    class Program
    {
        static void Main(string[] args)
        {
            //MonteCarlo mc = new MonteCarlo(15, short.MaxValue, 100, LandscapeFilling.Random);
            //var result = mc.DoAlgo();

            HillClimbing hc = new HillClimbing(5, 32, LandscapeFilling.ValueOfBinary, HillClimbingType.Width);
            hc.DoAlgo();

            //            var values = new List<List<double>>
            //            {
            //new List<double>{0.00, 10.44, 11.18, 4.24, 13.60, 7.00, 12.21, 13.45, 13.45, 9.49, 11.05, 3.16, 1.41, 7.21, 10.20},
            //new List<double>{10.44, 0.00, 2.00, 13.00, 8.25, 4.24, 4.00, 7.07, 6.00, 14.32, 13.60, 8.06, 11.18, 14.32, 13.89},
            //new List<double>{11.18, 2.00, 0.00, 13.15, 6.32, 5.83, 2.00, 5.10, 4.00, 13.60, 12.53, 9.22, 11.70, 14.04, 13.00},
            //new List<double>{4.24, 13.00, 13.15, 0.00, 13.60, 10.44, 13.60, 13.89, 14.32, 6.00, 8.25, 7.21, 2.83, 3.16, 7.07},
            //new List<double>{13.60, 8.25, 6.32, 13.60, 0.00, 11.05, 4.47, 1.41, 2.83, 11.18, 9.00, 13.00, 13.45, 13.00, 10.05},
            //new List<double>{7.00, 4.24, 5.83, 10.44, 11.05, 0.00, 7.62, 10.20, 9.49, 13.45, 13.60, 4.12, 8.06, 12.53, 13.45},
            //new List<double>{12.21, 4.00, 2.00, 13.60, 4.47, 7.62, 0.00, 3.16, 2.00, 13.15, 11.70, 10.63, 12.53, 14.04, 12.37},
            //new List<double>{13.45, 7.07, 5.10, 13.89, 1.41, 10.20, 3.16, 0.00, 1.41, 12.04, 10.05, 12.53, 13.45, 13.60, 11.00},
            //new List<double>{13.45, 6.00, 4.00, 14.32, 2.83, 9.49, 2.00, 1.41, 0.00, 13.00, 11.18, 12.21, 13.60, 14.32, 12.04},
            //new List<double>{9.49, 14.32, 13.60, 6.00, 11.18, 13.45, 13.15, 12.04, 13.00, 0.00, 2.83, 11.66, 8.25, 3.16, 1.41},
            //new List<double>{11.05, 13.60, 12.53, 8.25, 9.00, 13.60, 11.70, 10.05, 11.18, 2.83, 0.00, 12.65, 10.00, 5.83, 1.41},
            //new List<double>{3.16, 8.06, 9.22, 7.21, 13.00, 4.12, 10.63, 12.53, 12.21, 11.66, 12.65, 0.00, 4.47, 9.90, 12.08},
            //new List<double>{1.41, 11.18, 11.70, 2.83, 13.45, 8.06, 12.53, 13.45, 13.60, 8.25, 10.00, 4.47, 0.00, 5.83, 9.06},
            //new List<double>{7.21, 14.32, 14.04, 3.16, 13.00, 12.53, 14.04, 13.60, 14.32, 3.16, 5.83, 9.90, 5.83, 0.00, 4.47},
            //new List<double>{10.20, 13.89, 13.00, 7.07, 10.05, 13.45, 12.37, 11.00, 12.04, 1.41, 1.41, 12.08, 9.06, 4.47, 0.00},
            //            };

            //            EGA ega = new EGA(values);

            //            ega.SetupInitialPopulation(Encoding.ClosestTown)
            //                .SetupParents(Pairs.Quality)
            //                .SetupCrossover(Crossover.Order)
            //                .SetupMutation(0.1, Mutation.Inversion)
            //                .SetupBreeding(Breeding.Roulette, 0.15)
            //                .EnableElitary()
            //                .DoAlgo(10);

            //            ega.PrintBestResult();
        }
    }
}
