﻿using System;
using System.IO;

namespace NeuralOperator
{
    class Program
    {
        readonly static string directory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName + "\\Files\\";

        static void Main(string[] args)
        {
            Console.WriteLine("Please be patient...");
            Operator.TrainAugmentation(learningRatio: 0.3, epochs: 1, path: directory, 28 * 28, 200, 10);
            Operator.Test(directory);
            Console.WriteLine("Accuracy: " + Operator.Accuracy * 100 + " %");
            Console.WriteLine("Want to serialize? (y/n)");
            string read = Console.ReadLine();
            while (!read.ToLower().Equals("y") && !read.ToLower().Equals("n"))
                read = Console.ReadLine();
            if (read.ToLower().Equals("y"))
                Operator.Serialize(directory);
            Console.ReadKey();
        }
    }
}
