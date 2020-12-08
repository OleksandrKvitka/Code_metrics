using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Text;

namespace Code_metrics
{
    public class Analyzer
    {
        public int BlankLoc;
        public int PhisicLoc;
        public int LogicLoc;
        public int CommentLoc;
        public void Read()
        {
            string zipPath = @"D:\!Kvitka\training\Euro_diffusion.zip";
            using (ZipArchive archive = ZipFile.OpenRead(zipPath))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    if (entry.FullName.EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
                    {
                        using (var reader = new StreamReader(entry.Open()))
                        {
                            while (reader.Peek() >= 0)
                            {
                                Analyze(reader.ReadLine());
                            }
                        }
                    }
                }
            }
        }

        public void Analyze(string str)
        {
            PhisicLoc++;
            if (String.IsNullOrWhiteSpace(str))
                BlankLoc++;
        }
    }
}
