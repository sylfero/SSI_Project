using System;
using Mnist;
using KNN;
using System.IO;

namespace KNNOperator
{
    class Program
    {
        readonly static string directory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName + "\\Files\\";

        static void Main(string[] args)
        {
            byte[][] trainImagesByte = MnistFiles.ReadImages(directory + "train-images.idx3-ubyte");
            double[][] trainImagesDouble = trainImagesByte.Normalize();

            int[] trainLabels = MnistFiles.ReadFullLables(directory + "train-labels.idx1-ubyte");

            byte[][] testImagesByte = MnistFiles.ReadImages(directory + "t10k-images.idx3-ubyte");
            double[][] testImagesDouble = testImagesByte.Normalize();

            int[] testLabels = MnistFiles.ReadFullLables(directory + "t10k-labels.idx1-ubyte");

            double acc = 0;

            for (int i = 0; i < 100; i++)
            {
                int output = Knn.Classify(testImagesDouble[i], trainImagesDouble, trainLabels, 10, 10);

                if (output == testLabels[i])
                {
                    acc++;
                }
            }
            acc /= 100;
            Console.WriteLine(acc);
            Console.ReadKey();
        }
    }
}
