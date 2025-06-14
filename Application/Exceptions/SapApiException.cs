using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySapProject.Application.Exceptions
{
    public class SapApiException : Exception
    {
        public int StatusCode { get; }
        public string? ReasonPhrase { get; }
        public string? ResponseContent { get; }

        public SapApiException(string message, int statusCode, string? reasonPhrase = null, string? responseContent = null)
            : base(message)
        {
            StatusCode = statusCode;
            ReasonPhrase = reasonPhrase;
            ResponseContent = responseContent;
        }

        public override string ToString()
        {
            return $"SAP API Error: {Message} | StatusCode: {StatusCode} | Reason: {ReasonPhrase} | Response: {ResponseContent}";
        }
    }
}
