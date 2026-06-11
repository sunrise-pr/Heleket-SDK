[![NuGet](https://img.shields.io/nuget/v/Heleket.Sdk.svg)]

# Heleket.Sdk

.NET SDK for the MVP Heleket payment API surface:

- create payment invoices
- get payment information
- refund payments
- list payment services
- list payment history
- resend and test payment webhooks
- read balances
- validate payment and payout webhook signatures

Non-MVP endpoint families such as static wallets, QR generation, payouts, transfers, discounts, exchange rates, and reference methods are not implemented yet.

## Install

```powershell
dotnet add package Heleket.Sdk
```

## Plain C# Usage

```csharp
using Heleket.Client;
using Heleket.Configuration;
using Heleket.Payments.Requests;

var client = new HeleketClient(new HeleketOptions
{
    MerchantId = "merchant-uuid",
    PaymentApiKey = "payment-api-key",
    PayoutApiKey = "optional-payout-api-key"
});
```

## ASP.NET Core DI

```csharp
using Heleket.DependencyInjection;

builder.Services.AddHeleket(options =>
{
    options.MerchantId = builder.Configuration["Heleket:MerchantId"]!;
    options.PaymentApiKey = builder.Configuration["Heleket:PaymentApiKey"]!;
    options.PayoutApiKey = builder.Configuration["Heleket:PayoutApiKey"];
});

// Inject IHeleketClient where needed.
```

## Create Payment

```csharp
var response = await client.Payments.CreateAsync(new CreatePaymentRequest
{
    Amount = "50.00",
    Currency = "USD",
    OrderId = "order_123",
    UrlCallback = "https://example.com/webhooks/heleket"
}, cancellationToken);

if (!response.IsSuccess)
{
    Console.WriteLine(response.Message);
    return;
}

var invoiceUrl = response.Result?.Url;
```

## Get Payment Info

```csharp
var response = await client.Payments.GetInfoAsync(new GetPaymentInfoRequest
{
    OrderId = "order_123"
}, cancellationToken);

var status = response.Result?.Status ?? response.Result?.PaymentStatus;
```

## Refund Payment

```csharp
var response = await client.Payments.RefundAsync(new RefundPaymentRequest
{
    OrderId = "order_123",
    Address = "refund-wallet-address",
    IsSubtract = true,
    Amount = "50.00"
}, cancellationToken);
```

## Payment Services

```csharp
var response = await client.Payments.GetServicesAsync(cancellationToken);

if (response.IsSuccess)
{
    foreach (var service in response.Result ?? [])
    {
        Console.WriteLine($"{service.Currency} {service.Network}");
    }
}
```

## Payment History

```csharp
var response = await client.Payments.GetHistoryAsync(new PaymentHistoryRequest
{
    DateFrom = "2026-06-01 00:00:00",
    DateTo = "2026-06-11 23:59:59"
}, cursor: null, cancellationToken);

var nextCursor = response.Result?.Pagination?.NextCursor;
```

## Balance

```csharp
var response = await client.Balance.GetAsync(cancellationToken);
var balances = response.Result?.Balances;
```

## Operational Webhook Helpers

```csharp
await client.Payments.ResendWebhookAsync(new ResendPaymentWebhookRequest
{
    OrderId = "order_123"
}, cancellationToken);

await client.Payments.SendTestWebhookAsync(new TestPaymentWebhookRequest
{
    OrderId = "order_123",
    UrlCallback = "https://example.com/webhooks/heleket"
}, cancellationToken);
```

## Webhook Validation

Validate the raw request body before trusting parsed webhook fields.

```csharp
using Heleket.Webhooks.Models;
using Newtonsoft.Json;

var rawBody = await new StreamReader(Request.Body).ReadToEndAsync();

if (!client.Webhooks.ValidatePayment(rawBody))
{
    return Results.BadRequest();
}

var webhook = JsonConvert.DeserializeObject<HeleketPaymentWebhook>(rawBody);

if (webhook?.Status?.IsSuccessfulFinal(webhook.IsFinal == true) == true)
{
    // activate the order
}

return Results.Ok();
```

For low-level usage, pass the API key explicitly:

```csharp
using Heleket.Abstractions;
using Heleket.Signing;
using Heleket.Webhooks.Validation;

IHeleketWebhookValidator validator = new HeleketWebhookValidator(new HeleketSigner());
var isValid = validator.Validate(rawBody, paymentApiKey);
```
