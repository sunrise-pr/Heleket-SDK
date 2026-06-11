using Heleket.Abstractions;
using Heleket.Common;
using Heleket.Internal;
using Heleket.Payments.Requests;
using Heleket.Payments.Responses;

namespace Heleket.Client;

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

    /// <inheritdoc />
    public Task<HeleketResponse<IReadOnlyList<PaymentService>>> GetServicesAsync(
        CancellationToken cancellationToken = default)
    {
        return _sender.PostPaymentAsync<IReadOnlyList<PaymentService>>(
            "payment/services",
            EmptyRequest.Instance,
            cancellationToken);
    }

    /// <inheritdoc />
    public Task<HeleketResponse<HeleketPage<HeleketPayment>>> GetHistoryAsync(
        PaymentHistoryRequest request,
        string? cursor = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return _sender.PostPaymentAsync<HeleketPage<HeleketPayment>>(
            "payment/list",
            request,
            cancellationToken,
            cursor);
    }

    /// <inheritdoc />
    public Task<HeleketResponse<ResendPaymentWebhookResult>> ResendWebhookAsync(
        ResendPaymentWebhookRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return _sender.PostPaymentAsync<ResendPaymentWebhookResult>(
            "payment/resend-webhook",
            request,
            cancellationToken);
    }

    /// <inheritdoc />
    public Task<HeleketResponse<TestPaymentWebhookResult>> SendTestWebhookAsync(
        TestPaymentWebhookRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return _sender.PostPaymentAsync<TestPaymentWebhookResult>(
            "payment/test-webhook",
            request,
            cancellationToken);
    }
}
