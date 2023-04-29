using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedLibrary.DTOs
{
    public class Response<T> where T : class
    {
        public T Data { get; private set; }
        public int StatusCode { get; private set; }

        [JsonIgnore]
        public bool IsSuccessful { get; private set; } // Response'ın başarılı olup olmadığını status code yerine buradan alalım.
        public ErrorDto Error { get; private set; }

        public static Response<T> Success(T data, int statusCode)
        {
            return new Response<T> { Data = data, StatusCode = statusCode, IsSuccessful=true };
        }

        public static Response<T> Success(int statusCode)
        {
            return new Response<T> { Data = default, StatusCode = statusCode, IsSuccessful = true }; // Silme, güncelleme durumlarında data dönmeyeceğimiz için default olarak geçiyoruz. NoContent.
        }

        public static Response<T> Fail(ErrorDto errorDto, int statusCode)
        {
            return new Response<T> { Error = errorDto, StatusCode = statusCode, IsSuccessful = false };
        }

        public static Response<T> Fail(string errorMessage, int statusCode, bool isShow)
        {
            var errorDto = new ErrorDto(errorMessage, isShow);
            return new Response<T> { Error=errorDto, StatusCode=statusCode, IsSuccessful = false};
        }
    }
}
