using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Core.Models
{
    public class UserRefreshToken
    {
        public string UserId { get; set; }
        public string RefreshTokenCode { get; set; }
        public DateTime Expiration { get; set; }
    }
}
