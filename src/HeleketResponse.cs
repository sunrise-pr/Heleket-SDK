using Newtonsoft.Json;

namespace Heleket;

/// <summary>
/// Represents the standard Heleket API response envelope.
/// </summary>
/// <typeparam name="T">The typed result payload returned when the request succeeds.</typeparam>
public sealed class HeleketResponse<T>
{
    /// <summary>
    /// Gets the Heleket response state. A value of 0 indicates success.
    /// </summary>
    [JsonProperty("state")]
    public int State { get; init; }

    /// <summary>
    /// Gets the typed result payload when available.
    /// </summary>
    [JsonProperty("result")]
    public T? Result { get; init; }

    /// <summary>
    /// Gets the error message returned by Heleket when available.
    /// </summary>
    [JsonProperty("message")]
    public string? Message { get; init; }

    /// <summary>
    /// Gets field-level validation errors returned by Heleket when available.
    /// </summary>
    [JsonProperty("errors")]
    public Dictionary<string, string[]>? Errors { get; init; }

    /// <summary>
    /// Gets a value indicating whether <see cref="State"/> equals 0.
    /// </summary>
    public bool IsSuccess => State == 0;
}
