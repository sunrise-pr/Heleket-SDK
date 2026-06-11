using System.Net;

namespace Heleket;

/// <summary>
/// Represents an HTTP-level Heleket API failure.
/// </summary>
public sealed class HeleketApiException : Exception
{
    /// <summary>
    /// Creates an exception from the HTTP status code and raw response body.
    /// </summary>
    /// <param name="statusCode">The non-success HTTP status code returned by Heleket.</param>
    /// <param name="responseBody">The raw response body returned by Heleket.</param>
    public HeleketApiException(HttpStatusCode statusCode, string responseBody)
        : base($"Heleket API request failed with HTTP {(int)statusCode}.")
    {
        StatusCode = statusCode;
        ResponseBody = responseBody;
    }

    /// <summary>
    /// Gets the non-success HTTP status code returned by Heleket.
    /// </summary>
    public HttpStatusCode StatusCode { get; }

    /// <summary>
    /// Gets the raw response body returned by Heleket.
    /// </summary>
    public string ResponseBody { get; }
}
