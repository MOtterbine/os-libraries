using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS.Interfaces
{
    public interface IChannel
    {
        string Name { get; set; }
        string Description { get; set; }
    }
}
