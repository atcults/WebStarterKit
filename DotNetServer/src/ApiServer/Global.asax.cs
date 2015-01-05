using System;
using System.Text.RegularExpressions;

namespace WebApp
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private static readonly Regex WwwRegex = new Regex(@"www\.(?<mainDomain>.*)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
     
        protected void Application_Start()
        {
            //No code required
        }

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            var hostName = Request.Headers["x-forwarded-host"];
            hostName = string.IsNullOrEmpty(hostName) ? Request.Url.Host : hostName;
            var match = WwwRegex.Match(hostName);
            if (!match.Success) return;

            var mainDomain = match.Groups["mainDomain"].Value;
            var builder = new UriBuilder(Request.Url)
            {
                Host = mainDomain
            };
            var redirectUrl = builder.Uri.ToString();
            Response.Clear();
            Response.StatusCode = 301;
            Response.StatusDescription = "Moved Permanently";
            Response.AddHeader("Location", redirectUrl);
            Response.End();
        }
    }
}