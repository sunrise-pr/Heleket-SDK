# Heleket.Sdk

.NET SDK for the MVP Heleket payment API surface:

- create payment invoices
- get payment information
- refund payments
- validate payment and payout webhook signatures

Non-MVP endpoint families such as static wallets, QR generation, payment history, payouts, transfers, discounts, exchange rates, balance, and reference methods are not implemented yet.

## Install

```powershell
dotnet add package Heleket.Sdk
```

## Plain C# Usage

```csharp
using Heleket;
using Heleket.Options;
using Heleket.Payments;

var client = new HeleketClient(new HeleketOptions
{
    MerchantId = "merchant-uuid",
    PaymentApiKey = "payment-api-key",
    PayoutApiKey = "optional-payout-api-key"
});
```

## ASP.NET Core DI

```csharp
using Heleket;
using Heleket.Extensions;

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

## Webhook Validation

Validate the raw request body before trusting parsed webhook fields.

```csharp
var rawBody = await new StreamReader(Request.Body).ReadToEndAsync();

if (!client.Webhooks.ValidatePayment(rawBody))
{
    return Results.BadRequest();
}

return Results.Ok();
```

For low-level usage, pass the API key explicitly:

```csharp
using Heleket.Services;

IHeleketWebhookValidator validator = new HeleketWebhookValidator(new HeleketSigner());
var isValid = validator.Validate(rawBody, paymentApiKey);
```
