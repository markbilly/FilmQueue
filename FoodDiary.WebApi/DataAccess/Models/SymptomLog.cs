using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDiary.WebApi.DataAccess.Models
{
    public class SymptomLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public long SymptomId { get; set; }

        [Required]
        public SymptomSeverity Severity { get; set; }

        [Required]
        public DateTime CreatedDateTime { get; set; }
    }

    public enum SymptomSeverity
    {
        Mild = 1,
        Moderate = 2,
        Severe = 3
    }
}
