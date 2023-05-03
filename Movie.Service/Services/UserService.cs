using AutoMapper.Internal.Mappers;
using Microsoft.AspNetCore.Identity;
using Movie.Core.DTOs;
using Movie.Core.Models;
using Movie.Core.Services;
using Movie.Service.Mapping;
using SharedLibrary.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Service.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<UserApp> _userManager;  // Kullanıcı ile ilgili işlem yapacağım için geçiyorum.

        public UserService(UserManager<UserApp> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto) // Yeni kullanıcı oluştur.
        {
            var user = new UserApp
            {
                Email = createUserDto.Email,
                UserName = createUserDto.UserName,
                FirstName = createUserDto.FirstName,
                LastName = createUserDto.LastName
            };

            var result = await _userManager.CreateAsync(user, createUserDto.Password);
            if (!result.Succeeded) // başarılı değilse,
            {
                var errors = result.Errors.Select(x => x.Description).ToList();
                return Response<UserAppDto>.Fail(new ErrorDto(errors, true),400); // Hata dön.
            }
            return Response<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(user), 200);
        }


        public async Task<Response<UserAppDto>> GetUserByNameAsync(string userName) // İsme göre getir.
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return Response<UserAppDto>.Fail("UserName not found", 404, true);
            }
            return Response<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(user), 200);
        }
    }
}
