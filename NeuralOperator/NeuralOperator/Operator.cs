using NeuralNetwork;
using NeuralNetwork.ActivationFunctions;
using System;
using System.Linq;
using Mnist;
using System.Collections.Generic;

namespace NeuralOperator
{
    static class Operator
    {
        private static Network network;

        public static double Accuracy => network.Accuracy;

        public static void Train(double learningRatio, int epochs, string path, params int[] neuralLayers)
        {
            byte[][] imagesByte = MnistFiles.ReadImages(path + "train-images.idx3-ubyte");
            double[][] imagesDouble = imagesByte.Normalize();

            byte[][] labelsByte = MnistFiles.ReadLabels(path + "train-labels.idx1-ubyte");
            double[][] labelsDouble = labelsByte.Normalize();

            network = new Network(learningRatio, new SigmoidActivationFunction(), neuralLayers);
            network.Train(imagesDouble, labelsDouble, epochs);
        }

        public static void TrainAugmentation(double learningRatio, int epochs, string path, params int[] neuralLayers)
        {
            byte[][] images = MnistFiles.ReadImages(path + "train-images.idx3-ubyte");
            byte[][] labels = MnistFiles.ReadLabels(path + "train-labels.idx1-ubyte");
            List<(byte[], byte[])> aug = images.Augmentation(labels);

            byte[][] imagesByte = new byte[aug.Count][];
            byte[][] labelsByte = new byte[aug.Count][];

            for (int i = 0; i < aug.Count; i++)
            {
                imagesByte[i] = aug[i].Item1;
                labelsByte[i] = aug[i].Item2;
            }

            double[][] imagesDouble = imagesByte.Normalize();
            double[][] labelsDouble = labelsByte.Normalize();

            network = new Network(learningRatio, new SigmoidActivationFunction(), neuralLayers);
            network.Train(imagesDouble, labelsDouble, epochs);
        }

        public static void Test(string path)
        {
            byte[][] imagesByte = MnistFiles.ReadImages(path + "t10k-images.idx3-ubyte");
            double[][] imagesDouble = imagesByte.Normalize();

            byte[][] labelsByte = MnistFiles.ReadLabels(path + "t10k-labels.idx1-ubyte");
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

        public static void Serialize(string path) => network.Serialize(path + "Networks\\network.csv");
    }
}
