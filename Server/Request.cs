using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public class Request
    {
        public string method;
        public string path;
        public int dateTime;
        public string body;

        public Request(string method, string path, int dateTime, string body)
        {
            this.method = method;
            this.path = path;
            this.dateTime = dateTime;
            this.body = body;
        }
    }
}
