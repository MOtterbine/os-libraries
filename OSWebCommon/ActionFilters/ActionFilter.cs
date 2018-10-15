using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace OS.Web.Filters
{
    public class AnimationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //filterContext.HttpContext.Trace.Write("(Logging Filter)Action Executing: " +
            //    filterContext.ActionDescriptor.ActionName);

            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {

            //if (filterContext.Exception != null)
            //    filterContext.HttpContext.Trace.Write("(Logging Filter)Exception thrown");


            base.OnActionExecuted(filterContext);
        }
    }

}