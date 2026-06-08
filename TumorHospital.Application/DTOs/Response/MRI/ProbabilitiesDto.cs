using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TumorHospital.Application.DTOs.Response.MRI
{
    public class ProbabilitiesDto
    {
        [JsonPropertyName("glioma")]
        public decimal Glioma { get; set; }

        [JsonPropertyName("meningioma")]
        public decimal Meningioma { get; set; }

        [JsonPropertyName("notumor")]
        public decimal Notumor { get; set; }

        [JsonPropertyName("pituitary")]
        public decimal Pituitary { get; set; }
    }
}
