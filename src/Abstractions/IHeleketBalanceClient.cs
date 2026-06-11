using Heleket.Balance.Responses;
using Heleket.Common;

namespace Heleket.Abstractions;

/// <summary>
/// Provides Heleket balance operations.
/// </summary>
public interface IHeleketBalanceClient
{
    /// <summary>
    /// Gets merchant balance information.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The Heleket response envelope containing balances.</returns>
    Task<HeleketResponse<BalanceResult>> GetAsync(CancellationToken cancellationToken = default);
}
