using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movie.Core.DTOs;
using Movie.Core.Models;
using Movie.Core.Repositories;
using Movie.Core.Services;
using SharedLibrary.DTOs;
using System.Diagnostics.Tracing;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace Movie.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MovieRatingController : CustomBaseController
    {
        private IMovieRatingService _movieRatingService;
        private readonly IService<Core.Models.Movie> _movieService;
        private IMapper _imapper;

        public MovieRatingController(IMovieRatingService movieRatingService, IService<Core.Models.Movie> movieService, IMapper mapper)
        {
            _movieRatingService = movieRatingService;
            _movieService = movieService;
            _imapper = mapper;
        }
        [HttpPost]
        [Route("SaveMovieRating")]
        public async Task<IActionResult> SaveMovieRating(MovieRatingDto movieDto)
        {
            var check = _movieService.Where(x => x.Title == movieDto.MovieName || x.Original_Title == movieDto.MovieName).FirstOrDefault();
            if (check == null)
            {
                return BadRequest();
            }
            movieDto.MovieId = check.Id;
            var userandmovieCheck= await _movieRatingService.GetByMovieIdAndUserIdAsync(check.Id, movieDto.UserId);
            if (userandmovieCheck!=null)
            {
                userandmovieCheck.Comment = movieDto.Comment;
                userandmovieCheck.Rating = movieDto.Rating;
                return Ok(_imapper.Map<MovieRatingDto>(_movieRatingService.UpdateAsync(userandmovieCheck)));
            }
            return Ok(await _movieRatingService.AddAsync(_imapper.Map<MovieRating>(movieDto)));
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveMovieRating(int id)
        {
            var movie = await _movieRatingService.GetByIdAsync(id);
            if (movie==null)
            {
                return BadRequest();
            }
            await _movieRatingService.RemoveAsync(movie);
            return NoContent();       
        } 

        [HttpGet]
        [Route("get")]
        public async Task<IActionResult> GettAllMovieRating()
        {
            var deneme = HttpContext.User.Identities.FirstOrDefault().Claims.FirstOrDefault().Value; 
            var sndns= _movieRatingService.Where(x=>x.UserId==deneme).ToList(); 
            return Ok(_imapper.Map<IEnumerable<MovieRatingDto>>(sndns));
       

        }

    }
}
