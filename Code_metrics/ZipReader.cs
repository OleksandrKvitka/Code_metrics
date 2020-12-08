using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Text;

namespace Code_metrics
{
    public static class ZipReader
    {
        public static void Read()
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
                                Console.WriteLine(reader.ReadLine());
                            }
                        }
                    }
                }
            }
        }
    }
}
