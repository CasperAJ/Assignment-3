using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Server
{
    public class RawDataJTP
    {
        public Request RWJTP_Request(string clientRequest)
        {
            Request requestObj = JsonConvert.DeserializeObject<Request>(clientRequest);

            return requestObj;
        }

        public string RWJTP_Response(Request requestObj)
        {

            Response responseObj = new Response("1 Ok", "tester");

            var x = JsonConvert.SerializeObject(responseObj);
            return x;
        }

    }
}
