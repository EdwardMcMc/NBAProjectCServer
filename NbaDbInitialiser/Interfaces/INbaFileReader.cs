using System;
using System.Collections.Generic;
using System.Text;

namespace NbaDbInitialiser.Interfaces
{
    public interface INbaFileReader
    {
        void AddFilePath(string path);
        string[] ReadAllLines();
    }
}
