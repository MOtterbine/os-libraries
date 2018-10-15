using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OS.Helpers
{
    public static class ExceptionReader
    {
        /// <summary>
        /// Returns a string that reprensents a recursive inner exception search on the exception passed in
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string GetFullExceptionMessage(Exception ex)
        {
            if(ex.InnerException == null)
            {
                return ex.Message;
            }
             return OS.Helpers.TextFormatter.Format("{0}, {1}", ex.Message, GetFullExceptionMessage(ex.InnerException));
            //return GetFullExceptionMessage(ex.InnerException);
        }
    }
}