using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie.Core.DTOs;
using Movie.Core.Models;
using Movie.Core.Repositories;
using Movie.Core.Services;
using Movie.Core.UnifOfWorks;
using SharedLibrary.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Service.Services
{
    public class MovieRatingService : Service<MovieRating>, IMovieRatingService
    {
        private readonly IMovieRatingRepository _movieRatingRepository;

        public MovieRatingService(IGenericRepository<MovieRating> genericRepository, IUnitOfWork unitOfWork) : base(genericRepository, unitOfWork)
        {

        }

        public async  Task<MovieRating> GetByMovieIdAndUserIdAsync(int movieId, string userId)
        {
            var query = await _movieRatingRepository.Where(x => x.MovieId == movieId && x.UserId == userId).FirstOrDefaultAsync();
            return query;
        }



        //[HttpPost("{movieId}/rate")]
        //public async Task<IActionResult> AddRating(int movieId, [FromBody] MovieRatingDto ratingDto)
        //{
        //    if (ratingDto == null)
        //    {
        //        return Response<MovieRatingDto>.Fail("RatingDto is null");
        //    }

        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    if (string.IsNullOrEmpty(userId))
        //    {
        //        return Unauthorized();
        //    }

        //    var existingRating = await _movieRatingService.GetByMovieIdAndUserIdAsync(movieId, userId);
        //    if (existingRating != null)
        //    {
        //        return Conflict($"User has already rated the movie with id {movieId}");
        //    }

        //    ratingDto.MovieId = movieId;
        //    ratingDto.UserId = userId;

        //    var response = await _movieRatingService.AddAsync(ratingDto);
        //    return StatusCode(response.StatusCode, response);
        //}

        //public Task<MovieRatingDto> GetByMovieIdAndUserIdAsync(int movieId, string userId)
        //{
        //    throw new NotImplementedException();
        //}

        //[HttpPut("{movieId}/rate")]
        //public async Task<IActionResult> UpdateRating(int movieId, [FromBody] MovieRatingDto ratingDto)
        //{
        //    if (ratingDto == null)
        //    {
        //        return BadRequest("RatingDto is null");
        //    }

        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    if (string.IsNullOrEmpty(userId))
        //    {
        //        return Unauthorized();
        //    }

        //    var existingRating = await _movieRatingService.GetByMovieIdAndUserIdAsync(movieId, userId);
        //    if (existingRating == null)
        //    {
        //        return NotFound($"User has not rated the movie with id {movieId}");
        //    }

        //    if (existingRating.Id != ratingDto.Id)
        //    {
        //        return BadRequest("RatingDto id does not match the existing rating id");
        //    }

        //    var response = await _movieRatingService.UpdateAsync(ratingDto);
        //    return StatusCode(response.StatusCode, response);
        //}

    }
}
