using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TumorHospital.Application.DTOs.Request.Payment
{
    /// <summary>
    /// Webhook payload data
    /// </summary>
    public class WebhookPayload
    {
        public string OrderId { get; set; }
    }

    /// <summary>
    /// Webhook model for successful payment notifications
    /// </summary>
    public class WebHookModel
    {
        [JsonPropertyName("invoice_id")]
        public long InvoiceId { get; set; }

        [JsonPropertyName("invoice_key")]
        public string InvoiceKey { get; set; }

        [JsonPropertyName("hashKey")]
        public string HashKey { get; set; }

        [JsonPropertyName("payment_method")]
        public string PaymentMethod { get; set; }

        [JsonPropertyName("invoice_status")]
        public string InvoiceStatus { get; set; }

        [JsonPropertyName("pay_load")]
        public string? PayloadString { get; set; }
    }


    /// <summary>
    /// Webhook model for cancelled or failed transactions
    /// </summary>
    public class CancelTransactionModel
    {
        /// <summary>
        /// Verification hash key
        /// </summary>
        [JsonPropertyName("hashKey")]
        [Required]
        public string HashKey { get; set; }

        /// <summary>
        /// Transaction reference ID
        /// </summary>
        [JsonPropertyName("referenceId")]
        [Required]
        public string ReferenceId { get; set; }

        /// <summary>
        /// Transaction status
        /// </summary>
        [JsonPropertyName("status")]
        [Required]
        public string Status { get; set; }

        /// <summary>
        /// Payment method used for the transaction
        /// </summary>
        [JsonPropertyName("paymentMethod")]
        [Required]
        public string PaymentMethod { get; set; }

        /// <summary>
        /// Additional payload data
        /// </summary>
        [JsonPropertyName("pay_load")]
        public object? PayLoad { get; set; }
    }

    /// <summary>
    /// Failed webhook model (form-encoded)
    /// </summary>
    public class FaliledWebHook
    {
        /// <summary>
        /// Invoice ID from failed transaction
        /// </summary>
        [FromForm(Name = "invoice_id")]
        [Required]
        public long InvoiceId { get; set; }

        /// <summary>
        /// Invoice key from failed transaction
        /// </summary>
        [FromForm(Name = "invoice_key")]
        [Required]
        public string InvoiceKey { get; set; }

        /// <summary>
        /// Error message describing the failure
        /// </summary>
        [FromForm(Name = "errorMessage")]
        [Required]
        public string ErrorMessage { get; set; }
    }
}
