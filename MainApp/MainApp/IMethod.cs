namespace MainApp
{
    interface IMethod
    {
        double Accuracy { get; }

        double Calculate(double[] input);
    }
}
