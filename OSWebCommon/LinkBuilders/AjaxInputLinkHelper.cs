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
    public static class AjaxInputLinkHelper
    {

        public static HtmlString InputActionLink(this AjaxHelper helper, string actionName, string controllerName, object routeValues, AjaxOptions ajaxOptions, object htmlAttributes = null)
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
            var link = helper.ActionLink(actionLinkText, actionName, controllerName, routeValues, ajaxOptions);
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
        /// <param name="ajaxOptions"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static HtmlString CustomLinkTextActionLink(this AjaxHelper helper, string linkTextMarkup, string actionName, string controllerName, object routeValues, AjaxOptions ajaxOptions, object htmlAttributes = null)
        {

            TagBuilder spanBuilder = new TagBuilder("span");
            spanBuilder.InnerHtml = linkTextMarkup.ToString();

            // create a known dummy string - THAT IS UNLIKELY TO ACTUALLY EXIST ANYWHERE ELSE
            string placeHolder = "a`a~b0+?99t4)3#@ln";

            // get the raw markup using the dummy string
            var link = helper.ActionLink(placeHolder, actionName, controllerName, routeValues, ajaxOptions, htmlAttributes);

            // finally replace the dummy string with the supplied markup string 'linkTextMarkup'....
            string sth = link.ToString().Replace(placeHolder, linkTextMarkup.ToString());

            return new MvcHtmlString(sth);

        }
    }
}
