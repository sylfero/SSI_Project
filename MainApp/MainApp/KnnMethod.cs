using KNN;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MainApp
{
    class KnnMethod : IMethod
    {
        private int k;

        public double Accuracy { get; }

        public KnnMethod(string path)
        {
            string[] tmp = File.ReadAllLines(path);
            k = int.Parse(tmp[0]);
            Accuracy = double.Parse(tmp[1]) * 100;
        }

        public async Task<double> Calculate(double[] input)
        {
            double d = 0;
            await Task.Run(() => d = Knn.Classify(input, 10, k));
            return d;
        }

        public override string ToString()
        {
            return "Knn " + Math.Round(Accuracy);
        }
    }
}
