using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.DataAccess.Models
{
    public class WatchlistItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public int RuntimeInMinutes { get; set; }

        [Required]
        public DateTime CreatedDateTime { get; set; }

        [Required]
        public string CreatedByUserId { get; set; }
    }
}
