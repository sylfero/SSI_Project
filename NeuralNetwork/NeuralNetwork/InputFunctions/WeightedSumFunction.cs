using System.Collections.Generic;
using System.Linq;

namespace NeuralNetwork.InputFunctions
{
    internal class WeightedSumFunction : IInputFunction
    {
        public double Calculate(List<Synapse> inputs) => inputs.Select(x => x.Weight * x.Output).Sum();
    }
}
