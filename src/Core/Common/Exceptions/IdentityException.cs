using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions
{
    public class IdentityException:Exception
    {
        public IdentityException(int code, string clientMessage, int statusCode) : base(clientMessage)
        {
            Code = code;
            ClientMessage = clientMessage;
            StatusCode = statusCode;
        }

        public int Code { get; set; }
        public string ClientMessage { get; set; }
        public int StatusCode { get; set; }
    }
}
