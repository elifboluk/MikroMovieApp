using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Core.DTOs
{
    public class CreateUserDto // Yeni kullanıcıyı kaydedecek Dto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }        
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
