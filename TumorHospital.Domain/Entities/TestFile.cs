using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TumorHospital.Domain.Entities
{
    public class TestFile
    {
        [Key]
        public int Id { get; set; }
        public string ImageURL { get; set; }
    }
}
