using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Movie.Core.DTOs;
using Movie.Core.Models;
using Movie.Core.Repositories;
using Movie.Core.Services;
using Movie.Core.UnifOfWorks;
using SharedLibrary.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Service.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ITokenService _tokenService; // Token oluşturmak için token servis alındı
        private readonly UserManager<UserApp> _userManager; // Kullanıcı var mı yok mu 
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<UserRefreshToken> _userRefreshTokenService;

        public AuthenticationService(ITokenService tokenService, UserManager<UserApp> userManager, IUnitOfWork unitOfWork, IGenericRepository<UserRefreshToken> userRefreshTokenService)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _userRefreshTokenService = userRefreshTokenService;
        }

        public async Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto)
        {
            if (loginDto == null) // null ise;
            {
                throw new ArgumentNullException(nameof(loginDto));
            }

            var user = await _userManager.FindByEmailAsync(loginDto.Email); // email var mı?
            if (user == null) // email yoksa
            {
                return Response<TokenDto>.Fail("E-mail or password is wrong.", 400, true); // 400 Client hatası.
            }

            if (!await _userManager.CheckPasswordAsync(user , loginDto.Password)) // E-mail doğru, şifre doğru mu diye bakalım. Yanlışsa;
            {
                return Response<TokenDto>.Fail("E-mail or password is wrong.", 400, true); // 400 Client hatası.
            }

            // Bütün if'lerden kurtulduysak artık eminiz ki kullanıcı var, E-mail ve şifre doğru girildi. Artık token üretebiliriz.
            var token = _tokenService.CreateToken(user); 




            var userRefreshToken = await _userRefreshTokenService.Where(x => x.UserId == user.Id).SingleOrDefaultAsync(); // Kullanıcının id'sine göre Refresh token'ı kaydedeceğim.

            // Ama önce UserId'ye göre refresh token veritabanında var mı, kontrol et. Varsa getir, yoksa null dön.
            if (userRefreshToken==null) // Refresh token veritabanında yoksa oluştur.
            {
                await _userRefreshTokenService.AddAsync(new UserRefreshToken { UserId = user.Id, RefreshTokenCode = token.RefreshToken, Expiration = DateTime.Now.AddDays(5)});
            }
            else // Refresh token veritabanında varsa güncelle.
            {
                userRefreshToken.RefreshTokenCode = token.RefreshToken;
                userRefreshToken.Expiration = token.RefreshTokenExpiration;
            }
            await _unitOfWork.CommitAsync();
            return Response<TokenDto>.Success(token, 200);
        }

        public async Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken)
        {
            var existRefreshToken = await _userRefreshTokenService.Where(x => x.RefreshTokenCode == refreshToken).SingleOrDefaultAsync();// Veritabanında refresh token var mı kontrol et.
            if (existRefreshToken==null) // null ise;
            {
                return Response<TokenDto>.Fail("Refresh token not found.",404,true);
            }

            var user = await _userManager.FindByIdAsync(existRefreshToken.UserId); // existRefreshToken üzerinden UserId'i alındı.
            if (user==null) // user var mı kontrol edelim. Null ise;
            {
                return Response<TokenDto>.Fail("User Id not found", 404, true);
            }

            var tokenDto = _tokenService.CreateToken(user);
            existRefreshToken.RefreshTokenCode = tokenDto.RefreshToken;
            existRefreshToken.Expiration = tokenDto.RefreshTokenExpiration;


            await _unitOfWork.CommitAsync();
            return Response<TokenDto>.Success(tokenDto, 200);
        }

        public async Task<Response<NoDataDto>> RevokeRefreshToken(string refreshToken)
        {
            var existRefreshToken = await _userRefreshTokenService.Where(x => x.RefreshTokenCode == refreshToken).SingleOrDefaultAsync();
            if (existRefreshToken == null)
            {
                return Response<NoDataDto>.Fail("Refresh token not found.", 404, true);
            }

            _userRefreshTokenService.Remove(existRefreshToken);
            await _unitOfWork.CommitAsync();
            return Response<NoDataDto>.Success(200);
        }
    }
}
