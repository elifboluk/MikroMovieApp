using Movie.Core.DTOs;
using SharedLibrary.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Core.Services
{
    public interface IAuthenticationService // Kimlik doğrulama. Kullanıcıdan alınan bilgiler (username-password) doğruysa geriye token dönecek. 
    {
        Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto); // loginDto doğruysa token dön.
        Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken); // refreshToken ile yeniden bir token oluştur.
        Task<Response<NoDataDto>> RevokeRefreshToken(string refreshToken); // Kullanıcı logout yaptığında veya güvenlik sağlamak amacıyla refresh token'ı null olarak set et.
    }
}
