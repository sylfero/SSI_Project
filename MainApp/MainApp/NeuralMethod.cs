using NeuralNetwork;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MainApp
{
    class NeuralMethod : IMethod
    {
        private Network network;

        public double Accuracy { get; }

        public NeuralMethod(string path)
        {
            network = Serializer.Deserialize(path);
            Accuracy = network.Accuracy * 100;
        }

        public async Task<double> Calculate(double[] input)
        {
            double[] output = null;
            await Task.Run(() => output = network.Calculate(input));
            return Array.IndexOf(output, output.Max()); ;
        }

        public override string ToString()
        {
            return "NeuralNetwork " + Math.Round(Accuracy);
        }
    }
}
