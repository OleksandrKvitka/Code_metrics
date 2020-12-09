using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Text;

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
            //LogicLoc = CalculateLogicalLoc();
        }

        public void Print()
        {
            Console.WriteLine($"Count of phisic lines of code = {PhisicLoc}");
            Console.WriteLine($"Count of blank lines of code = {BlankLoc}");
            Console.WriteLine($"Count of commented lines of code = {CommentLoc}");
            //Console.WriteLine($"Count of logic lines of code = {LogicLoc}");
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
            var keywords = new string[] { "else", "if", "?", "try", "catch", "switch", "for", "while", "return", "break", "goto", "exit", "continue", "throw",  "finally", ";", "namespace" };
            foreach (string _line in LinesList)
            {
                string line = _line;
                bool isChecked = false;
                while (!isChecked)
                {
                    foreach (string keyword in keywords)
                    {
                        if (line.Contains(keyword))
                        {
                            if (keyword == "?" && line.Contains(":"))
                            {
                                line = line.TrimStart('?');
                                line = line.TrimStart(':');
                            }
                            else if (keyword == "else" && line.Contains("else if"))
                            {
                                line.Remove(line.IndexOf(keyword), keyword.Length + 3);
                            }
                            else
                                line.Remove(line.IndexOf(keyword), keyword.Length);
                            counterLoc++;
                        }
                        else if (line.Contains("("))
                        {
                            counterLoc++;
                            line = line.TrimStart('(');
                        }
                        else
                            isChecked = true;
                    }      
                }
            }
            return counterLoc;
        }

        //public bool isText(string str)
        //{

        //}
    }
}
