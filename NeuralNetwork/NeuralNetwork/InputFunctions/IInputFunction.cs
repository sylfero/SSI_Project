using System.Collections.Generic;

namespace NeuralNetwork.InputFunctions
{
    internal interface IInputFunction
    {
        double Calculate(List<Synapse> inputs);
    }
}
