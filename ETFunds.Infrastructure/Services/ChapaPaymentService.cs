using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using ETFunds.Application.Dtos;
using ETFunds.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace ETFunds.Infrastructure.Services;

public class ChapaPaymentService(HttpClient _httpClient, IConfiguration _configuration) : IChapaPaymentService
{


    public async Task<ChapaPaymentResponse> InitializePaymentAsync(
        ChapaPaymentRequest request,
        CancellationToken cancellationToken)
    {
        var secretKey = _configuration["Chapa:SecretKey"];

        if (string.IsNullOrWhiteSpace(secretKey))
        {
            throw new InvalidOperationException(
                "Chapa secret key is not configured.");
        }

        var chapaRequest = new
        {
            amount = request.Amount.ToString("0.00"),
            currency = request.Currency,
            email = request.Email,
            first_name = request.FirstName,
            last_name = request.LastName,
            phone_number = request.PhoneNumber,
            tx_ref = request.TransactionReference,

            callback_url = " https://comic-flattered-moneywise.ngrok-free.dev/api/payments/chapa/callback",
            return_url = "http://localhost:4200/payment/success"
        };

        using var httpRequest = new HttpRequestMessage(
            HttpMethod.Post,
            "https://api.chapa.co/v1/transaction/initialize");

        httpRequest.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", secretKey);

        httpRequest.Content = JsonContent.Create(chapaRequest);

        var response = await _httpClient.SendAsync(
            httpRequest,
            cancellationToken);

        var responseBody = await response.Content.ReadAsStringAsync(
            cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return new ChapaPaymentResponse
            {
                Success = false,
                Message = $"Chapa request failed: {responseBody}"
            };
        }

        using var json = JsonDocument.Parse(responseBody);

        var root = json.RootElement;

        var status = root.TryGetProperty("status", out var statusElement)
            ? statusElement.GetString()
            : null;

        if (status != "success")
        {
            return new ChapaPaymentResponse
            {
                Success = false,
                Message = root.TryGetProperty("message", out var message)
                    ? message.GetString()
                    : "Chapa payment initialization failed."
            };
        }

        var data = root.GetProperty("data");

        var checkoutUrl = data.TryGetProperty("checkout_url", out var checkout)
            ? checkout.GetString()
            : null;

        var chapaReference = data.TryGetProperty("tx_ref", out var txRef)
            ? txRef.GetString()
            : null;

        return new ChapaPaymentResponse
        {
            Success = true,
            CheckoutUrl = checkoutUrl,
            ChapaReference = chapaReference
        };
    }

    public async Task<ChapaPaymentVerificationResponse> VerifyPaymentAsync(
        string transactionReference,
        CancellationToken cancellationToken)
    {
        var secretKey = _configuration["Chapa:SecretKey"];

        if (string.IsNullOrWhiteSpace(secretKey))
        {
            throw new InvalidOperationException(
                "Chapa secret key is not configured.");
        }

        var url =
            $"https://api.chapa.co/v1/transaction/verify/{transactionReference}";

        using var httpRequest = new HttpRequestMessage(
            HttpMethod.Get,
            url);

        httpRequest.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", secretKey);

        var response = await _httpClient.SendAsync(
            httpRequest,
            cancellationToken);

        var responseBody = await response.Content.ReadAsStringAsync(
            cancellationToken);

        // ADD THIS
        Console.WriteLine("========== CHAPA VERIFICATION ==========");
        Console.WriteLine($"HTTP Status: {response.StatusCode}");
        Console.WriteLine($"Response: {responseBody}");

        if (!response.IsSuccessStatusCode)
        {
            return new ChapaPaymentVerificationResponse
            {
                Success = false,
                Message = $"Chapa verification failed: {responseBody}"
            };
        }

        using var json = JsonDocument.Parse(responseBody);

        var root = json.RootElement;

        var status = root.TryGetProperty(
            "status",
            out var statusElement)
            ? statusElement.GetString()
            : null;

        Console.WriteLine($"Chapa status: {status}");

        if (status != "success")
        {
            return new ChapaPaymentVerificationResponse
            {
                Success = false,
                Status = status,
                Message = "Chapa transaction was not successful."
            };
        }

        var data = root.GetProperty("data");

        var txRef = data.TryGetProperty(
            "tx_ref",
            out var txRefElement)
            ? txRefElement.GetString()
            : null;

        var amount = data.TryGetProperty(
     "amount",
     out var amountElement)
     ? amountElement.GetDecimal()
     : 0;

        var currency = data.TryGetProperty(
            "currency",
            out var currencyElement)
            ? currencyElement.GetString()
            : null;

        var chapaReference = data.TryGetProperty(
    "reference",
    out var referenceElement)
    ? referenceElement.GetString()
    : null;

        return new ChapaPaymentVerificationResponse
        {
            Success = true,
            TransactionReference = txRef,
            Status = status,
            Amount = amount,
            Currency = currency,
            ChapaReference = chapaReference
        };
    }


}