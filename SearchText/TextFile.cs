using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
        public void Print()
        {
            r = null;
            try
            {
                r = new StreamReader(path);
                while (!r.EndOfStream)
                    Console.WriteLine(r.ReadLine());
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (r != null) r.Close();
            }
        }
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
        public int GetNumberOfWords()
        {
            r = null;
            try
            {
                int cnt = 0;
                r = new StreamReader(path);
                string line;
                while (!r.EndOfStream)
                {
                    //גם תו בודד (מכל סוג) שלפניו יש רווח, יחשב כמילה
                    line = r.ReadLine();

                    for (int i = 0; i < line.Length; i++)
                        if (line[i] != ' ' && (i == 0 || line[i - 1] == ' '))
                            cnt++;//תחילת מילה חדשה


                    //דרך נוספת, ע"י שימוש בפונקצייה של סטרינג
                    //יכולה לשמש במקום הלולאה שמעל
                    //string[] words = line.Split(' ');
                    //for (int i = 0; i < words.Length; i++)
                    //    if (words[i] != "") cnt++;//מפני שיתכנו רווחים רצופים


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
        public int GetUniqueWordsNumber()
        {
            //גם תו בודד (מכל סוג) שלפניו יש רווח, יחשב כמילה

            int cnt = 0;
            r = null;
            try
            {
                r = new StreamReader(path);
                Dictionary<string, int> dic = new Dictionary<string, int>();
                string line;
                while (!r.EndOfStream)
                {
                    line = r.ReadLine();

                    for (int i = 0; i < line.Length;)
                        if (line[i] != ' ' && (i == 0 || line[i - 1] == ' '))//תחילת מילה חדשה
                        {
                            int j = i + 1;
                            string word = line[i] + "";

                            for (; j < line.Length && line[j] != ' '; j++)
                                word += line[j];

                            word = GetWithoutPunctuation(word);//הערה1
                            //word = word.ToUpper(); //הערה2

                            if (!dic.ContainsKey(word)) dic.Add(word, 1);
                            else dic[word]++;//

                            i = j + 1; //הלולאה הפנימית נעצרה כי הגיעה לרווח, או לסוף השורה
                        }
                        else i++;
                }

                Console.Write("The unique words are:");
                for (int i = 0; i < dic.Count; i++)
                    if (dic.ElementAt(i).Value == 1)
                    {
                        Console.Write(" " + dic.ElementAt(i).Key);
                        cnt++;
                    }
                Console.WriteLine();

                return cnt;

                // 1: GetWithoutPunctuation(word) ----- If "it?" and "it!" are no different words.
                // 2: word.ToUpper() ----- If "it" and "It" are no different words.
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (r != null) r.Close();
            }
            return cnt;
        }
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
        public int GetMaxSentenceLen()
        {
            //משפט מסתיים בנקודה או עד סוף הקובץ
            int max = int.MinValue;
            r = null;
            try
            {
                r = new StreamReader(path);
                while (!r.EndOfStream)
                {
                    //לפי מה האורך נמדד?
                    max = Math.Max(GetWordsCount(r.ReadLine()), max);
                    //max = Math.Max(r.ReadLine().Length, max);
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (r != null) r.Close();
            }
            return max;
        }
        public double GetAvgSentenceLen()
        {
            //משפט מסתיים בנקודה או עד סוף הקובץ
            int sum = 0, cnt = 0;
            r = null;
            try
            {
                r = new StreamReader(path);
                while (!r.EndOfStream)
                {
                    string line = r.ReadLine();

                    //לפי מה האורך נמדד?
                    sum += GetWordsCount(line);
                    //sum += line.Length;
                    if (line != "") cnt++;
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (r != null) r.Close();
            }

            if (cnt > 0) return (double)sum / cnt;
            return sum;
        }
        private int GetWordsCount(string line)
        {
            int cnt = 0;
            for (int i = 0; i < line.Length; i++)
                if (line[i] != ' ' && (i == 0 || line[i - 1] == ' '))
                    cnt++;//תחילת מילה חדשה
            return cnt;
        }
        public string GetPopularWord()
        {
            //גם תו בודד (מכל סוג) שלפניו יש רווח, יחשב כמילה

            int cnt = 0, max = int.MinValue;
            string po_word = "";
            r = null;
            try
            {
                r = new StreamReader(path);
                Dictionary<string, int> dic = new Dictionary<string, int>();
                string line;
                while (!r.EndOfStream)
                {
                    line = r.ReadLine();

                    for (int i = 0; i < line.Length;)
                        if (line[i] != ' ' && (i == 0 || line[i - 1] == ' '))//תחילת מילה חדשה
                        {
                            int j = i + 1;
                            string word = line[i] + "";

                            for (; j < line.Length && line[j] != ' '; j++)
                                word += line[j];

                            word = GetWithoutPunctuation(word);//הערה1
                            //word = word.ToUpper(); //הערה2

                            if (!dic.ContainsKey(word)) dic.Add(word, 1);
                            else dic[word]++;//

                            i = j + 1; //הלולאה הפנימית נעצרה כי הגיעה לרווח, או לסוף השורה
                        }
                        else i++;
                }

                for (int i = 0; i < dic.Count; i++)
                    if (dic.ElementAt(i).Value > max)
                    {
                        po_word = dic.ElementAt(i).Key;
                        max = dic.ElementAt(i).Value;
                    }

                // 1: GetWithoutPunctuation(word) ----- If "it?" and "it!" are no different words.
                // 2: word.ToUpper() ----- If "it" and "It" are no different words.
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (r != null) r.Close();
            }
            return po_word;
        }
        public string GetPopularWord2()
        {
            //גם תו בודד (מכל סוג) שלפניו יש רווח, יחשב כמילה
            //מילים ללא משמעות תחבירית
            string[] arr = { "am", "are", "don't", "that", "the", "is", "this", "a", "an" };

            int cnt = 0, max = int.MinValue;
            string po_word = "";
            r = null;
            try
            {
                r = new StreamReader(path);
                Dictionary<string, int> dic = new Dictionary<string, int>();
                string line;
                while (!r.EndOfStream)
                {
                    line = r.ReadLine();

                    for (int i = 0; i < line.Length;)
                        if (line[i] != ' ' && (i == 0 || line[i - 1] == ' '))//תחילת מילה חדשה
                        {
                            int j = i + 1;
                            string word = line[i] + "";

                            for (; j < line.Length && line[j] != ' '; j++)
                                word += line[j];

                            word = GetWithoutPunctuation(word);//הערה1
                            //word = word.ToUpper(); //הערה2

                            if (Array.IndexOf(arr, word) == -1)//האם המילה בעלת משמעות תחבירית
                            {
                                if (!dic.ContainsKey(word)) dic.Add(word, 1);
                                else dic[word]++;//
                            }
                            i = j + 1; //הלולאה הפנימית נעצרה כי הגיעה לרווח, או לסוף השורה
                        }
                        else i++;
                }

                for (int i = 0; i < dic.Count; i++)
                    if (dic.ElementAt(i).Value > max)
                    {
                        po_word = dic.ElementAt(i).Key;
                        max = dic.ElementAt(i).Value;
                    }

                // 1: GetWithoutPunctuation(word) ----- If "it?" and "it!" are no different words.
                // 2: word.ToUpper() ----- If "it" and "It" are no different words.
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (r != null) r.Close();
            }
            return po_word;
        }

        public int GetTheLongestWithoutK()
        {
            int cnt = 0, max = int.MinValue;
            try
            {
                string[] lines = File.ReadAllLines(path);
                string allTxt = "";

                for (int i = 0; i < lines.Length; i++)
                    allTxt += lines[i] + " ";

                //גם תו בודד (מכל סוג) שלפניו יש רווח, יחשב כמילה
                string[] words = allTxt.Split(' ');
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

    }
}