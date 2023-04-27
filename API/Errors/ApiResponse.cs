using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Errors
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string message=null)
        {
            StatusCode = statusCode;
            Message = message??GetDefaultMesaageForStatusCode(statusCode);
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }

        private string GetDefaultMesaageForStatusCode(int statusCode)
        {
            return statusCode switch 
            {
                400=>"Bad Request you have made",
                401=>"Authorized, you are not",
                404=>"Resource found,it was not",
                500=>"Errors are the path to the dark side. Errors lead to anger.",
               _=>null
                
            };
        }
    }
}