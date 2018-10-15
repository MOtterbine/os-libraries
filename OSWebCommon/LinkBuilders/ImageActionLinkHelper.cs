using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Razor;

namespace OS.Web.LinkBuilders
{
    public static class ImageActionLinkHelper
    {

        public static HtmlString ImageActionLink(this AjaxHelper helper, string actionName, string controllerName, object routeValues, AjaxOptions ajaxOptions, object htmlAttributes = null, object imgAttributes = null)
        {
            IDictionary<string, object> attributespp = new RouteValueDictionary(imgAttributes);

            // Build up the <img></img> tag...
            var builder = new TagBuilder("img");
            foreach (KeyValuePair<string, object> kvp in attributespp)
            {
                builder.MergeAttribute(kvp.Key, kvp.Value as string);
            }

            string actionLinkText = "NotUsedForPics";

            var link = helper.ActionLink(actionLinkText, actionName, controllerName, routeValues, ajaxOptions);

            string sth = link.ToString().Replace(actionLinkText, builder.ToString(TagRenderMode.SelfClosing));

            return new HtmlString(sth);
        }
        public static HtmlString ImageActionLink(this HtmlHelper helper, string actionName, string controllerName, object routeValues, object htmlAttributes = null, object imgAttributes = null)
        {
            IDictionary<string, object> attributespp = new RouteValueDictionary(imgAttributes);

            // Build up the <img></img> tag...
            var builder = new TagBuilder("img");
            foreach (KeyValuePair<string, object> kvp in attributespp)
            {
                builder.MergeAttribute(kvp.Key, kvp.Value as string);
            }

            string actionLinkText = "NotUsedForPics";




            var link = helper.InputActionLink(actionName, controllerName, routeValues, htmlAttributes);

            string sth = link.ToString().Replace(actionLinkText, builder.ToString(TagRenderMode.SelfClosing));

            return new HtmlString(sth);
        }

    }
}
