using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie.Core.Models;
using Movie.Core.Repositories;
using Movie.Core.UnifOfWorks;
using Movie.Repository.EntityFramework;
using Movie.Repository.Repositories;
using Newtonsoft.Json;

namespace Movie.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieDbController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Core.Models.Movie> _movieRepository;
        
        public MovieDbController(IUnitOfWork unitOfWork, IGenericRepository<Core.Models.Movie> movieRepository)
        {
            _unitOfWork = unitOfWork;
            _movieRepository = movieRepository;            
        }        

        [HttpGet]
        [Route("GetAll")]
        public async Task<string> GetAll()
        {
            string route = "https://api.themoviedb.org/3/movie/top_rated?api_key=4f903b108c7a754673ed6a616b15889a&page=";            

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            // Pass the handler to httpclient(from you are calling api)
            using (var client = new HttpClient(clientHandler))
            {
                int page = 1;
                const int maxPages = 5; // Toplam 500 kayıt için 5 sayfa

                while (page <= maxPages)
                {
                    string url = route + page;

                    // Build the request.
                    var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);

                    // Send the request and get response.
                    HttpResponseMessage response = await client.SendAsync(httpRequest).ConfigureAwait(false);
                    // Read response as a string.
                    var result = await response.Content.ReadAsStringAsync();

                    if (result != null && !String.IsNullOrEmpty(result))
                    {
                        var movieResults = JsonConvert.DeserializeObject<Movie.Core.Models.MovieResults>(result);

                        var movies = movieResults.Results.Select(movie => new Core.Models.Movie
                        {                            
                            Original_Language = movie.Original_Language,
                            Original_Title = movie.Original_Title,
                            Title = movie.Title,
                            Overview = movie.Overview,
                            Vote_Average = movie.Vote_Average,
                            Vote_Count = movie.Vote_Count,
                            Release_Date = movie.Release_Date
                        }).ToList();
                        
                        await _movieRepository.AddRangeAsync(movies);                       

                        await _unitOfWork.CommitAsync(); // Veritabanına kaydet.
                    }
                    page++;
                }
            }
            return "OK";
        } 
    }
}
// Gelen datalar json olarak geldiği için class objesine çevirilecek.
// Gelen class add işlemiyle database'e kaydet.


/*
        // GET: api/<TestController>
        [HttpGet]
        public async Task<string> Get()
        {

            string route = "https://api.themoviedb.org/3/movie/" + 551 + "?api_key=4f903b108c7a754673ed6a616b15889a";


            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            // Pass the handler to httpclient(from you are calling api)

            using (var client = new HttpClient(clientHandler))
            using (var httpRequest = new HttpRequestMessage())
            {
                // Build the request.
                httpRequest.Method = HttpMethod.Get;
                httpRequest.RequestUri = new Uri(route);


                // Send the request and get response.
                HttpResponseMessage response = await client.SendAsync(httpRequest).ConfigureAwait(false);
                // Read response as a string.
                var result = await response.Content.ReadAsStringAsync(); // middleware apiden dönecek ve sonucu burda göstereceğiz

                if (result != null && !String.IsNullOrEmpty(result))
                {
                    var res = JsonConvert.DeserializeObject<Movie.Core.Models.Movie>(result);
                }
            }
            return "OK";
        }
        */


