using AutoMapper;
using Movie.Core.DTOs;
using Movie.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.API.Mapping
{// DTOMapper
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<MovieDto, Core.Models.Movie>().ReverseMap();
            CreateMap<MovieRatingDto, MovieRating>().ReverseMap();
            CreateMap<UserAppDto, UserApp>().ReverseMap();
        }
    }
}
