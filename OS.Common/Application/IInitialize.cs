using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS.Application
{
    public delegate void CreationComplete();
    public interface IInitialize
    {
        bool ReadyForUse { get; }
        event CreationComplete Created;
        void Initialize();
    }
}
