using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace OS.Configuration
{

    public class ConfigurationManager
    {




        /// <summary>
        /// This method reads app settings the local app.config file for the calling assembly/dll"/>
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="obj">valid return value if returned true</param>
        /// <returns>true if value exists</returns>
        public static bool GetDllConfigAppSetting(string setting, out object obj)
        {
            try
            {
                // The dllPath can't just use Assembly.GetExecutingAssembly().Location as ASP.NET doesn't copy the config to shadow copy path
                var dllPath = new Uri(System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase).LocalPath;
                var dllConfig = System.Configuration.ConfigurationManager.OpenExeConfiguration(dllPath);

                // Get the appSettings section
                var appSettings = (System.Configuration.AppSettingsSection)dllConfig.GetSection("appSettings");
                System.Configuration.KeyValueConfigurationElement element = appSettings.Settings[setting];
                obj = element.Value;
                return true;
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException(string.Format("ConfigurationManager.GetDllConfigAppSetting(...) - error attempting to get value from dll 'appSettings' section , param:{0} - {1}", setting, ex.Message), ex);
            }
        }

        /// <summary>
        /// This method reads a connection string setting for the calling assembly/dll"
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="obj">valid return value if returned true</param>
        /// <returns>true if value exists</returns>
        public static bool GetDllConnectionString(string setting, out string connString)
        {
            try
            {
                // The dllPath can't just use Assembly.GetExecutingAssembly().Location as ASP.NET doesn't copy the config to shadow copy path
                var dllPath = new Uri(System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase).LocalPath;
                var dllConfig = System.Configuration.ConfigurationManager.OpenExeConfiguration(dllPath);


                var connStrings = (System.Configuration.ConnectionStringsSection)dllConfig.GetSection("connectionStrings");
                connString = connStrings.ConnectionStrings[setting].ConnectionString;
                //  ConnectionStrings.ConnectionStrings[setting];
                // Get the appSettings section
                //   var connStrings = dllConfig.ConnectionStrings[setting];
                //   connString = (String)connStrings[setting];
                return true;
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException(string.Format("ConfigurationManager.GetDllConnectionString(...) - error attempting to get value from dll 'connectionstrings' section , param:{0} - {1}", setting, ex.Message), ex);
            }
        }

        /// <summary>
        /// This method reads app settings the local app.config file for the calling assembly/dll"/>
        /// </summary>
        /// <param name="setting"></param>
        /// <returns>true if value exists</returns>
        public static object GetMainAppSetting(string setting)
        {
            object obj = null;
            try
            {
                if (System.Configuration.ConfigurationManager.AppSettings.Count > 0)
                {
                    string configValue = System.Configuration.ConfigurationManager.AppSettings[setting];
                    if (configValue != null)
                    {
                        obj = configValue;
                        return obj;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Format("An error occured while attempting to read main app configuration parameter '{0}' - {1}", setting, ex.Message));
            }
            return null;
        }


    }
}
