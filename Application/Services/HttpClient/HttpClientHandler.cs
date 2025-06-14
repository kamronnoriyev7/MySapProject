using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySapProject.Application.Services.HttpClient
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
            var routeId = await _sessionStorage.GetRouteIdAsync();

            if (!string.IsNullOrEmpty(sessionId) || !string.IsNullOrEmpty(routeId))
            {
                var cookieValue = new List<string>();
                if (!string.IsNullOrEmpty(sessionId))
                    cookieValue.Add($"B1SESSION={sessionId}");
                if (!string.IsNullOrEmpty(routeId))
                    cookieValue.Add($"ROUTEID={routeId}");

                request.Headers.Remove("Cookie");
                request.Headers.Add("Cookie", string.Join("; ", cookieValue));
            }

            return await base.SendAsync(request, cancellationToken);
        }

    }
}
