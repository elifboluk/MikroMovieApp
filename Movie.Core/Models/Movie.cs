using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Core.Models
{
    public class Movie
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
/*
"Id" özelliği, film için benzersiz bir kimlik belirler.
"Title" özelliği, film başlığını tutar.
"Overview" özelliği, filmin özetini tutar.
"PosterPath" özelliği, filmin poster resminin yolunu tutar.
"VoteAverage" özelliği, filmin ortalama puanını tutar.
"ReleaseDate" özelliği, filmin yayınlanma tarihini tutar.
"Ratings" özelliği, "MovieRating" sınıfındaki gibi, bir film için verilen puan ve notları içeren bir liste oluşturur. 
*/
