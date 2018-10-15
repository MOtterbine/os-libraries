using System;
using System.Collections.Generic;

namespace OS.Application
{
    public interface IRuntimeArguments
    {
        string GetValue(string paramName);
        Dictionary<String, String> ArgumentsFound { get;  }
    }
}
