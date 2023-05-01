using Movie.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Core.DTOs
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Original_Language { get; set; }
        public string Original_Title { get; set; }
        public string Title { get; set; }
        public string Overview { get; set; }
        public double Vote_Average { get; set; }
        public int Vote_Count { get; set; }
        public DateTime Release_Date { get; set; }
        public ICollection<MovieRating> Ratings { get; set; }
    }
}
