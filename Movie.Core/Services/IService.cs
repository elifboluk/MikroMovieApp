using SharedLibrary.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Core.Services
{
    public interface IService<T, TDto> where T : class where TDto : class // Gelen entity'i hangi dto'ya dönüştüreceğimi belirliyorum.
    {
        Task<Response<TDto>> GetByIdAsync(int id); // Geriye direkt olarak Dto dönüyorum.
        Task<Response<IEnumerable<TDto>>> GetAllAsync();
        Task<Response<IEnumerable<TDto>>> Where(Expression<Func<T, bool>> expression);
        Task<Response<TDto>> AddAsync(T entity);
        Task<Response<NoDataDto>> Remove(T entity); // Başarılı işlem fakat client'a geriye data dönmüyorum. 
        Task<Response<NoDataDto>> Update(T entity);
    }
}
