using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Code_metrics
{
    public class Analyzer
    {
        private List<string> LinesList;
        public int BlankLoc;
        public int PhisicLoc;
        public int LogicLoc;
        public int CommentLoc;

        public Analyzer()
        {
            LinesList = new List<string>();
        }
        public void Read(string path)
        {
            using (ZipArchive archive = ZipFile.OpenRead(path))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    if (entry.FullName.EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
                    {
                        using (var reader = new StreamReader(entry.Open()))
                        {
                            while (reader.Peek() >= 0)
                            {
                                LinesList.Add(reader.ReadLine());
                            }
                        }
                    }
                }
            }
        }

        public void CalculateAllMetrics()
        {
            PhisicLoc = LinesList.Count;
            BlankLoc = CalculateBlankLoc();
            CommentLoc = CalculateCommentedLoc();
            LogicLoc = CalculateLogicalLoc();
        }

        public void Print()
        {
            Console.WriteLine($"Count of phisic lines of code = {PhisicLoc}");
            Console.WriteLine($"Count of blank lines of code = {BlankLoc}");
            Console.WriteLine($"Count of commented lines of code = {CommentLoc}");
            Console.WriteLine($"Count of logic lines of code = {LogicLoc}");
        }

        public int CalculateBlankLoc()
        {
            int counter = 0;
            foreach (string line in LinesList)
            {
                if (String.IsNullOrWhiteSpace(line))
                {
                    counter++;
                }
            }
            return counter;
        }

        public int CalculateCommentedLoc()
        {
            int counter = 0;
            bool isComment = false;
            foreach (string line in LinesList)
            {
                for (int i = 0; i < line.Length; i++)
                {
                    if (line[i] == '/' && !isComment)
                    {
                        if (line[i + 1] == '/')
                        {
                            counter++;
                            break;
                        }
                        else if (line[i + 1] == '*')
                        {
                            counter++;
                            i++;
                            isComment = true;
                            continue;
                        }
                    }
                    if (isComment && i == 0)
                    {
                        counter++;
                    }
                    if (line[i] == '*' && line[i + 1] == '/')
                    {
                        isComment = false;
                        i++;
                        continue;
                    }
                }
            }
            return counter;
        }

        public int CalculateLogicalLoc()
        {
            int counterLoc = 0;
            var keywords = new string[] { " else ", " if ", "?", " try ", " catch ", " switch ", " for ", " foreach ", " while ", " return ", " break ", " goto ", " exit ", " continue ", " throw ",  " finally ", ";"};
            foreach (string _line in LinesList)
            {
                if (!String.IsNullOrWhiteSpace(_line))
                {
                    //string line = CleanFromText(_line);
                    string line = _line;
                    bool isChecked = false;
                    foreach (string keyword in keywords)
                    {
                        while (line.Contains(keyword))
                        {
                            if (keyword == "?" && line.Contains(":"))
                            {
                                line = line.Remove(line.IndexOf("?"), 1);
                                line = line.Remove(line.IndexOf(":"), 1);
                            }
                            else if (keyword == " else " && line.Contains(" else if "))
                            {
                                line = line.Remove(line.IndexOf(keyword), keyword.Length + 3);
                            }
                            else
                                line = line.Remove(line.IndexOf(keyword), keyword.Length);
                            counterLoc++;
                            isChecked = true;
                        }
                    }
                    while (line.Contains("(") && !isChecked)
                    {
                        counterLoc++;
                        line = line.Remove(line.IndexOf("("), 1);
                    }
                }
            }
            return counterLoc;
        }

        public string CleanFromText(string str)
        {
            var symbol1 = '"'.ToString();
            var symbol2 = "'";
            int amount1 = new Regex(symbol1).Matches(str).Count;
            string resString = "";
            if (amount1 % 2 == 0)
            {
                var substring = str.Split(symbol1);
                for (int i = 0; i < substring.Length; i++)
                {
                    if (i % 2 == 0)
                        resString += substring[i];
                }
                str = resString;
            }
            int amount2 = new Regex(symbol2).Matches(str).Count;
            if (amount2 % 2 == 0)
            {
                var substring = str.Split(symbol2);
                for (int i = 0; i < substring.Length; i++)
                {
                    if (i % 2 == 0)
                        resString += substring[i];
                }
                str = resString;
            }

            return str;
        }
    }
}
