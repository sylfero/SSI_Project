using NeuralNetwork.ActivationFunctions;
using NeuralNetwork.InputFunctions;
using System.Collections.Generic;

namespace NeuralNetwork
{
    public class Neuron
    {
        public List<Synapse> Inputs { get; }
        public List<Synapse> Outputs { get; }

        public IActivationFunction ActivationFunction { get; set; }
        public IInputFunction InputFunction { get; set; }

        public double OutputValue { get; set; }
        public double InputValue { get; set; }
        public double Gradient { get; set; }

        public Neuron(IActivationFunction activationFunction, IInputFunction inputFunction)
        {
            Inputs = new List<Synapse>();
            Outputs = new List<Synapse>();
            ActivationFunction = activationFunction;
            InputFunction = inputFunction;
        }

        public double CalculateOutput()
        {
            //For first layer neurons (they don't have any inputs) we don't calculate output
            if (Inputs.Count == 0) return InputValue; 
            InputValue = InputFunction.Calculate(Inputs);
            OutputValue = ActivationFunction.Calculate(InputValue);
            return OutputValue;
        }

        public void PushValueOnOuput(double outputValue)
        {
            //Update values of output synapses
            Outputs.ForEach(output => output.Output = outputValue);
        }
    }
}
