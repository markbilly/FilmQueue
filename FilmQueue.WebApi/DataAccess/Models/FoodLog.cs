using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.DataAccess.Models
{
    public class FoodLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public long FoodId { get; set; }

        [Required]
        public DateTime CreatedDateTime { get; set; }
    }
}
