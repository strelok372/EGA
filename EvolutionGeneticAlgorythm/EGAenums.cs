using System;
using System.Collections.Generic;
using System.Text;

namespace Algorythms.EvolutionGeneticAlgorythm
{
    public enum Pairs
    {
        Random,
        Quality,
        Countity
    }

    public enum Crossover
    {
        PartialDisplay,
        Order
    }

    public enum Encoding
    {
        ClosestTown,
        ClosestNeighboor
    }

    public enum Breeding
    {
        Roulette,
        Tournament,
        Ranked
    }

    public enum Mutation
    {
        Point,
        Saltation,
        Inversion,
        Translocation
    }

    public enum TaskEncoding
    {
        Bagpack = 0,
        Commivojager = 1,
        Optimisation = 2,
        Approximation = 3
    }
}
