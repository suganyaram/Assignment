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

        private static void ValidateSentences()
        {
            try
            {
                String line;
                Dictionary<string, int> wordList = new Dictionary<string, int>();

                //Opens a file in read mode  

                using (StreamReader file = new StreamReader("C:\\Users\\suganya.r\\Assignment\\ContactBookAPI\\ConsoleApp1\\data.txt"))
                {
                    //Gets each line till end of file is reached  
                    while ((line = file.ReadLine()) != null)
                    {
                        //Splits each line into words  
                        String[] sentences = line.Split('.');

                        foreach (var sentence in sentences)
                        {
                            int vowelCount = 0;
                            foreach (var c in sentence)
                            {
                                if (c == 'a' || c == 'e' || c == 'i' || c == 'o' || c == 'u')
                                {
                                    vowelCount++;
                                }
                            }
                            wordList.TryAdd(sentence, vowelCount);
                        }
                    }

                    foreach (var word in wordList.OrderByDescending(key => key.Value))
                    {
                        Console.WriteLine("Key: {0}, Value: {1}", word.Key, word.Value);
                    }
                }

                using (StreamWriter sw = new StreamWriter("C:\\Users\\suganya.r\\Assignment\\ContactBookAPI\\ConsoleApp1\\output.txt"))
                {
                    foreach (var s in wordList.OrderByDescending(key => key.Value))
                    {
                        sw.WriteLine(s.Key);
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error");
            }
        }
    }
}
