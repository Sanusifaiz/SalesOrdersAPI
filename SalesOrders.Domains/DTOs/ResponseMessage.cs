using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesOrders.Domains.DTOs
{
    public class ResponseMessage<T>
    {
        public bool Error { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }

        public ResponseMessage(bool error, string message, T? data)
        {
            this.Error = error;
            this.Message = message;
            this.Data = data;
        }

        public static ResponseMessage<T> Ok(string message, T data)
        {
            return new ResponseMessage<T>(false, message, data);
        }

        public static ResponseMessage<T> Ok(string message)
        {
            return new ResponseMessage<T>(false, message, default);
        }

        public static ResponseMessage<T> Fail(string message, T data)
        {
            return new ResponseMessage<T>(true, message, data);
        }

        public static ResponseMessage<T> Fail(string message)
        {
            return new ResponseMessage<T>(true, message, default);
        }
    }
}
