using Heleket.Abstractions;
using Heleket.Balance.Responses;
using Heleket.Common;
using Heleket.Internal;

namespace Heleket.Client;

internal sealed class HeleketBalanceClient : IHeleketBalanceClient
{
    private readonly HeleketHttpSender _sender;

    public HeleketBalanceClient(HeleketHttpSender sender)
    {
        _sender = sender;
    }

    /// <inheritdoc />
    public Task<HeleketResponse<BalanceResult>> GetAsync(CancellationToken cancellationToken = default)
    {
        return _sender.PostPaymentAsync<BalanceResult>("balance", EmptyRequest.Instance, cancellationToken);
    }
}
