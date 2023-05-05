using Microsoft.EntityFrameworkCore;
using Movie.Core.DTOs;
using Movie.Core.Models;
using Movie.Core.Repositories;
using Movie.Repository.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Repository.Repositories
{
    public class MovieRatingRepository : GenericRepository<MovieRating>, IMovieRatingRepository
    {

        public MovieRatingRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<MovieRating> GetByMovieIdAndUserIdAsync(int movieId, string userId)
        {
            return await _context.MovieRatings.FirstOrDefaultAsync(x => x.MovieId == movieId && x.UserId == userId);
        }
    }
}
