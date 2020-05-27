using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Mnist
{
    public static class MnistFiles
    {
        private static Random random = new Random();

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

        public static int[] ReadFullLables(string path)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
            {
                long numberOfLabels = reader.BaseStream.Length - 8;
                int[] data = new int[numberOfLabels];

                //Get rid of first two numbers
                int magicNumber = reader.ReadInt32();
                int numberOfItems = reader.ReadInt32();

                for (int i = 0; i < numberOfLabels; i++)
                {
                    data[i] = reader.ReadByte();
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

        public static List<(byte[], byte[])> Augmentation(this byte[][] data, byte[][] labels)
        {
            List<(byte[], byte[])> output = new List<(byte[], byte[])>();
            List<Func<byte[], byte[]>> funcs = new List<Func<byte[], byte[]>>();
            funcs.Add(Left);
            funcs.Add(Right);
            funcs.Add(Upside);
            funcs.Add(Noise);

            for (int i = 0; i < data.Length; i++)
            {
                output.Add((data[i], labels[i]));
                output.Add((funcs[random.Next(funcs.Count)](data[i]), labels[i]));
            }

            output.Shuffle();
            return output;
        }

        private static byte[] Left(byte[] input)
        {
            byte[] output = (byte[])input.Clone();

            int k = 1;
            int l = 1;

            for (int i = 0; i < 28 * 28; i += 28)
            {
                for (int j = i; j < i + 28; j++)
                {
                    output[j] = input[l * 28 - k];
                    l++;
                }
                l = 1;
                k++;
            }

            return output;
        }

        private static byte[] Right(byte[] input)
        {
            byte[] output = (byte[])input.Clone();

            int k = 28;
            int l = 28;

            for (int i = 0; i < 28 * 28; i += 28)
            {
                for (int j = i; j < i + 28; j++)
                {
                    output[j] = input[l * 28 - k];
                    l--;
                }
                l = 28;
                k--;
            }

            return output;
        }

        private static byte[] Upside(byte[] input)
        {
            byte[] output = (byte[])input.Clone();

            int k = 28;
            int l = 28;

            for (int i = 0; i < 28 * 28; i += 28)
            {
                for (int j = i; j < i + 28; j++)
                {
                    output[j] = input[l * 28 - k];
                    k--;
                }
                k = 28;
                l--;
            }

            return output;
        }

        private static byte[] Noise(byte[] input)
        {
            byte[] output = (byte[])input.Clone();

            for (int i = 0; i < 100; i++)
            {
                output[random.Next(output.Length)] = 255;
            }

            return output;
        }

        private static void Shuffle(this List<(byte[], byte[])> data)
        {
            for (int i = 0; i < data.Count; i++)
            {
                (byte[], byte[]) tmp = data[i];
                int r = random.Next(i, data.Count);
                data[i] = data[r];
                data[r] = tmp;
            }
        }
    }
}
