using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Core.DTOs
{
    public class MovieRatingDto
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        [Range(1,10)]
        public int Rating { get; set; }
        public int MovieId { get; set; }        
        public string UserId { get; set; }
    }
}
