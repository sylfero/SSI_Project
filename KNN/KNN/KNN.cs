using System;
using System.Linq;

namespace KNN
{
    public static class Knn
    {
        public static int Classify(double[] input, double[][] trainData, int[] expected, int classes, int k)
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
            int result = Vote(info, expected, classes, k);

            return result;
        }

        private static double Distance(double[] input, double[] data)
        {
            double sum = 0;

            for (int i = 0; i < input.Length; i++)
                sum += Math.Abs(input[i] - data[i]);

            return sum;
        }

        private static int Vote(IndexAndDistance[] info, int[] expected, int classes, int k)
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
