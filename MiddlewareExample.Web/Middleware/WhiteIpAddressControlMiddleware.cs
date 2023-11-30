using System.Net;

namespace MiddlewareExample.Web.Middleware
{
    public class WhiteIpAddressControlMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        private const string WhiteIpAddress = "::1";

        public WhiteIpAddressControlMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //IPV4 ==> 127.0.0.1 =>localhost
            //IPV6 ==> ::1 => localhost

            var reqIpAddress = context.Connection.RemoteIpAddress;

            bool anyWhiteIpAddress = IPAddress.Parse(WhiteIpAddress).Equals(reqIpAddress);

            if (anyWhiteIpAddress)
            {
                await _requestDelegate(context); // next()
            }else
            {
                context.Response.StatusCode = HttpStatusCode.Forbidden.GetHashCode();
                await context.Response.WriteAsync("Forbidden");
            }
        }
    }
}
