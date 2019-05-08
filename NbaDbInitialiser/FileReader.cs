using NbaDbInitialiser.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NbaDbInitialiser
{
    public class FileReader : INbaFileReader
    {
        private string FilePath { get; set; }

        public void AddFilePath(string path)
        {
            var current = Environment.CurrentDirectory;
            var filePath = Path.GetFullPath(Path.Combine(current, @"..\NbaDbInitialiser", path));

            if (File.Exists(filePath))
            {
                FilePath = filePath;
            }
            else
            {
                throw new FileNotFoundException($"No file located at {filePath}");
            }
        }

        public string[] ReadAllLines()
        {
            return File.ReadAllLines(FilePath);
        }
    }
}
