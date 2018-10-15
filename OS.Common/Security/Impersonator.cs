using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
using OS.Interfaces;


namespace OS.Security
{
    public class Impersonator : IDisposable, IImpersonate, IReportInfo
    {

        //protected ILog _Logger = log4net.LogManager.GetLogger(typeof(Impersonator));

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword, int dwLogonType, int dwLogonProvider, out SafeTokenHandle phToken);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public extern static bool CloseHandle(IntPtr handle);

        /// <summary>
        ///  Constructor meant to be used within a 'using' block
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="domainName"></param>
        public Impersonator(string userName, string password, string domainName)
        {
            this.StartImpersonation(userName, password, domainName);
        }

        public Impersonator()
        {
        }

        public WindowsImpersonationContext _ImpersonatedUser { get; set; }

        public bool StartImpersonation(string userName, string password, string domainName)
        {
            //!!!!!!!!!!!!!!!!! DO NOT LOG, DISPLAY OR ALLOW CLIENT ACCESS TO USER NAME AND/OR PASSWORD !!!!!!!!!!!!!!!!!!!!!!!!!!

            if (string.IsNullOrEmpty(userName))
            {
                this.InfoText = "StartImpersonation() - userName is empty or null";
                return false;
            }
            if (string.IsNullOrEmpty(password))
            {
                this.InfoText = "StartImpersonation() - password is empty or null";
                return false;
            }
            if (string.IsNullOrEmpty(domainName))
            {
                this.InfoText = "StartImpersonation() - domainName is empty or null";
                return false;
            }

            this.InfoText = string.Format("StartImpersonation() - NT User: {0}", WindowsIdentity.GetCurrent().Name);
            SafeTokenHandle safeTokenHandle;

            try
            {
                // Get the user token for the specified user, domain, and password using the unmanaged LogonUser method.
                // The local machine name can be used for the domain name to impersonate a user on this machine.

                const int LOGON32_PROVIDER_DEFAULT = 0;
                //This parameter causes LogonUser to create a primary token.
                const int LOGON32_LOGON_INTERACTIVE = 2;

                //!!!!!!!!!!!!!!!!! DO NOT LOG, DISPLAY OR ALLOW CLIENT ACCESS TO USER NAME AND/OR PASSWORD !!!!!!!!!!!!!!!!!!!!!!!!!!

                // Call LogonUser to obtain a handle to an access token.
                bool returnValue = LogonUser(userName, domainName, password,
                    LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT,
                    out safeTokenHandle);


                if (false == returnValue)
                {
                    int ret = Marshal.GetLastWin32Error();
                    switch (ret)
                    {
                        case 87:

                            break;
                    }
                    this.InfoText = string.Format("StartImpersonation() - LogonUser failed with error code : {0}", ret);
                    return false;
                }
                using (safeTokenHandle)
                {
                 //   this.InfoText = string.Format("User {0} login {1}", userName, returnValue ? "Succeeded" : "Failed");
                  //  this.InfoText = string.Format("Windows NT token: {0}",safeTokenHandle);

                    // Check the 'pre' identity.
                    //this.InfoText = string.Format("NT Application Identity before impersonation: {0}", WindowsIdentity.GetCurrent().Name);


                    this._ImpersonatedUser = WindowsIdentity.Impersonate(safeTokenHandle.DangerousGetHandle());

                    // Check the 'post' or impersonated identity.
                    this.InfoText = string.Format("NT Application Identity after impersonation: {0}", WindowsIdentity.GetCurrent().Name);
                    return true;   
                
                
                
                    // Use the token handle returned by LogonUser.
                    //using (WindowsImpersonationContext impersonatedUser = WindowsIdentity.Impersonate(safeTokenHandle.DangerousGetHandle()))
                    //{
                    //    // Check the identity.
                    //    this._Logger.Info("After impersonation: " + WindowsIdentity.GetCurrent().Name);
                    //    retVal = SaveBrand(brand);
                    //}
                    // Releasing the context object stops the impersonation
                    // Check the identity.
                    //this._Logger.Info("After closing the context: " + WindowsIdentity.GetCurrent().Name);

                }
            }
            catch (Exception ex)
            {
                this.InfoText = string.Format("Exception occurred - {0}", Helpers.ExceptionReader.GetFullExceptionMessage(ex));
            }
            return false;
        }


        public void StopImpersonization()
        {
            if (this._ImpersonatedUser != null)
            {
                this._ImpersonatedUser.Dispose();
            }
            this.InfoText = string.Format("StopImpersonization() complete. Current NT User: {0}", WindowsIdentity.GetCurrent().Name);
        }

        public bool IsImpersonating
        {
            get
            {
                return this._ImpersonatedUser != null;
            }
        }

        public string InfoText { get; private set; }

        #region IDisposable Members

        private bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }
        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be disposed.
        // If disposing equals false, the method has been called by the
        // runtime from inside the finalizer and you should not reference
        // other objects. Only unmanaged resources can be disposed.
        private void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                    this.StopImpersonization();
                }
                // Note disposing has been done.
                disposed = true;
            }
        }
        ~Impersonator()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }

        #endregion IDisposable Members






    }
}