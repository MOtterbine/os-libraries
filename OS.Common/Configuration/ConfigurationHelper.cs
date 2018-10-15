using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace OS.Configuration
{

    public static class ConfigurationHelper
    {
        private static object creationLock = new object();
        public static void InstantiateObject(string typeString, out object userObject, params object[] constructionParams)
        {
            lock (creationLock)
            {
                Type type = Type.GetType(typeString);
                if (type == null)
                {
                    throw new NullReferenceException(string.Format("Unable to create object of type '{0}'", typeString));
                }
                userObject = Activator.CreateInstance(type, constructionParams);
            }
        }
    }
}
