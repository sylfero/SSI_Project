using System;

namespace NeuralNetwork.ActivationFunctions
{
    public class SigmoidActivationFunction : IActivationFunction
    {
        public double Calculate(double input) => 1 / (1 + Math.Exp(-input));

        public double Derivative(double input) => Calculate(input) * (1 - Calculate(input));
    }
}
