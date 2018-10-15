using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OS.Configuration
{
    public interface IConfigurationReport
    {
        Dictionary<string, string> GetConfigurationElements();
    }
}
