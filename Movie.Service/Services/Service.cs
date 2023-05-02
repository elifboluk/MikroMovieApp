using Microsoft.EntityFrameworkCore;
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

        public async Task<Response<NoDataDto>> Remove(int id)
        {
            // Bu id'ye sahip data var mı?
            var isExistEntity = await _genericRepository.GetByIdAsync(id); 
            if (isExistEntity ==null) // null ise;
            {
                return Response<NoDataDto>.Fail($"{typeof(T).Name}({id}) not found.",404,true);
            }
            _genericRepository.Remove(isExistEntity); // Eğer data varsa sil. (Memory'deki state = deleted olarak işaretlendi.)
            await _unitOfWork.CommitAsync(); // Değişiklik database'e yansıtıldı.
            return Response<NoDataDto>.Success(204); // Başarılı, geriye data dönmüyorum. NoContent → Response body'sinde hiçbir data olmayacak.

            // Remove (TDto entity) olsaydı;
            // _genericRepository.Remove(entity);
            // await _unitOfWork.CommitAsync();
        }

        public async Task<Response<NoDataDto>> Update(TDto entity, int id)
        {
            // Bu id'ye sahip data var mı?
            var isExistEntity = await _genericRepository.GetByIdAsync(id); // Entity.Detached olarak işaretli. Memory'de takip edilmesin, çünkü Update metodu ile zaten memory'e takip edileceğini bildiriyorum.
            if (isExistEntity == null) // null ise;
            {
                return Response<NoDataDto>.Fail($"{typeof(T).Name}({id}) not found.", 404, true);
            }
            var updateEntity = ObjectMapper.Mapper.Map<T>(entity); // Dto → entity.
            _genericRepository.Update(updateEntity);
            await _unitOfWork.CommitAsync();
            return Response<NoDataDto>.Success(204); // NoContent → Response body'sinde hiçbir data olmayacak.
        }

        public async Task<Response<IEnumerable<TDto>>> Where(Expression<Func<T, bool>> expression) // page, pagesize
        {
            var list = _genericRepository.Where(expression);
            // list.Skip(5).Take(10);
            return Response<IEnumerable<TDto>>.Success(ObjectMapper.Mapper.Map<IEnumerable<TDto>>(await list.ToListAsync()),200);
        }
    }
}
