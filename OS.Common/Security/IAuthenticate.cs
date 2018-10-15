using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS.Security
{
    public interface IAuthenticate
    {
        string UserName { get; }
        bool AuthenticationSuccessful { get; }
        void SubmitCredentials(string userName, string passWord);
        void SubmitPasswordChange(string userName, string currentPassword, string newPassword);

    }
}
