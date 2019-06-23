using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.DataAccess.Models
{
    public class WatchNextSelectionRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public long FilmId { get; set; }

        [Required]
        public DateTime SelectedDateTime { get; set; }

        public DateTime? ExpiredDateTime { get; set; }

        public bool? Watched { get; set; }
    }
}
