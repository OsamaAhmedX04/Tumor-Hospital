using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TumorHospital.Domain.Entities
{
    public class MentalHealthSurvey
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Patient")]
        public string? PatientId { get; set; }
        public Patient? Patient { get; set; }
        public int AnxietyScore { get; set; } // between 0 and 10 
        public int DepressionScore { get; set; } // between 0 and 10 
        public int StressScore { get; set; } // between 0 and 10 
        public DateTime SurveyDate { get; set; }
    }
}
