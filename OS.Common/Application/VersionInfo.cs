using System.Reflection;

namespace OS.Application
{
    public static class VersionInfo
    {
        public static string AppName
        {
            get
            {
                return Assembly.GetCallingAssembly().GetName().Name;
            }
        }
        public static string AppVersion
        {
            get
            {
                return Assembly.GetCallingAssembly().GetName().Version.ToString();
            }
        }

    }
}
