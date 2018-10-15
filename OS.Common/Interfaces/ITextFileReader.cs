using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS.Interfaces
{
    public interface ITextFileReader
    {
        bool Read(string filePath);
        List<string> Lines { get; }
    }
}
