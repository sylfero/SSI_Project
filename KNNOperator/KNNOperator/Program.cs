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
            byte[][] testImagesByte = MnistFiles.ReadImages(directory + "t10k-images.idx3-ubyte");
            double[][] testImagesDouble = testImagesByte.Normalize();

            int[] testLabels = MnistFiles.ReadFullLables(directory + "t10k-labels.idx1-ubyte");

            double acc = 0;

            int k = 10;
            int part = 10;

            for (int i = 0; i < part; i++)
            {
                int output = Knn.ClassifyAug(testImagesDouble[i], 10, k);

                if (output == testLabels[i])
                {
                    acc++;
                }
            }
            acc /= part;
            Console.WriteLine(acc);
            using(StreamWriter writer = new StreamWriter(directory + "Knns\\knn.csv"))
            {
                writer.WriteLine(k);
                writer.WriteLine(acc);
            }
            Console.ReadKey();
        }
    }
}
