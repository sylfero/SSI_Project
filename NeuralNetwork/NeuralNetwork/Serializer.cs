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
            string[] output = new string[network.Layers.Count];

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
            output[output.Length - 1] = network.Accuracy.ToString();

            File.WriteAllLines(path, output.ToList());
        }

        
        public static void Deserialize(this Network network, string path)
        {
            string[] input = File.ReadAllLines(path);

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
        }
    }
}
