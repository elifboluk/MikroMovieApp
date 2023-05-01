using Movie.Core.Repositories;
using Movie.Core.Services;
using Movie.Core.UnifOfWorks;
using Movie.Service.Mapping;
using SharedLibrary.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Service.Services
{
    public class Service<T, TDto> : IService<T, TDto> where T : class where TDto : class
    {
        private readonly IGenericRepository<T> _genericRepository;
        private readonly IUnitOfWork _unitOfWork;

        public Service(IGenericRepository<T> genericRepository, IUnitOfWork unitOfWork)
        {
            _genericRepository = genericRepository;
            _unitOfWork = unitOfWork;
        }


        public async Task<Response<TDto>> AddAsync(TDto entity)
        {
            var newEntity = ObjectMapper.Mapper.Map<T>(entity); // Gelen Dto'yu Mapper üzerinden entity'e çeviriyorum.
            await _genericRepository.AddAsync(newEntity);
            await _unitOfWork.CommitAsync(); // Veritabanına yansıtıldı.

            // Geriye tekrar Dto döneceğiz.↓
            var newDto = ObjectMapper.Mapper.Map<TDto>(newEntity); // Entity'i Dto'ya çeviriyorum.
            return Response<TDto>.Success(newDto, 200);
        }

        public async Task<Response<IEnumerable<TDto>>> GetAllAsync()
        {
            var movies = ObjectMapper.Mapper.Map<List<TDto>>(await _genericRepository.GetAllAsync()); // Unutma: GetAllAsync, IEnumerable tipinde.
            return Response<IEnumerable<TDto>>.Success(movies, 200); // Status code ile datayı döndüm.            
        }

        public async Task<Response<TDto>> GetByIdAsync(int id)
        {
            var movie = await _genericRepository.GetByIdAsync(id); // entity
            if (movie == null) // null ise;
            {
                return Response<TDto>.Fail($"{typeof(T).Name}({id}) not found.",404,true); // 404 not found.
            }
            return Response<TDto>.Success(ObjectMapper.Mapper.Map<TDto>(movie), 200); // Dto'ya dönüştürüldü, status code ile datayı döndüm.
        }

        public Task<Response<NoDataDto>> Remove(TDto entity)
        {
            throw new NotImplementedException();
        }

        public Task<Response<NoDataDto>> Update(TDto entity)
        {
            throw new NotImplementedException();
        }

        public Task<Response<IEnumerable<TDto>>> Where(Expression<Func<T, bool>> expression)
        {
            throw new NotImplementedException();
        }
    }
}
