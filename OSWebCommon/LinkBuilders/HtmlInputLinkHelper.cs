using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.Web.Routing;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Razor;

namespace OS.Web.LinkBuilders
{
    public static class HtmlInputLinkHelper
    {

        public static HtmlString InputActionLink(this HtmlHelper helper, string actionName, string controllerName, object routeValues, object htmlAttributes = null)
        {
            IDictionary<string, object> attributespp = new RouteValueDictionary(htmlAttributes);

            // Build up the <input></input> tag...
            var builder = new TagBuilder("input");
            foreach (KeyValuePair<string, object> kvp in attributespp)
            {
                builder.MergeAttribute(kvp.Key, kvp.Value as string);
            }


            // need an unusual and known string for placeholding (later replacement)
            string actionLinkText = "A-3~!ffI(Y67:Ov`";

            RouteValueDictionary rc1 = new RouteValueDictionary(routeValues);
            RouteValueDictionary rc = new RouteValueDictionary();
            if(rc1 != null)
            {
                rc.Concat(rc1);
            }

            var link = UrlHelper.GenerateUrl("Default", actionName, controllerName, rc, null, null, true);
           // var link =  helper.InputActionLink(actionName, controllerName, routeValues, ajaxOptions);
            string sth = link.ToString().Replace(actionLinkText, builder.ToString(TagRenderMode.SelfClosing));
            return new HtmlString(sth);
        }


        /// <summary>
        /// Gives us the ability to setup an anchor (or hyperlink) with markup in the link text
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="linkTextMarkup"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="routeValues"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static HtmlString CustomLinkTextActionLink(this HtmlHelper helper, string linkTextMarkup, string actionName, string controllerName, object routeValues = null, object htmlAttributes = null)
        {

            TagBuilder anchorBuilder = new TagBuilder("a");

            // Setup the basic link 
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            var link = urlHelper.Action(actionName, controllerName, routeValues);
            // assign the link
            anchorBuilder.Attributes.Add("href", link);

            foreach (KeyValuePair<string, object> kvp in new RouteValueDictionary(htmlAttributes))
            {
                anchorBuilder.MergeAttribute(kvp.Key, kvp.Value as string);
            }

            // Insert 'linkTextMarkup'
            anchorBuilder.InnerHtml = linkTextMarkup.ToString();
            string sth = anchorBuilder.ToString();

            return new MvcHtmlString(sth);

        }



    }
}
