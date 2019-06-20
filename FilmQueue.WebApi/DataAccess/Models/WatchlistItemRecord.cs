using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.DataAccess.Models
{
    public class WatchlistItemRecord
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

        public DateTime? WatchNextStart { get; set; }
        public DateTime? WatchNextEnd { get; set; }

        public DateTime? WatchedDateTime { get; set; }
    }
}
