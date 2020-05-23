using System.Threading.Tasks;

namespace MainApp
{
    interface IMethod
    {
        double Accuracy { get; }

        Task<double> Calculate(double[] input);
    }
}
