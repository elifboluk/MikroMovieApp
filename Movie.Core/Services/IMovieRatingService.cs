using Movie.Core.DTOs;
using Movie.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Core.Services
{
    public interface IMovieRatingService : IService<MovieRating>
    {
        Task<MovieRating> GetByMovieIdAndUserIdAsync(int movieId, string userId);
    }
}
