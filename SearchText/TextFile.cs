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
                    if (result[i] != ' ' && (i == 0 || result[i - 1] == ' ' || result[i - 1] == '-'))
                        cnt++;//תחילת מילה חדשה

                //דרך נוספת, ע"י שימוש בפונקצייה של סטרינג
                //יכולה לשמש במקום הלולאה שמעל
                //char[] splitchars = new char[] { ' ', '-', ',' };
                //string[] words = result.Split(splitchars, StringSplitOptions.RemoveEmptyEntries); 
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
                    if (result[i] != ' ' && (i == 0 || result[i - 1] == ' ' || result[i - 1] == '-'))//תחילת מילה חדשה
                    {
                        //גם תו בודד (מכל סוג) שלפניו יש רווח, יחשב כמילה
                        int j = i + 1;
                        string word = result[i] + "";

                        for (; j < result.Length && result[j] != ' ' && result[j] != '-'; j++)
                            word += result[j];

                        //Was=was!
                        word = GetWithoutPunctuation(word);//הערה1
                        word = word.ToUpper(); //הערה2

                        if (!dic.ContainsKey(word)) dic.Add(word, 1);
                        else dic[word]++;//

                        i = j + 1; //הלולאה הפנימית נעצרה כי הגיעה לרווח, או לסוף השורה
                    }
                    else i++;

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
            //דרך א
            /*int cnt = 0;
            for (int i = 0; i < line.Length; i++)
                if (line[i] != ' ' && (i == 0 || line[i - 1] == ' '|| line[i - 1] == '-'))
                    cnt++;//תחילת מילה חדשה
            return cnt;*/

            //דרך ב
            char[] splitchars = new char[] { ' ', '-', ',' };
            string[] words = line.Split(splitchars, StringSplitOptions.RemoveEmptyEntries);
            return words.Length;
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
                        max = Math.Max(GetWordsCount(sentence), max);
                        //max = Math.Max(sentence.Length, max);//לפי מספר תוים
                        sentence = "";
                    }
                }
                if (sentence != "")
                {
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
                   if (GetWordsCount(sentence)>0)cnt++;
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
            if (cnt > 0) return(double)sum / cnt;
            return sum;
        }

        //קבל את המילה הפופולרית ביותר
        public string GetPopularWord()
        {
            //גם תו בודד (מכל סוג) שלפניו יש רווח, יחשב כמילה

            int max = int.MinValue;
            string po_word = "";
            Dictionary<string, int> dic = new Dictionary<string, int>();
            try
            {
                string text = File.ReadAllText(path);
                string result = Regex.Replace(text, @"\r\n?|\n", " ");

                for (int i = 0; i < result.Length; i++)
                    if (result[i] != ' ' && (i == 0 || result[i - 1] == ' ' || result[i - 1] == '-'))//תחילת מילה חדשה
                    {
                        int j = i + 1;
                        string word = result[i] + "";

                        for (; j < result.Length && result[j] != '-' && result[j] != ' '; j++)
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

                char[] splitchars = new char[] { ' ', '-', ',' };
                string[] words = result.ToLower().Split(splitchars, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < words.Length; i++)
                    if (words[i].IndexOf("k") == -1) cnt++;
                    else
                    {
                        max = Math.Max(max, cnt);
                        cnt = 0;
                    }

                max = Math.Max(max, cnt);
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return max;
        }


        //קבל את המספר הגדול ביותר
        public int GetTheBiggestNumber()
        {
            //בהנחה שאין מספרים ברצף
            //ואין מספרים שיש בתוכם פסיק 500,422
            int max = int.MinValue;
            string[] ones = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
            string[] teens = { "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
            string[] tens = { "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };
            Dictionary<string, int> bigscales = new Dictionary<string, int>() {
                {"hundred", 100},
                {"hundreds", 100},
                {"thousand", 1000},
                {"000", 1000},//500,000
                {"million", 1000000},
                {"billion", 1000000000}};

            try
            {
                string text = File.ReadAllText(path);
                string result = Regex.Replace(text, @"\r\n?|\n", " ");
                int final = 0;

                char[] splitchars = new char[] { ' ', '-', ',' };
                string[] words = result.ToLower().Split(splitchars, StringSplitOptions.RemoveEmptyEntries);
                int res = 0;
                int currentResult = 0;
                int tmp;
                bool zeroFlag = false;
                bool intFlag = false;
                for (int i = 0; i < words.Length; i++)
                {
                    if (words[i] == "and") continue;
                    tmp = currentResult;
                    if (int.TryParse(words[i], out int x))
                    {
                        currentResult = x;
                        intFlag = true;
                    }
                    if (bigscales.ContainsKey(words[i]))
                    {
                        if (currentResult == 0) res += bigscales[words[i]];
                        else res += currentResult * bigscales[words[i]];
                        currentResult = 0;
                    }
                    else
                    {
                        int n;
                        if ((n = Array.IndexOf(ones, words[i]) + 1) > 0) currentResult += n;
                        else if ((n = Array.IndexOf(teens, words[i]) + 1) > 0) currentResult += n + 10;
                        else if ((n = Array.IndexOf(tens, words[i]) + 1) > 0) currentResult += n * 10;
                        else if (words[i] == "zero") zeroFlag = true;

                    }
                    if (tmp == currentResult || intFlag || zeroFlag)//נגמר המספר
                    {
                        final = res + currentResult;
                        if (zeroFlag || final != 0) max = Math.Max(max, final);
                        res = 0;
                        currentResult = 0;
                        zeroFlag = false;
                    }
                }
                final = res + currentResult;
                max = Math.Max(max, final);
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
                char[] splitchars = new char[] { ' ', ',', '.', '!', '?', '-' };

                string[] words = result.Split(splitchars, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < CSS_COLOR_NAMES.Length; i++)
                    CSS_COLOR_NAMES[i] = CSS_COLOR_NAMES[i].ToLower();

                for (int i = 0; i < words.Length; i++)
                    if (Array.IndexOf(CSS_COLOR_NAMES, words[i]) != -1)
                    {
                        if (!dic.ContainsKey(words[i])) dic.Add(words[i], 1);
                        else dic[words[i]]++;
                    }
            }
            catch (IOException ex) { Console.WriteLine(ex.Message); }
            return dic;
        }

        //הדפס את שמות הדמויות ואת הפופולרית ביותר
        public void PrintNames()
        {
            Dictionary<string, string> people = new Dictionary<string, string>(){
                { "Joe" ,""},
                { "Jarvis","Jarvis Lorry"},
                { "Lorry","Jarvis Lorry"},
                { "Jerry","" },
                { "Tom","" },
                {"Michal","Michal Katan" },
                {"Katan","Michal Katan" }
            };
            Dictionary<string, int> resultNames = new Dictionary<string, int>();
            try
            {
                string text = File.ReadAllText(path);
                string result = Regex.Replace(text, @"\r\n?|\n", " ");
                char[] splitchars = new char[] { ' ', ',', '.', '!', '?', '-' };

                string[] words = result.Split(splitchars, StringSplitOptions.RemoveEmptyEntries);
                int index = -1;
                for (int i = 0; i < words.Length; i++)
                    if ((index = Array.IndexOf(people.Keys.ToArray(), words[i])) > -1)//אם זה דמות
                    {
                        if (i < words.Length - 1)
                        {
                            //האם המילה הבאה היא המשך השם של הדמות שם משפחה+פרטי
                            if (Math.Abs(Array.IndexOf(people.Keys.ToArray(), words[i + 1]) - index) == 1)
                                i++;//לדלג על המשך השם
                        }

                        string fullName = people.ElementAt(index).Value;
                        if (fullName == "") fullName = people.ElementAt(index).Key;
                        if (!resultNames.ContainsKey(fullName)) resultNames.Add(fullName, 1);
                        else resultNames[fullName]++;
                    }

                int max = int.MinValue;
                string max_name = "";
                for (int i = 0; i < resultNames.Count; i++)
                {
                    int cnt = resultNames.ElementAt(i).Value;
                    string name = resultNames.ElementAt(i).Key;
                    if (cnt > max)
                    {
                        max = cnt;
                        max_name = name;
                    }
                    Console.WriteLine("{0}: {1}", name, cnt);
                }
                Console.WriteLine("The popolar name is: {0} ({1}).", max_name, max);

            }
            catch (IOException ex) { Console.WriteLine(ex.Message); }
            //return dic;
        }




    }
}
