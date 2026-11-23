using ETFunds.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ETFunds.Api.Controllers;

[ApiController]
[Route("api/payments/chapa")]
public class ChapaPaymentController(
    IChapaPaymentService chapaPaymentService,
    IPaymentService paymentService)
    : ControllerBase
{
    [HttpGet("callback")]
    public async Task<IActionResult> Callback(
    [FromQuery(Name = "trx_ref")] string? transactionReference,
    CancellationToken cancellationToken)
    {
        Console.WriteLine("========== CHAPA CALLBACK ==========");
        Console.WriteLine($"Full URL: {Request.Path}{Request.QueryString}");
        Console.WriteLine($"trx_ref: {transactionReference}");

        if (string.IsNullOrWhiteSpace(transactionReference))
        {
            Console.WriteLine("ERROR: trx_ref is missing.");

            return BadRequest(new
            {
                message = "Transaction reference is missing."
            });
        }

        Console.WriteLine("Calling Chapa verification...");

        var verification =
            await chapaPaymentService.VerifyPaymentAsync(
                transactionReference,
                cancellationToken);

        Console.WriteLine(
            $"Verification success: {verification.Success}");

        Console.WriteLine(
            $"Verification status: {verification.Status}");

        Console.WriteLine(
            $"Verification message: {verification.Message}");

        if (!verification.Success)
        {
            return BadRequest(new
            {
                message = verification.Message ??
                          "Payment verification failed."
            });
        }

        Console.WriteLine("Processing payment...");

        await paymentService.ProcessSuccessfulPaymentAsync(
            transactionReference,
            cancellationToken);

        Console.WriteLine("Payment processed successfully.");

        return Ok(new
        {
            message = "Payment processed successfully."
        });
    }
    [HttpGet("status/{transactionReference}")]
    public async Task<IActionResult> GetStatus(
    string transactionReference,
    CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(transactionReference))
        {
            return BadRequest(new
            {
                message = "Transaction reference is required."
            });
        }

        var payment = await paymentService.GetPaymentStatusAsync(
            transactionReference,
            cancellationToken);

        if (payment == null)
        {
            return NotFound(new
            {
                message = "Payment not found."
            });
        }

        return Ok(payment);
    }
}