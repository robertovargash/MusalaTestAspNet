using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusalaTestAspNet.Shared.Models
{
    public enum Code { Success = 200, Error = 500, Warning = 501 }

    public class MyResponse<T>
    {
        public Code Code { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public MyResponse()
        {
            this.Code = Code.Warning;
            this.Message = "Unknown";
        }
    }
}
