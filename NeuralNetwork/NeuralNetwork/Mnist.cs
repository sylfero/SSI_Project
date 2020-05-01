using System.IO;
using System.Linq;

namespace NeuralNetwork
{
    public static class Mnist
    {
        public static byte[][] ReadImages(string path)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
            {
                long numberOfImages = (reader.BaseStream.Length - 4) / (28 * 28);
                byte[][] data = new byte[numberOfImages][];

                //Get rid of first four numbers
                int magicNumber = reader.ReadInt32();
                int numberOfItems = reader.ReadInt32();
                int numberOfRows = reader.ReadInt32();
                int numberOfColumns = reader.ReadInt32();

                for (int i = 0; i < numberOfImages; i++)
                {
                    data[i] = reader.ReadBytes(28 * 28);
                }

                return data;
            }
        }

        public static byte[][] ReadLabels(string path)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
            {
                long numberOfLabels = reader.BaseStream.Length - 8;
                byte[][] data = new byte[numberOfLabels][];
                
                //Get rid of first two numbers
                int magicNumber = reader.ReadInt32();
                int numberOfItems = reader.ReadInt32();

                for (int i = 0; i < numberOfLabels; i++)
                {
                    data[i] = Enumerable.Repeat((byte)0, 10).ToArray();
                    data[i][reader.ReadByte()] = 1;
                }

                return data;
            }
        }

        public static double[][] Normalize(this byte[][] data)
        {
            double[][] output = new double[data.Length][];

            for (int i = 0; i < data.Length; i++)
            {
                output[i] = new double[data[i].Length];

                double max = data[i].Max();
                double min = data[i].Min();
                
                for (int j = 0; j < data[i].Length; j++)
                {
                    output[i][j] = (data[i][j] - min) / (max - min);
                }
            }

            return output;
        }
    }
}
