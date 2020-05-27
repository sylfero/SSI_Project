using NeuralNetwork.ActivationFunctions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NeuralNetwork
{
    public static class Serializer
    {
        public static void Serialize(this Network network, string path)
        {
            //Input layer inputs don't need weights
            string[] output = new string[network.Layers.Count + 1];

            for (int i = network.Layers.Count - 1; i > 0; i--)
            {
                StringBuilder builder = new StringBuilder();

                for (int j = 0; j < network.Layers[i].Neurons.Count; j++)
                {
                    for (int k = 0; k < network.Layers[i].Neurons[j].Inputs.Count; k++)
                    {
                        builder.Append(network.Layers[i].Neurons[j].Inputs[k].Weight + ",");
                    }

                    builder.Length--;
                    builder.Append("|");
                }

                builder.Length--;
                output[i - 1] = builder.ToString();
            }
            StringBuilder tmp = new StringBuilder();
            foreach (int i in network.NumberOfNeurons)
            {
                tmp.Append(i + " ");
            }
            tmp.Length--;
            output[output.Length - 2] = tmp.ToString();
            output[output.Length - 1] = network.Accuracy.ToString();

            File.WriteAllLines(path, output.ToList());
        }

        
        public static Network Deserialize(string path)
        {
            string[] input = File.ReadAllLines(path);
            string[] tmp = input[input.Length - 2].Split(' ');
            int[] tmp2 = new int[tmp.Length];
            for (int i =0; i< tmp.Length; i++)
            {
                tmp2[i] = int.Parse(tmp[i]);
            }
            Network network = new Network(0.1, new SigmoidActivationFunction(), tmp2);

            for (int i = network.Layers.Count - 1; i > 0; i--)
            {
                string[] neurons = input[i - 1].Split('|');

                for (int j = 0; j < network.Layers[i].Neurons.Count; j++)
                {
                    string[] synapses = neurons[j].Split(',');
                    for (int k = 0; k < network.Layers[i].Neurons[j].Inputs.Count; k++)
                    {
                        network.Layers[i].Neurons[j].Inputs[k].Weight = double.Parse(synapses[k]);
                    }
                }
            }

            network.Accuracy = double.Parse(input.Last());
            return network;
        }
    }
}
