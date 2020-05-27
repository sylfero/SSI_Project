using Mnist;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KNN
{
    public static class Knn
    {
        private readonly static string directory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName + "\\Files\\";

        private readonly static byte[][] trainImagesByte = MnistFiles.ReadImages(directory + "train-images.idx3-ubyte");
        private readonly static double[][] trainData = trainImagesByte.Normalize();

        private readonly static int[] expected = MnistFiles.ReadFullLables(directory + "train-labels.idx1-ubyte");

        public static int Classify(double[] input, int classes, int k)
        {
            IndexAndDistance[] info = new IndexAndDistance[trainData.Length];

            //Calculate distance for each element in trainData array and input
            for (int i = 0; i < trainData.Length; i++)
            {
                IndexAndDistance curr = new IndexAndDistance();
                double dist = Distance(input, trainData[i]);
                curr.idx = i;
                curr.dist = dist;
                info[i] = curr;
            }

            //Sort distances
            Array.Sort(info);

            //Get most possible class of input data
            int result = Vote(info, classes, k, expected);

            return result;
        }

        public static int ClassifyAug(double[] input, int classes, int k)
        {
            List<(byte[], int)> aug = trainImagesByte.AugmentationInt(expected);

            byte[][] images = new byte[aug.Count][];
            int[] labels = new int[aug.Count];

            for (int i = 0; i < aug.Count; i++)
            {
                images[i] = aug[i].Item1;
                labels[i] = aug[i].Item2;
            }

            double[][] imagesDouble = images.Normalize();

            IndexAndDistance[] info = new IndexAndDistance[imagesDouble.Length];

            //Calculate distance for each element in trainData array and input
            for (int i = 0; i < imagesDouble.Length; i++)
            {
                IndexAndDistance curr = new IndexAndDistance();
                double dist = Distance(input, imagesDouble[i]);
                curr.idx = i;
                curr.dist = dist;
                info[i] = curr;
            }

            //Sort distances
            Array.Sort(info);

            //Get most possible class of input data
            int result = Vote(info, classes, k, labels);

            return result;
        }

        private static double Distance(double[] input, double[] data)
        {
            double sum = 0;

            for (int i = 0; i < input.Length; i++)
                sum += Math.Abs(input[i] - data[i]);

            return sum;
        }

        private static int Vote(IndexAndDistance[] info, int classes, int k, int[] expected)
        {
            int[] votes = new int[classes];

            //Count how many times each class occurs in k nearest elements
            for (int i = 0; i < k; i++)
                votes[expected[info[i].idx]]++;

            return Array.IndexOf(votes, votes.Max());
        }

        //We need distance and index of each element cuz we will be sorting elements by distance but still need to locate elements in input array
        private class IndexAndDistance : IComparable
        {
            public int idx;
            public double dist;

            public int CompareTo(object obj)
            {
                if (obj == null) return 1;

                IndexAndDistance other = (IndexAndDistance)obj;

                if (other != null)
                    return dist.CompareTo(other.dist);
                else
                    throw new ArgumentException("Object is not a IndexAndDistance");
            }
        }
    }
}
