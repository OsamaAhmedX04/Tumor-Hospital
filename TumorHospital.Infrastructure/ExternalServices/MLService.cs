using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid.Helpers.Errors.Model;
using TumorHospital.Application.DTOs.Request.ML;
using TumorHospital.Application.DTOs.Response.ML;
using TumorHospital.Application.Intefaces.ExternalServices;
using TumorHospital.Application.Intefaces.UOW;
using TumorHospital.Domain.Constants;
using TumorHospital.Domain.Entities;
using TumorHospital.Infrastructure.Settings;

namespace TumorHospital.Infrastructure.ExternalServices
{
    public class MLService : IMLService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<MLService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly MLExternalApisSettings _settings;
        private readonly IFileService _fileService;

        public MLService(
            HttpClient httpClient,
            ILogger<MLService> logger,
            IUnitOfWork unitOfWork,
            IOptions<MLExternalApisSettings> options,
            IFileService fileService)
        {
            _httpClient = httpClient;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _settings = options.Value;
            _fileService = fileService;
        }

        public async Task<ChatResponseDto> ChatAsync(ChatRequestDto dto)
        {
            _logger.LogInformation(
                "Sending AI chat request | MessageLength: {Length}",
                dto.Message?.Length ?? 0);

            var json = JsonSerializer.Serialize(dto);

            var response = await _httpClient.PostAsync(
                _settings.ChatApi,
                new StringContent(json, Encoding.UTF8, "application/json"));

            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(
                    "AI chat request failed | StatusCode: {StatusCode} | Response: {Response}",
                    (int)response.StatusCode,
                    content);

                throw new Exception(content);
            }

            var result = JsonSerializer.Deserialize<ChatResponseDto>(
                content,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            _logger.LogInformation(
                "AI chat response received successfully");

            return result!;
        }

        public async Task<ExplainResponseDto> ExplainAsync(ExplainRequestDto dto)
        {
            var appointment = await _unitOfWork.Appointments.GetByIdAsync(dto.AppointmentId);

            if (appointment == null)
                throw new Exception("Appointment not found");

            _logger.LogInformation(
                "Sending MRI image for AI analysis | AppointmentId: {AppointmentId} ",
                dto.AppointmentId);

            using var formData = new MultipartFormDataContent();

            var stream = dto.Image.OpenReadStream();

            var imageContent = new StreamContent(stream);

            imageContent.Headers.ContentType =
                new MediaTypeHeaderValue(dto.Image.ContentType);

            formData.Add(
                imageContent,
                "image",
                dto.Image.FileName);

            formData.Add(
                new StringContent(false.ToString().ToLower()),
                "force");

            var response = await _httpClient.PostAsync(
                _settings.ExplainApi,
                formData);

            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(
                    "AI explain request failed | StatusCode: {StatusCode} | Response: {Response}",
                    (int)response.StatusCode,
                    content);

                throw new Exception(content);
            }

            var result = JsonSerializer.Deserialize<ExplainResponseDto>(
                content,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (result == null)
                throw new Exception("Invalid AI response");

            var filePath = "";

            var diagnostic = new Diagnostic
            {
                AppointmentId = dto.AppointmentId,

                ImageURL = filePath,

                PredictedClass =
                    result.Prediction.PredictedClass,

                ConfidenceScore =
                    result.Prediction.Confidence,

                GliomaProbability =
                    result.Prediction.Probabilities.Glioma,

                MeningiomaProbability =
                    result.Prediction.Probabilities.Meningioma,

                NoTumorProbability =
                    result.Prediction.Probabilities.Notumor,

                PituitaryProbability =
                    result.Prediction.Probabilities.Pituitary,

                CreatedAt = DateTime.Now
            };

            await _unitOfWork.Diagnostics.AddAsync(diagnostic);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation(
                "Diagnostic saved | DiagnosticId: {DiagnosticId} | AppointmentId: {AppointmentId} | Class: {Class} | Confidence: {Confidence}",
                diagnostic.Id,
                dto.AppointmentId,
                result.PredictedClass,
                result.Confidence);

            return result;
        }

        public async Task<DiagnosticResponseDto> GetDiagnosticByAppointmentIdAsync(Guid appointmentId)
        {
            _logger.LogInformation(
                "Retrieving diagnostic | AppointmentId: {AppointmentId}",
                appointmentId);

            var diagnostic = await _unitOfWork.Diagnostics
                .FirstOrDefaultAsync(d => d.AppointmentId == appointmentId);

            if (diagnostic == null)
            {
                _logger.LogWarning(
                    "Diagnostic not found | AppointmentId: {AppointmentId}",
                    appointmentId);

                throw new Exception("Diagnostic not found");
            }

            return new DiagnosticResponseDto
            {
                Id = diagnostic.Id,

                AppointmentId = diagnostic.AppointmentId,

                ImageURL = SupabaseConstants.PrefixSupaURL + diagnostic.ImageURL,

                ExplainResponseDto = new ExplainResponseDto
                {
                    PredictedClass = diagnostic.PredictedClass,

                    Confidence = diagnostic.ConfidenceScore,

                    Prediction = new PredictionDto
                    {
                        PredictedClass = diagnostic.PredictedClass,

                        Confidence = diagnostic.ConfidenceScore,

                        Probabilities = new ProbabilitiesDto
                        {
                            Glioma = diagnostic.GliomaProbability,

                            Meningioma = diagnostic.MeningiomaProbability,

                            Notumor = diagnostic.NoTumorProbability,

                            Pituitary = diagnostic.PituitaryProbability
                        }
                    }
                },

                CreatedAt = diagnostic.CreatedAt
            };
        }

    }
}