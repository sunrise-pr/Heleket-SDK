using Newtonsoft.Json;

namespace Heleket.Common;

/// <summary>
/// Represents a cursor-paginated Heleket result page.
/// </summary>
/// <typeparam name="T">The page item type.</typeparam>
public sealed class HeleketPage<T>
{
    /// <summary>Gets the page items.</summary>
    [JsonProperty("items")]
    public IReadOnlyList<T> Items { get; init; } = Array.Empty<T>();

    /// <summary>Gets pagination metadata.</summary>
    [JsonProperty("paginate")]
    public HeleketPagination? Pagination { get; init; }
}

/// <summary>
/// Represents Heleket cursor pagination metadata.
/// </summary>
public sealed class HeleketPagination
{
    /// <summary>Gets the number of items in the page.</summary>
    [JsonProperty("count")]
    public int Count { get; init; }

    /// <summary>Gets whether more pages are available.</summary>
    [JsonProperty("hasPages")]
    public bool HasPages { get; init; }

    /// <summary>Gets the next page cursor.</summary>
    [JsonProperty("nextCursor")]
    public string? NextCursor { get; init; }

    /// <summary>Gets the previous page cursor.</summary>
    [JsonProperty("previousCursor")]
    public string? PreviousCursor { get; init; }

    /// <summary>Gets the page size.</summary>
    [JsonProperty("perPage")]
    public int PerPage { get; init; }
}
