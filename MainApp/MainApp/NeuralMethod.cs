using NeuralNetwork;
using NeuralNetwork.ActivationFunctions;
using System;
using System.Linq;

namespace MainApp
{
    class NeuralMethod : IMethod
    {
        private Network network;

        public double Accuracy { get; }

        public NeuralMethod(string path)
        {
            network = new Network(0.1, new SigmoidActivationFunction(), 28 * 28, 32, 10);
            network.Deserialize(path);
            Accuracy = network.Accuracy * 100;
        }

        public double Calculate(double[] input)
        {
            double[] output = network.Calculate(input);
            return Array.IndexOf(output, output.Max());
        }

        public override string ToString()
        {
            return "NeuralNetwork " + Math.Round(Accuracy);
        }
    }
}
