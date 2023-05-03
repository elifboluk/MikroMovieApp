using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Movie.Core.DTOs;
using Movie.Core.Models;
using Movie.Core.Services;
using SharedLibrary.Configurations;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Service.Services
{
    internal class TokenService : ITokenService // Core katmanında
    {
        private readonly UserManager<UserApp> _userManager; // Kullanıcı ile ilgili işlem yapacağım için UserManager geçiyorum.
        private readonly CustomTokenOption _customTokenOption; // Options'lara göre token oluşacağı için CustomTokenOption geçiyorum.
        public TokenService(UserManager<UserApp> userManager, IOptions<CustomTokenOption> customTokenOption)
        {
            _userManager = userManager;
            _customTokenOption = customTokenOption.Value; // Birden fazla options olacağı için IOptions<CustomTokenOption> şeklinde generic olarak ifade edilmesi daha doğru.
        }

        private string CreateRefreshToken() // Random string token üretecek. Refresh token'ı set CreateRefreshToken ile set edeceğim.
        {
            var numberByte = new Byte[32]; // 32byte'lık random string değer
            using var rnd = RandomNumberGenerator.Create(); // Random değer üretecek. 
            rnd.GetBytes(numberByte); // rnd'nin üretilen byte'larını alıp numberByte'a atadım. 
            return Convert.ToBase64String(numberByte); // Base64 ile encode et ve stringe dönüştür. 32 byte'lık random string değer üretildi. 
        } 

        private IEnumerable<Claim> GetClaims(UserApp userApp, List<String> audiences) // audiences: token'ın istek yapacağı api bilgisi
        {// CrateRefrehToken ile token oluşunca payload'a eklenecek kullanıcı bilgileri.↓
            var userList = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, userApp.Id), // NameIdentifier = Kullanıcı id'sine karşılık geliyor.
                new Claim(JwtRegisteredClaimNames.Email, userApp.Email),
                new Claim(ClaimTypes.Name, userApp.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Json'ı kimliklendirir, id üretir.            
            };
            userList.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x))); // Her birine git bir claim nesnesi oluştur, JwtRegisteredClaimNames.Aud ile bir api'ye istek yapıldığında bu token'ın audience'ına istek yapmaya uygun mu değil mi diye bakılır ve uygun değilse istek geri çevirilir. 
            return userList;
        }
        public TokenDto CreateToken(UserApp userApp)
        {
            var accessTokenExpiration = DateTime.Now.AddMinutes(_customTokenOption.AccessTokenExpiration); // Süre
            var refreshTokenExpiration = DateTime.Now.AddMinutes(_customTokenOption.RefreshTokenExpiration); // Süre
            var securityKey = SignService.GetSymmetricSecurityKey(_customTokenOption.SecurityKey); // İmzalayacak key


            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature); // İmzalama algoritması HmacSha256Signature


            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: _customTokenOption.Issuer, // Token'ı yayınlayan bilgisi Auth server
                expires: accessTokenExpiration, 
                notBefore: DateTime.Now, // Token Şimdi ile verilen dakika arasında geçerli.
                claims: GetClaims(userApp, _customTokenOption.Audience), // Token'ının hangi api'lere ulaşabileceğinin bilgisi
                signingCredentials: signingCredentials); // imza


            var handler = new JwtSecurityTokenHandler(); // JwtSecurityTokenHandler, token oluşturacak.

            var token = handler.WriteToken(jwtSecurityToken);

            var tokenDto = new TokenDto
            {
                AccessToken = token,
                RefreshToken = CreateRefreshToken(),
                AccessTokenExpiration = accessTokenExpiration,
                RefreshTokenExpiration = refreshTokenExpiration
            };
            return tokenDto;
        }
    }
}

