﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.DTOs
{
    public class ErrorDto
    {
        public List<String> Errors { get; private set; } // Liste olarak hata tutuyoruz. 
        public bool IsShow { get; private set; } // Hatayı kullanıcıya göster/gösterme.

        public ErrorDto()
        {
            Errors = new List<string>();
        }

        public ErrorDto(string error, bool isShow)
        {
            Errors.Add(error);
            isShow = true;
        }

        public ErrorDto(List<string> errors, bool isShow)
        {
            Errors = errors;
            IsShow = isShow;
        }
    }
}
