using System;
using System.Collections.Generic;
using System.Text;

namespace Algorythms
{    
    public enum LandscapeFilling
    {
        Random,
        ValueOfBinary,
        QuadraticFunction
    }

    public enum AlgorithmMethod
    {
        MonteCarlo,
        HillClimbingDepth,
        HillClimbingWide,
        EvolutionaryGeneticAlgorithm
    }

    public enum HillClimbingType
    {
        Depth,
        Width
    }
}
