namespace PubliEventos.Web.Helpers
{
    using System.Web.Mvc;

    public static class SocialLink
    {
        /// <summary>
        /// Comparte un evento en twitter.
        /// </summary>
        /// <param name="htmlHelper">Helper.</param>
        /// <param name="title">Titulo del evento.</param>
        /// <param name="eventId">Identificador del evento.</param>
        /// <param name="imageUrl">Path de la imagen.</param>
        /// <returns></returns>
        public static MvcHtmlString SharedTwitter(this HtmlHelper htmlHelper, string title, string eventId, string imageUrl)
        {
            //declare the html helper 
            var builder = new TagBuilder("a");
            var url = string.Format("{0}://{1}/Event/Detail/{2}", System.Web.HttpContext.Current.Request.Url.Scheme, System.Web.HttpContext.Current.Request.Url.Authority, eventId);
            var imgBuilder = new TagBuilder("img");

            builder.Attributes.Add("href", "https://twitter.com/share?text=" + title + " > ");
            builder.Attributes.Add("data-url", url);
            builder.Attributes.Add("rel", "canonical");
            builder.Attributes.Add("title", "Compartir en Twitter");
            imgBuilder.MergeAttribute("src", "/Content/themes/images/twitter.png");

            imgBuilder.Attributes.Add("class", "shared-social");
            builder.Attributes.Add("target=", "_blank");
            builder.InnerHtml = imgBuilder.ToString(TagRenderMode.SelfClosing); ;

            return new MvcHtmlString(builder.ToString());
        }
    }
}