using Heleket.Internal;

namespace Heleket.Payments;

internal sealed class HeleketPaymentsClient : IHeleketPaymentsClient
{
    private readonly HeleketHttpSender _sender;

    public HeleketPaymentsClient(HeleketHttpSender sender)
    {
        _sender = sender;
    }

    /// <inheritdoc />
    public Task<HeleketResponse<HeleketPayment>> CreateAsync(
        CreatePaymentRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return _sender.PostPaymentAsync<HeleketPayment>("payment", request, cancellationToken);
    }

    /// <inheritdoc />
    public Task<HeleketResponse<HeleketPayment>> GetInfoAsync(
        GetPaymentInfoRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return _sender.PostPaymentAsync<HeleketPayment>("payment/info", request, cancellationToken);
    }

    /// <inheritdoc />
    public Task<HeleketResponse<RefundPaymentResponse>> RefundAsync(
        RefundPaymentRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return _sender.PostPaymentAsync<RefundPaymentResponse>("payment/refund", request, cancellationToken);
    }
}
