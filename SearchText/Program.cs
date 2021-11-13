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
            //Console.WriteLine(t.GetNumberOfAllLines());//כולל שורות ריקות שבין הקטעים
            Console.WriteLine(t.GetNumberOfLines());//לא כולל שורות ריקות שבין הקטעים 
           Console.WriteLine(t.GetNumberOfWords());
            Console.WriteLine(t.GetUniqueWordsNumber());
            //Console.WriteLine("Max sentence length: " + t.GetMaxSentenceLen());
            //Console.WriteLine("Average sentence length: " + t.GetAvgSentenceLen());
            //Console.WriteLine("The most popular word is: " + t.GetPopularWord());
            //Console.WriteLine(t.GetTheLongestWithoutK());
            Console.WriteLine("The biggest number:"+t.GetTheBiggestNumber());
            
            /*Dictionary<string, int> colors = t.GetColors();
            foreach(var c in colors)
                Console.WriteLine("{0}: {1}",c.Key,c.Value);
            if (colors.Count == 0) Console.WriteLine("No colors");*/

            Console.ReadLine();
        }
    }
}
