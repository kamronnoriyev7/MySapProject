using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySapProject.Application.Services
{
    public class HttpClientHandler : DelegatingHandler
    {
        private readonly ISessionStorage _sessionStorage;

        public HttpClientHandler(ISessionStorage sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var sessionId = await _sessionStorage.GetSessionIdAsync();
            if (!string.IsNullOrEmpty(sessionId))
            {
                request.Headers.Add("Cookie", $"B1SESSION={sessionId}");
            }

            var routeId = await _sessionStorage.GetRouteIdAsync();
            if (!string.IsNullOrEmpty(routeId))
            {
                request.Headers.Add("Cookie", $"ROUTEID={routeId}");
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
