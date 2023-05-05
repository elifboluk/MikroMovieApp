using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movie.Core.DTOs;
using Movie.Core.Services;

namespace Movie.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : CustomBaseController
    {
        private readonly IService<Core.Models.Movie> _movieService;
        private IMapper _imapper;

        public MovieController(IService<Core.Models.Movie> movieService,IMapper mapper)
        {
            _movieService = movieService;
            _imapper = mapper;
        }
        [Route("GetMovies")]
        [HttpGet]
        public async Task<IActionResult> GetMovies()
        { 

            return Ok(_imapper.Map<IEnumerable<MovieDto>>(await _movieService.GetAllAsync()));
        }

        [HttpPost]
        public async Task<IActionResult> SaveMovie(MovieDto movieDto)
        {
            
            return Ok(await _movieService.AddAsync(_imapper.Map<Core.Models.Movie>(movieDto)));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMovie(MovieDto movieDto)
        {
            
            return Ok(_imapper.Map<MovieDto>(_movieService.UpdateAsync(_imapper.Map<Core.Models.Movie>(movieDto))));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            await _movieService.RemoveAsync(await _movieService.GetByIdAsync(id));
            return NoContent();
        }
    }
}
