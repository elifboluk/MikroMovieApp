using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Core.DTOs
{
    public class TokenDto
    {
        public string AccessToken { get; set; }
        public DateTime AccessTokenExpiration { get; set; } // Access Token süresi
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; } // Refresh Token süresi

    }
}
