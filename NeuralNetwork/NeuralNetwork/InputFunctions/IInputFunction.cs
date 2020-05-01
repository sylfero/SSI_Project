using System.Collections.Generic;

namespace NeuralNetwork.InputFunctions
{
    public interface IInputFunction
    {
        double Calculate(List<Synapse> inputs);
    }
}
