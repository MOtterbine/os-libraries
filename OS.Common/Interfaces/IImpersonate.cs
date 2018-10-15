using System;

namespace OS.Interfaces
{
    public interface IImpersonate : IDisposable
    {
        bool IsImpersonating { get; }
        bool StartImpersonation(string userName, string password, string domainName);
        void StopImpersonization();
    }
}


