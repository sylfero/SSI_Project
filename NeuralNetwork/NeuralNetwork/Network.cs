using NeuralNetwork.ActivationFunctions;
using System.Collections.Generic;
using System.Linq;
using NeuralNetwork.InputFunctions;

namespace NeuralNetwork
{
    public class Network
    {
        public double Accuracy { get; set; }
        internal double LearningRate { get; set; }
        internal int[] NumberOfNeurons { get; set; }

        internal List<Layer> Layers { get; set; }

        public Network(double learningRate, IActivationFunction activationFunction, params int[] numberOfNeurons)
        {
            NumberOfNeurons = numberOfNeurons;
            LearningRate = learningRate;
            Layers = new List<Layer>();
            //numberOfNeurons[] - how many layers there should be and how many neurons each layer should have
            foreach (int number in numberOfNeurons)
            {
                Layer layer = new Layer();
                Layers.Add(layer);

                for (int i = 0; i < number; i++)
                {
                    layer.Neurons.Add(new Neuron(activationFunction, new WeightedSumFunction()));
                }
            }

            ConnectLayers();
        }
        
        public void Train(double[][] inputValues, double[][] expectedValues, int epochs)
        {
            for (int i = 0; i < epochs; i++)
            {
                for (int j = 0; j < inputValues.Length; j++)
                {
                    double[] outputs = Calculate(inputValues[j]);

                    CalculateErrors(outputs, expectedValues[j]);

                    UpdateWeights();
                }
            }
        }

        public double[] Calculate(double[] input)
        {
            PushInputValues(input);

            //We gather calculated values from last layer neurons to array
            double[] outputs = new double[Layers.Last().Neurons.Count];
            for (int i = 0; i < outputs.Length; i++)
            {
                Neuron currentNeuron = Layers.Last().Neurons[i];
                outputs[i] = currentNeuron.OutputValue;
            }
            return outputs;
        }

        public void PushInputValues(double[] inputs)
        {
            //Give inputs values to first layer of neurons
            for (int i = 0; i < inputs.Length; i++)
            {
                Neuron currentNeuron = Layers.First().Neurons[i];
                currentNeuron.OutputValue = currentNeuron.InputValue = inputs[i];
            }
            
            //Calculate outputs for other layers
            for (int i = 0; i < Layers.Count; i++)
            {
                //Calculate output for each neuron and give this value to output synapses connected to this neuron
                foreach (Neuron neuron in Layers[i].Neurons)
                {
                    neuron.PushValueOnOuput(neuron.CalculateOutput());
                }
            }
        }

        private void ConnectLayers()
        {
            //We don't connect first layer to anythin befor so we can omit it
            for (int i = 1; i < Layers.Count; i++)
            {
                //Each neuron in given layer
                foreach (Neuron neuron in Layers[i].Neurons)
                {
                    //Is connected with each nuron in previous layer with synapse
                    foreach (Neuron previousNeuron in Layers[i - 1].Neurons)
                    {
                        Synapse synapse = new Synapse(previousNeuron, neuron);
                        neuron.Inputs.Add(synapse);
                        previousNeuron.Outputs.Add(synapse);
                    }
                }
            }
        }

        private void CalculateErrors(double[] outputs, double[] expectedValues)
        {
            for (int i = 0; i < outputs.Length; i++)
            {
                Neuron currentNeuron = Layers.Last().Neurons[i];
                currentNeuron.Gradient = currentNeuron.ActivationFunction.Derivative(currentNeuron.InputValue) * (expectedValues[i] - outputs[i]);
            }

            for (int i = Layers.Count - 2; i > 0; i--)
            {
                for (int j = 0; j < Layers[i].Neurons.Count; j++)
                {
                    double d = 0;
                    for (int k = 0; k < Layers[i + 1].Neurons.Count; k++)
                    {
                        d += Layers[i + 1].Neurons[k].Gradient * Layers[i].Neurons[j].Outputs[k].Weight;
                    }
                    Neuron currentNeuron = Layers[i].Neurons[j];
                    currentNeuron.Gradient = d * currentNeuron.ActivationFunction.Derivative(currentNeuron.InputValue);
                }
            }
        }

        private void UpdateWeights()
        {
            //Update wights for all synapses based on calculated errors
            for (int i = Layers.Count - 1; i > 0; i--)
            {
                for (int j = 0; j < Layers[i].Neurons.Count; j++)
                {
                    for (int k = 0; k < Layers[i - 1].Neurons.Count; k++)
                    {
                        double delta = Layers[i].Neurons[j].Gradient * Layers[i - 1].Neurons[k].OutputValue;
                        Layers[i].Neurons[j].Inputs[k].UpdateWeight(LearningRate, delta);
                    }
                }
            }
        }
    }
}
