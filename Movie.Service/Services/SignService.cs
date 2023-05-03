using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Service.Services
{
    public static class SignService // Symmetric Security Key (Simetrik İmza)
    {
        public static SecurityKey GetSymmetricSecurityKey(string securityKey)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey)); // SymmetricSecurityKey'den bir nesne örneği oluşturarak içerisine bir byte dizini verdiğimde geriye symmetyric security key dönecek. Encoding.UTF8 üzerinden yukarıdaki securityKey string'inin byte'ını alıyorum. 
        }
    }
}
/*
Options Pattern:
    app.setting içerisindeki tutmuş olduğumuz key-value'lara bir class üzerinden erişmemize imkan sağlayan pattern'dir.

"Token Options": Token Options, JWT (JSON Web Token) oluştururken kullanılan parametreleri içeren bir yapıdır.
{
"Audience": Token hangi api'lere istek yapabilir? Bu parametre, JWT'nin hangi amaçla kullanılacağını belirler. JWT'nin kullanılacağı hedef uygulamanın adını veya URL'sini temsil eder. Bu, JWT'nin sadece belirli bir hedefe gönderilmesini sağlar.

"Issuer": Bu token'ı kim dağıtmış? Gerçek dünyada hangi host üzerinden dağıtılıyor? Bu parametre, JWT'nin kim tarafından oluşturulduğunu belirtir. JWT'yi oluşturan kuruluşun adını veya URL'sini içerir.

"AccesTokenExpiration":  Bu parametre, oluşturulan JWT'nin geçerlilik süresini belirler. Geçerlilik süresi, JWT'nin ne kadar süre boyunca kullanılabileceğini belirler. (dakika)

"RefreshTokenExpiration": Bu parametre, yenileme tokenlarının ne kadar süreyle geçerli olacağını belirler. Yenileme tokenları, JWT'nin geçerlilik süresi dolduğunda yeniden oluşturmak için kullanılan özel tokenlardır. (dakika)

"SecurityKey": JWT'nin şifreleme işlemi için kullanılacak olan güvenlik anahtarını içerir. Bu anahtar, JWT'nin güvenli bir şekilde iletilmesini sağlar ve JWT'nin kimlik doğrulama sürecinde kullanılmasına izin verir.
}


*/