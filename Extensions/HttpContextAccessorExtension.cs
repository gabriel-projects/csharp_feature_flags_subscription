using UAParser;

namespace Api.GRRInnovations.FeatureFlags.Extensions
{
    public static class HttpContextAccessorExtension
    {
        public static string GetBrowserName(this IHttpContextAccessor httpContextAccessor)
        {
            var userAgent = httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString();

            if (string.IsNullOrEmpty(userAgent)) return "undefined";

            var parser = Parser.GetDefault();
            ClientInfo clientInfo = parser.Parse(userAgent);

            return clientInfo.UA.Family; // Ex: Chrome, Firefox, Edge
        }
    }
}
