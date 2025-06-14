using MySapProject.Application.Exceptions;

namespace MySapProject.Infrastructure.Extensions;

public static class HttpResponseMessageExtensions
{
    public static async Task EnsureSuccessOrThrowAsync(this HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new SapApiException(
                message: $"Request to SAP API failed.",
                statusCode: (int)response.StatusCode,
                reasonPhrase: response.ReasonPhrase,
                responseContent: content
            );
        }
    }
}


