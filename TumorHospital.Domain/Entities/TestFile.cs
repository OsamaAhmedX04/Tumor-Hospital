using System.ComponentModel.DataAnnotations;

namespace TumorHospital.Domain.Entities
{
    public class TestFile
    {
        [Key]
        public int Id { get; set; }
        public string ImageURL { get; set; }
    }
}
