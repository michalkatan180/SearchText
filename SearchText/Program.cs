using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SearchText
{
    class Program
    {
        static void Main(string[] args)
        {
            TextFile t = new TextFile("../../../dickens.txt");

            //t.Print();
            Console.WriteLine("number of all lines " + t.GetNumberOfAllLines());//כולל שורות ריקות שבין הקטעים
            Console.WriteLine("******************************************************");
            Console.WriteLine("number of full lines: " + t.GetNumberOfLines());//לא כולל שורות ריקות שבין הקטעים 
            Console.WriteLine("******************************************************");
            Console.WriteLine("number of words: " + t.GetNumberOfWords());
            Console.WriteLine("******************************************************");
            Console.WriteLine("number of unique words: " + t.GetUniqueWordsNumber());
            Console.WriteLine("******************************************************");
            Console.WriteLine("Max sentence length: " + t.GetMaxSentenceLen());
            Console.WriteLine("******************************************************");
            Console.WriteLine("Average sentence length: " + t.GetAvgSentenceLen());
            Console.WriteLine("******************************************************");
            Console.WriteLine("The most popular word is: " + t.GetPopularWord());
            Console.WriteLine("******************************************************");
            Console.WriteLine("The longest withhout 'k': " + t.GetTheLongestWithoutK());
            Console.WriteLine("******************************************************");
            Console.WriteLine("The biggest number: " + t.GetTheBiggestNumber());
            Console.WriteLine("******************************************************");
            Dictionary<string, int> colors = t.GetColors();
            foreach (var c in colors)
                Console.WriteLine("{0}: {1}", c.Key, c.Value);
            if (colors.Count == 0) Console.WriteLine("No colors");
            Console.WriteLine("******************************************************");
            t.PrintNames();
            Console.WriteLine("******************************************************");
            Console.ReadLine();
        }
    }
}
