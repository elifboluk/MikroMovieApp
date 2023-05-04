﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Core.DTOs
{
    public class UserAppDto // Client'lara direkt olarak UserApp içerisindeki bilgileri dönemeyiz. Sadece ihtiyaçları olan bilgiye ulaşabilmeliler.
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
