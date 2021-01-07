using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            ValidateSentences();
        }

        public static void ValidateSentences()
        {
            try
            {
                Dictionary<string, int> wordList = new Dictionary<string, int>();
                string inputFilename, outputFilename;
                IOFileNamePath(out inputFilename, out outputFilename);
                //Opens a file in read mode  
                using (StreamReader file = new StreamReader(inputFilename))
                {
                    String line = file.ReadToEnd();

                    //Splits each line into sentences  
                    String[] sentences = line.Split('.');

                    foreach (var sentence in sentences)
                    {
                        int vowelCount = 0;
                        foreach (var c in sentence)
                        {
                            if (c == 'a' || c == 'e' || c == 'i' || c == 'o' || c == 'u' || c == 'A' || c == 'E' || c == 'I' || c == 'O' || c == 'U')
                            {
                                vowelCount++;
                            }
                        }
                        wordList.TryAdd(sentence, vowelCount);
                    }

                    foreach (var word in wordList.OrderByDescending(key => key.Value))
                    {
                        Console.WriteLine("Key: {0}, Value: {1}", word.Key, word.Value);
                    }
                }

                using (StreamWriter sw = new StreamWriter(outputFilename))
                {
                    foreach (var s in wordList.OrderByDescending(key => key.Value))
                    {
                        sw.WriteLine(s.Key);
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("invalid file");
            }
        }

        private static void IOFileNamePath(out string inputFilename, out string outputFilename)
        {
            inputFilename = @"C:\Users\suganya.r\Assignment\ContactBookAPI\ConsoleApp1\data.txt";
            outputFilename = @"C:\Users\suganya.r\Assignment\ContactBookAPI\ConsoleApp1\output.txt";
        }
    }
}
