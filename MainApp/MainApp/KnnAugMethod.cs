using KNN;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MainApp
{
    class KnnAugMethod : IMethod
    {
        private int k;

        public double Accuracy { get; }

        public KnnAugMethod(string path)
        {
            string[] tmp = File.ReadAllLines(path);
            k = int.Parse(tmp[0]);
            Accuracy = double.Parse(tmp[1]) * 100;
        }

        public async Task<double> Calculate(double[] input)
        {
            double d = 0;
            await Task.Run(() => d = Knn.ClassifyAug(input, 10, k));
            return d;
        }

        public override string ToString()
        {
            return "Knn Aug " + Math.Round(Accuracy);
        }
    }
}
