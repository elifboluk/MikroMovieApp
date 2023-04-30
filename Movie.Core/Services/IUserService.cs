using Movie.Core.DTOs;
using SharedLibrary.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Core.Services
{
    public interface IUserService
    {
        Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto);
        Task<Response<UserAppDto>> GetUserByNameAsync(string userName); // Username ile beraber user bilgilerini getir.
    }
}

// User, veritabanı ile ilgili bir işlem ama repository'sini oluşturmadık, direkt olarak servisini oluşturduk. Bunun nedeni Identity kütüphanesi ile beraber buradan bize zaten hazır metodlar geliyor. Ayrıca user ile ilgili bir repository katmanı oluşturmamıza gerek yok. UserManager sınıfından DI'ya geçtiğimiz anda UserManager sınıfı üzerinden Create User, Update User gibi kullanıcının rol eklenmesi ve rolden çıkarılması gibi yapılar geliyor.
// Identity tarafından bize kullanıcılar hakkında işlem yapabilmek için 3 büyük class gelir; UserManager(Kullanıcılar hakkında işlem yapabilmek için), RolManager(Roller üzerinde işlem yapabilmek için, rol ekleme, rol silme, rol güncelleme gibi), SignInManager(Kullanıcıların login, logout olması gibi işlemleri gerçekleştirmek için) 
// Bunlar bana zaten IdentityApi'den hazır olarak geldiği için ekstra bir repository katmanına ihtiyacımız yok direkt olarak servislerini yazıyoruz.

