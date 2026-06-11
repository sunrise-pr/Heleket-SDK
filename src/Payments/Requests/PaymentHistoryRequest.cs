using Newtonsoft.Json;

namespace Heleket.Payments.Requests;

/// <summary>
/// Request body for retrieving Heleket payment history.
/// </summary>
public sealed class PaymentHistoryRequest
{
    /// <summary>Gets the start datetime filter in the format expected by Heleket.</summary>
    [JsonProperty("date_from")]
    public string? DateFrom { get; init; }

    /// <summary>Gets the end datetime filter in the format expected by Heleket.</summary>
    [JsonProperty("date_to")]
    public string? DateTo { get; init; }
}
