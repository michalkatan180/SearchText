using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SearchText
{
    class TextFile
    {
        string path;
        StreamReader r;

        public TextFile(string path)
        {
            this.path = path;
        }

        //הדפסה-------------
        public void Print()
        {
            try
            {
                Console.WriteLine(File.ReadAllText(path));
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        //כמות כל השורות בקובץ
        public int GetNumberOfAllLines()
        {
            //כולל שורות ריקות שבין הקטעים
            r = null;
            try
            {
                int cnt = 0;
                r = new StreamReader(path);
                while (!r.EndOfStream)
                {
                    r.ReadLine();
                    cnt++;
                }
                return cnt;
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (r != null) r.Close();
            }
            return -1;
        }

        //כמות השורות בקובץ, לא כולל שורות ריקות
        public int GetNumberOfLines()
        {
            //לא כולל שורות ריקות שבין הקטעים 
            r = null;
            try
            {
                int cnt = 0;
                r = new StreamReader(path);
                while (!r.EndOfStream)
                    if (r.ReadLine() != "") cnt++;
                return cnt;
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (r != null) r.Close();
            }
            return -1;
        }

        //כמות המילים
        public int GetNumberOfWords()
        {
            int cnt = 0;
            try
            {
                string text = File.ReadAllText(path);
                string result = Regex.Replace(text, @"\r\n?|\n", " ");
                //גם תו בודד (מכל סוג) שלפניו יש רווח, יחשב כמילה

                for (int i = 0; i < result.Length; i++)
                    if (result[i] != ' ' && (i == 0 || result[i - 1] == ' '))
                        cnt++;//תחילת מילה חדשה

                //דרך נוספת, ע"י שימוש בפונקצייה של סטרינג
                //יכולה לשמש במקום הלולאה שמעל
                //string[] words = result.Split(' ');
                //for (int i = 0; i < words.Length; i++)
                //    if (words[i] != "") cnt++;//מפני שיתכנו רווחים רצופים

            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return cnt;
        }

        //כמות המילים היחודיות
        public int GetUniqueWordsNumber()
        {
            int cnt = 0;
            try
            {
                string text = File.ReadAllText(path);
                string result = Regex.Replace(text, @"\r\n?|\n", " ");
                Dictionary<string, int> dic = new Dictionary<string, int>();

                for (int i = 0; i < result.Length;)
                    if (result[i] != ' ' && (i == 0 || result[i - 1] == ' '))//תחילת מילה חדשה
                    {
                        //גם תו בודד (מכל סוג) שלפניו יש רווח, יחשב כמילה
                        int j = i + 1;
                        string word = result[i] + "";

                        for (; j < result.Length && result[j] != ' '; j++)
                            word += result[j];

                        //Was=was!
                        word = GetWithoutPunctuation(word);//הערה1
                        word = word.ToUpper(); //הערה2

                        if (!dic.ContainsKey(word)) dic.Add(word, 1);
                        else dic[word]++;//

                        i = j + 1; //הלולאה הפנימית נעצרה כי הגיעה לרווח, או לסוף השורה
                    }
                    else i++;

                //דרך נוספת, ע"י שימוש בפונקצייה של סטרינג
                //יכולה לשמש במקום הלולאה שמעל
                //string[] words = result.Split(' ');
                //for (int i = 0; i < words.Length; i++)
                //    if (words[i] != "")
                //    {
                //        string w = words[i];
                //        w = GetWithoutPunctuation(w);//הערה1
                //        w = w.ToUpper(); //הערה2
                //        if (!dic.ContainsKey(w)) dic.Add(w, 1);
                //        else dic[w]++;
                //    }

                for (int i = 0; i < dic.Count; i++)
                    if (dic.ElementAt(i).Value == 1) cnt++;

                return cnt;

                // 1: GetWithoutPunctuation(word) ----- If "it?" and "it!" are no different words.
                // 2: word.ToUpper() ----- If "it" and "It" are no different words.
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return cnt;
        }

        //קבלת המילה ללא תוים מיוחדים-------------
        private string GetWithoutPunctuation(string s)
        {
            string toReturn = "";
            for (int i = 0; i < s.Length; i++)
            {
                if (!(s[i] == '.' ||
                      s[i] == ',' ||
                      s[i] == ':' ||
                      s[i] == '!' ||
                      s[i] == '%' ||
                      s[i] == '?' || s[i] == '/'))          //וכו' וכו  
                    toReturn += s[i];
            }
            return toReturn;

        }

        //קבל את כמות המילים-------------
        private int GetWordsCount(string line)
        {
            int cnt = 0;
            for (int i = 0; i < line.Length; i++)
                if (line[i] != ' ' && (i == 0 || line[i - 1] == ' '))
                    cnt++;//תחילת מילה חדשה
            return cnt;
        }

        //אורך המשפט הארוך ביותר
        public int GetMaxSentenceLen()
        {
            int max = int.MinValue;
            try
            {
                string text = File.ReadAllText(path);
                string result = Regex.Replace(text, @"\r\n?|\n", " ");
                string sentence = "";
                for (int i = 0; i < result.Length; i++)
                {
                    sentence += result[i];
                    if (result[i] == '.' || result[i] == ',' || result[i] == '!' || result[i] == '?')//סוף משפט
                    {
                        Console.WriteLine(sentence);
                        max = Math.Max(GetWordsCount(sentence), max);
                        //max = Math.Max(sentence.Length, max);//לפי מספר תוים
                        sentence = "";
                    }
                }
                if (sentence != "")
                {
                    Console.WriteLine(sentence);
                    max = Math.Max(GetWordsCount(sentence), max);
                    //max = Math.Max(sentence.Length, max);//לפי מספר תוים                }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return max;
        }

        //אורך ממוצע של משפט
        public double GetAvgSentenceLen()
        {
            int sum = 0, cnt = 0;
            try
            {
                string text = File.ReadAllText(path);
                string result = Regex.Replace(text, @"\r\n?|\n", " ");
                string sentence = "";
                for (int i = 0; i < result.Length; i++)
                {
                    sentence += result[i];
                    if (result[i] == '.' || result[i] == ',' || result[i] == '!' || result[i] == '?')//סוף משפט
                    {
                        sum += GetWordsCount(sentence);
                        cnt++;
                        sentence = "";
                    }
                }
                if (sentence != "")
                {
                    sum += GetWordsCount(sentence);
                    cnt++;
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
            if (cnt > 0) return (double)sum / cnt;
            return sum;
        }

        //קבל את המילה הפופולרית ביותר
        public string GetPopularWord()
        {
            //גם תו בודד (מכל סוג) שלפניו יש רווח, יחשב כמילה

            int cnt = 0, max = int.MinValue;
            string po_word = "";
            Dictionary<string, int> dic = new Dictionary<string, int>();
            try
            {
                string text = File.ReadAllText(path);
                string result = Regex.Replace(text, @"\r\n?|\n", " ");

                for (int i = 0; i < result.Length; i++)
                    if (result[i] != ' ' && (i == 0 || result[i - 1] == ' '))//תחילת מילה חדשה
                    {
                        int j = i + 1;
                        string word = result[i] + "";

                        for (; j < result.Length && result[j] != ' '; j++)
                            word += result[j];

                        word = GetWithoutPunctuation(word);
                        word = word.ToUpper();

                        if (!dic.ContainsKey(word)) dic.Add(word, 1);
                        else dic[word]++;//

                        i = j + 1; //הלולאה הפנימית נעצרה כי הגיעה לרווח, או לסוף השורה
                    }
                    else i++;

                for (int i = 0; i < dic.Count; i++)
                    if (dic.ElementAt(i).Value > max)
                    {
                        po_word = dic.ElementAt(i).Key;
                        max = dic.ElementAt(i).Value;
                    }
                // 1: GetWithoutPunctuation(word) ----- If "it?" and "it!" are no different words.
                // 2: word.ToUpper() ----- If "it" and "It" are no different words.
            }
            catch (IOException ex) { Console.WriteLine(ex.Message); }
            return po_word;
        }

        //אורך הרצף הארוך ביותר בלי ק
        public int GetTheLongestWithoutK()
        {
            int cnt = 0, max = int.MinValue;
            try
            {
                string text = File.ReadAllText(path);
                string result = Regex.Replace(text, @"\r\n?|\n", " ");

                string[] words = result.Split(' ');
                for (int i = 0; i < words.Length; i++)
                    if (words[i] != "")
                    {
                        if (words[i].IndexOf("k") == -1) cnt++;
                        else
                        {
                            max = Math.Max(max, cnt);
                            cnt = 0;
                        }
                    }
                max = Math.Max(max, cnt);
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return max;
        }


        //קבל את המספר הגדול ביותר---------------------לא סיימתי
        public int GetTheBiggestNumber()
        {
            int max = int.MinValue;
            string[] ones = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
            string[] teens = { "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
            string[] tens = { "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };
            var bigscales = new Dictionary<string, int>() {
                {"hundred", 100},
                {"hundreds", 100},
                {"thousand", 1000},
                {"million", 1000000},
                {"billion", 1000000000}};
            try
            {
                string text = File.ReadAllText(path);
                string result = Regex.Replace(text, @"\r\n?|\n", " ");

                char[] splitchars = new char[] { ' ', '-', ',' };
                string[] words = result.ToLower().Split(splitchars, StringSplitOptions.RemoveEmptyEntries);
                int res = 0;
                int currentResult = 0;
                int bigMultiplierValue = 1;
                bool bigMultiplierIsActive = false;
                bool zeroFlag = false;
                bool intFlag = false;
                for (int i = 0; i < words.Length; i++)
                {
                    if (words[i] == "and") continue;
                    int x;
                    if (int.TryParse(words[i], out x)) intFlag = true;
                    int tmp = currentResult;
                    if (bigscales.ContainsKey(words[i]))
                    {
                        bigMultiplierValue *= bigscales[words[i]];
                        bigMultiplierIsActive = true;
                    }
                    else
                    {
                        if (bigMultiplierIsActive)
                        {
                            res += currentResult * bigMultiplierValue;
                            currentResult = 0;
                            bigMultiplierValue = 1;
                            bigMultiplierIsActive = false;
                        }
                        int n;
                        if ((n = Array.IndexOf(ones, words[i]) + 1) > 0) currentResult += n;
                        else if ((n = Array.IndexOf(teens, words[i]) + 1) > 0) currentResult += n + 10;
                        else if ((n = Array.IndexOf(tens, words[i]) + 1) > 0) currentResult += n * 10;
                        else if (words[i] == "zero") zeroFlag = true;
                    }
                    if (intFlag||(currentResult == tmp && currentResult > 0) || zeroFlag)//נגמר המספר
                    {
                        int final = res + currentResult * bigMultiplierValue;
                        Console.WriteLine("final: "+final);
                        max = Math.Max(max, final);
                        res = 0;
                        currentResult = 0;
                        bigMultiplierValue = 1;
                        bigMultiplierIsActive = false;
                        zeroFlag = false;
                        intFlag = false;
                    }
                }


            }
            catch (IOException ex) { Console.WriteLine(ex.Message); }
            return max;
        }

        //קבל את הצבעים ואת מספר הופעותיהם
        public Dictionary<string, int> GetColors()
        {
            string[] CSS_COLOR_NAMES = {
  "AliceBlue",
  "AntiqueWhite",
  "Aqua",
  "Aquamarine",
  "Azure",
  "Beige",
  "Bisque",
  "Black",
  "BlanchedAlmond",
  "Blue",
  "BlueViolet",
  "Brown",
  "BurlyWood",
  "CadetBlue",
  "Chartreuse",
  "Chocolate",
  "Coral",
  "CornflowerBlue",
  "Cornsilk",
  "Crimson",
  "Cyan",
  "DarkBlue",
  "DarkCyan",
  "DarkGoldenRod",
  "DarkGray",
  "DarkGrey",
  "DarkGreen",
  "DarkKhaki",
  "DarkMagenta",
  "DarkOliveGreen",
  "DarkOrange",
  "DarkOrchid",
  "DarkRed",
  "DarkSalmon",
  "DarkSeaGreen",
  "DarkSlateBlue",
  "DarkSlateGray",
  "DarkSlateGrey",
  "DarkTurquoise",
  "DarkViolet",
  "DeepPink",
  "DeepSkyBlue",
  "DimGray",
  "DimGrey",
  "DodgerBlue",
  "FireBrick",
  "FloralWhite",
  "ForestGreen",
  "Fuchsia",
  "Gainsboro",
  "GhostWhite",
  "Gold",
  "GoldenRod",
  "Gray",
  "Grey",
  "Green",
  "GreenYellow",
  "HoneyDew",
  "HotPink",
  "IndianRed",
  "Indigo",
  "Ivory",
  "Khaki",
  "Lavender",
  "LavenderBlush",
  "LawnGreen",
  "LemonChiffon",
  "LightBlue",
  "LightCoral",
  "LightCyan",
  "LightGoldenRodYellow",
  "LightGray",
  "LightGrey",
  "LightGreen",
  "LightPink",
  "LightSalmon",
  "LightSeaGreen",
  "LightSkyBlue",
  "LightSlateGray",
  "LightSlateGrey",
  "LightSteelBlue",
  "LightYellow",
  "Lime",
  "LimeGreen",
  "Linen",
  "Magenta",
  "Maroon",
  "MediumAquaMarine",
  "MediumBlue",
  "MediumOrchid",
  "MediumPurple",
  "MediumSeaGreen",
  "MediumSlateBlue",
  "MediumSpringGreen",
  "MediumTurquoise",
  "MediumVioletRed",
  "MidnightBlue",
  "MintCream",
  "MistyRose",
  "Moccasin",
  "NavajoWhite",
  "Navy",
  "OldLace",
  "Olive",
  "OliveDrab",
  "Orange",
  "OrangeRed",
  "Orchid",
  "PaleGoldenRod",
  "PaleGreen",
  "PaleTurquoise",
  "PaleVioletRed",
  "PapayaWhip",
  "PeachPuff",
  "Peru",
  "Pink",
  "Plum",
  "PowderBlue",
  "Purple",
  "RebeccaPurple",
  "Red",
  "RosyBrown",
  "RoyalBlue",
  "SaddleBrown",
  "Salmon",
  "SandyBrown",
  "SeaGreen",
  "SeaShell",
  "Sienna",
  "Silver",
  "SkyBlue",
  "SlateBlue",
  "SlateGray",
  "SlateGrey",
  "Snow",
  "SpringGreen",
  "SteelBlue",
  "Tan",
  "Teal",
  "Thistle",
  "Tomato",
  "Turquoise",
  "Violet",
  "Wheat",
  "White",
  "WhiteSmoke",
  "Yellow",
  "YellowGreen" };
            Dictionary<string, int> dic = new Dictionary<string, int>();
            try
            {
                string text = File.ReadAllText(path);
                string result = Regex.Replace(text, @"\r\n?|\n", " ").ToLower();
                string[] words = result.Split(' ');

                for (int i = 0; i < CSS_COLOR_NAMES.Length; i++)
                    CSS_COLOR_NAMES[i] = CSS_COLOR_NAMES[i].ToLower();

                for (int i = 0; i < words.Length; i++)
                    if (words[i] != "")
                        if (Array.IndexOf(CSS_COLOR_NAMES, words[i]) != -1)
                        {
                            if (!dic.ContainsKey(words[i])) dic.Add(words[i], 1);
                            else dic[words[i]]++;
                        }
            }
            catch (IOException ex) { Console.WriteLine(ex.Message); }
            return dic;
        }

        //קבל את שמות הדמויות
        /*public Dictionary<string, int> GetColors()
        {
            string[] people =
            {
        "Joe",
        "Jarvis Lorry",
        "Jerry",
        "Tom",
            "",
            "",""}
            Dictionary<string, int> dic = new Dictionary<string, int>();
            try
            {
                string text = File.ReadAllText(path);
                string result = Regex.Replace(text, @"\r\n?|\n", " ").ToLower();
                string[] words = result.Split(' ');

                for (int i = 0; i < CSS_COLOR_NAMES.Length; i++)
                    CSS_COLOR_NAMES[i] = CSS_COLOR_NAMES[i].ToLower();

                for (int i = 0; i < words.Length; i++)
                    if (words[i] != "")
                        if (Array.IndexOf(CSS_COLOR_NAMES, words[i]) != -1)
                        {
                            if (!dic.ContainsKey(words[i])) dic.Add(words[i], 1);
                            else dic[words[i]]++;
                        }
            }
            catch (IOException ex) { Console.WriteLine(ex.Message); }
            return dic;
        }
        */

        //קבל את השם הנפוץ ביותר


        //המרת מחרוזת של מספר
        public static int WordNumberToInt(string number)
        {
            //stackoverflowאת הפונקציה הזה מצאתי ב       
            string[] ones = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
            string[] teens = { "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
            string[] tens = { "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };
            var bigscales = new Dictionary<string, int>() {
                {"hundred", 100},
                {"hundreds", 100},
                {"thousand", 1000},
                {"million", 1000000},
                {"billion", 1000000000}};
            string[] minusWords = { "minus", "negative" };
            var splitchars = new char[] { ' ', '-', ',' };

            var lowercase = number.ToLower();
            var inputwords = lowercase.Split(splitchars, StringSplitOptions.RemoveEmptyEntries);

            int result = 0;
            int currentResult = 0;
            int bigMultiplierValue = 1;
            bool bigMultiplierIsActive = false;
            bool minusFlag = false;

            foreach (string curword in inputwords)
            {
                // input words are either bigMultipler words or little words
                //
                if (bigscales.ContainsKey(curword))
                {
                    bigMultiplierValue *= bigscales[curword];
                    bigMultiplierIsActive = true;
                }

                else
                {
                    // multiply the current result by the previous word bigMultiplier
                    // and disable the big multiplier until next time
                    if (bigMultiplierIsActive)
                    {
                        result += currentResult * bigMultiplierValue;
                        currentResult = 0;
                        bigMultiplierValue = 1; // reset the multiplier value
                        bigMultiplierIsActive = false; // turn it off until next time
                    }

                    // translate the incoming text word to an integer
                    int n;
                    if ((n = Array.IndexOf(ones, curword) + 1) > 0)
                    {
                        currentResult += n;
                    }
                    else if ((n = Array.IndexOf(teens, curword) + 1) > 0)
                    {
                        currentResult += n + 10;
                    }
                    else if ((n = Array.IndexOf(tens, curword) + 1) > 0)
                    {
                        currentResult += n * 10;
                    }
                    // allow for negative words (like "minus") 
                    else if (minusWords.Contains(curword))
                    {
                        minusFlag = true;
                    }
                    // allow for phrases like "zero 500" hours military time
                    else if (curword == "zero")
                    {
                        continue;
                    }
                    // allow for text digits too, like "100 and 5"
                    else if (int.TryParse(curword, out int tmp))
                    {
                        currentResult += tmp;
                    }
                    //else if (curword != "and")
                    //{
                    //    throw new ApplicationException("Expected a number: " + curword);
                    //}
                }
            }

            var final = result + currentResult * bigMultiplierValue;
            if (minusFlag)
                final *= -1;
            return final;
        }

    }
}