using Heleket.Payments;
using Newtonsoft.Json;
using Xunit;

namespace Heleket.Tests;

public sealed class PaymentStatusTests
{
    [Fact]
    public void KnownStatus_Deserializes()
    {
        var dto = JsonConvert.DeserializeObject<StatusDto>("{\"status\":\"paid\"}");

        Assert.Equal(PaymentStatus.Paid, dto!.Status);
        Assert.Equal("paid", dto.Status.Value);
    }

    [Fact]
    public void UnknownStatus_PreservesRawValue()
    {
        var dto = JsonConvert.DeserializeObject<StatusDto>("{\"status\":\"future_status\"}");

        Assert.Equal("future_status", dto!.Status.Value);
    }

    [Fact]
    public void Status_SerializesAsRawString()
    {
        var json = JsonConvert.SerializeObject(new StatusDto { Status = PaymentStatus.PaidOver });

        Assert.Equal("{\"status\":\"paid_over\"}", json);
    }

    [Fact]
    public void Helpers_ClassifyKnownStatuses()
    {
        Assert.True(PaymentStatus.IsSuccessfulFinal(PaymentStatus.Paid, isFinal: true));
        Assert.True(PaymentStatus.PaidOver.IsSuccessfulFinal(isFinal: true));
        Assert.False(PaymentStatus.Paid.IsSuccessfulFinal(isFinal: false));
        Assert.True(PaymentStatus.Cancel.IsFailure());
        Assert.True(PaymentStatus.RefundPaid.IsRefund());
        Assert.False(new PaymentStatus("future_status").IsFailure());
    }

    private sealed class StatusDto
    {
        [JsonProperty("status")]
        public PaymentStatus Status { get; init; }
    }
}
