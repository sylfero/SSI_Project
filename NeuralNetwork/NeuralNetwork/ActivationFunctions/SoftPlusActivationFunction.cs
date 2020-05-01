using System;

namespace NeuralNetwork.ActivationFunctions
{
    public class SoftPlusActivationFunction : IActivationFunction
    {
        public double Calculate(double input) => Math.Log(1 + Math.Pow(Math.E, input));

        public double Derivative(double input) => 1 / (1 + Math.Pow(Math.E, -input));
    }
}
