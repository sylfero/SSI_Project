using NeuralNetwork;
using NeuralNetwork.ActivationFunctions;
using System;
using System.Linq;

namespace NeuralOperator
{
    static class Operator
    {
        private static Network network;

        public static double Accuracy => network.Accuracy;

        public static void Train(double learningRatio, int epochs, string path, params int[] neuralLayers)
        {
            byte[][] imagesByte = Mnist.ReadImages(path + "train-images.idx3-ubyte");
            double[][] imagesDouble = imagesByte.Normalize();

            byte[][] labelsByte = Mnist.ReadLabels(path + "train-labels.idx1-ubyte");
            double[][] labelsDouble = labelsByte.Normalize();

            network = new Network(learningRatio, new SigmoidActivationFunction(), neuralLayers);
            network.Train(imagesDouble, labelsDouble, epochs);
        }

        public static void Test(string path)
        {
            byte[][] imagesByte = Mnist.ReadImages(path + "t10k-images.idx3-ubyte");
            double[][] imagesDouble = imagesByte.Normalize();

            byte[][] labelsByte = Mnist.ReadLabels(path + "t10k-labels.idx1-ubyte");
            double[][] labelsDouble = labelsByte.Normalize();

            double[][] control = new double[labelsByte.Length][];
            double acc = 0;

            for (int i = 0; i < control.Length; i++)
            {
                control[i] = Enumerable.Repeat(0d, 10).ToArray();
                double[] output = network.Calculate(imagesDouble[i]);
                
                control[i][Array.IndexOf(output, output.Max())] = 1;

                if (Array.IndexOf(control[i], control[i].Max()) == Array.IndexOf(labelsDouble[i], labelsDouble[i].Max()))
                {
                    acc++;
                }
            }

            network.Accuracy = acc / control.Length;
            Console.WriteLine(network.Accuracy);
        }

        public static void Serialize(string path) => network.Serialize(path + "network.csv");
    }
}
