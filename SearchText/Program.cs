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
            //Console.WriteLine(t.GetNumberOfLines());//לא כולל שורות ריקות שבין הקטעים 
            //Console.WriteLine(t.GetNumberOfWords());
            //Console.WriteLine(t.GetUniqueWordsNumber());
            //Console.WriteLine("Max sentence length: " + t.GetMaxSentenceLen());
            //Console.WriteLine("Average sentence length: " + t.GetAvgSentenceLen());
            //Console.WriteLine("The most popular word is: " + t.GetPopularWord());
            //Console.WriteLine("The most popular word2 is: " + t.GetPopularWord2());
            Console.WriteLine(t.GetTheLongestWithoutK());

            Console.ReadLine();
        }
    }
}
