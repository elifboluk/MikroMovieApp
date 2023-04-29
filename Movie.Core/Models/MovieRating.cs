using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Core.Models
{
    public class MovieRating
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }

        [ForeignKey("Movie")]
        public int MovieId { get; set; }
        public virtual Movie Movie { get; set; }

        [ForeignKey("UserApp")]
        public string UserId { get; set; }
        public virtual UserApp User { get; set; }
        
        
    }
}
