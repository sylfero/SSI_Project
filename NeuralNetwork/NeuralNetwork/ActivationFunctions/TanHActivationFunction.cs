using System;

namespace NeuralNetwork.ActivationFunctions
{
    public class TanHActivationFunction : IActivationFunction
    {
        public double Calculate(double input) => (2 / (1 + Math.Exp(-2 * input))) - 1;

        public double Derivative(double input) => 1 - Math.Pow(Calculate(input), 2);
    }
}
