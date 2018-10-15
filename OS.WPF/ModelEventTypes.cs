using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS.WPF
{
    public enum ModelEventTypes
    {
        DebugLog,
        WarnLog,
        ErrorLog,
        InfoLog,
        DisplayMessage,
        Confirmation,
        FilePathRequest,
        Authenticate,
        ChangePassword,
        SetPassword,
        ExitApplication,
        LoginFailed,
        ProcessStarted,
        ProcessFinished
    }
}
