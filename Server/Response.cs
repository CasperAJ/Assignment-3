using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public class Response
    {
        public string status;
        public string body;

        public Response(string status, string body)
        {
            this.status = status;
            this.body = body;
        }
    }
}
