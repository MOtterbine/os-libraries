using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;


namespace OS.Web.Styles
{

    /// <summary>
    /// This class is typically not used, but left for transparency...There already is a class named 'LessTransform' in System.web.optimization.dll
    /// Presumably, that class is basic - like the following class, but we can modify this class and use it if needed.
    /// For now, we are not using this class.
    /// 
    /// *******PLEASE UPDATE COMMENT IF THIS CLASS ENDS UP BEING USED SOMEWHERE********
    /// 
    /// </summary>
    public class LessTransform : IBundleTransform
    {
        public void Process(BundleContext context, BundleResponse response)
        {
            response.Content = dotless.Core.Less.Parse(response.Content);
            response.ContentType = "text/css";
        }
    }
}