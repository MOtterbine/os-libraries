using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS.Helpers
{
    public static class TextFormatter
    {
        public static string Format(string text, params object [] parameters)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(text, parameters);
            return sb.ToString();
        }
    }
}
