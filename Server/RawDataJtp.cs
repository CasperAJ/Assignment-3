using System;
using System.Collections.Generic;
using System.Linq;
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

        public string RWJTP_Response(Request clientRequest)
        {
            Response responseObj = new Response("","");
            responseObj = MethodCheck(clientRequest, responseObj);
            responseObj = DateCheck(clientRequest, responseObj);

            return JsonConvert.SerializeObject(responseObj);
        }

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // Error creation
        public Response ErroResponse(Request failedRequest, Response responseObj)
        {
            if (failedRequest.method == "Illegal method")
            {
                responseObj.status = "6 Bad Request";
                responseObj.body = "Missing method";
            }

            return responseObj;
        }

        // Validation methods

        public Response MethodCheck(Request clientRequest, Response responseObj)
        {
            string[] acceptedMethods = new[] {"create", "read", "update", "delete", "echo"};

            if (clientRequest.method == string.Empty || !acceptedMethods.Contains(clientRequest.method))
            {
                responseObj.status += "4 Illegal method";
            }
            return responseObj;
        }

        public Response DateCheck(Request clientRequest, Response responseObj)
        {
            if (clientRequest.dateTime != DateTimeOffset.Now.ToUnixTimeSeconds())
            {
                responseObj.status += "4 Illegal date";
            }
            return responseObj;
        }

        public Response CheckPath(Request clientRequest, Response responseObj)
        {
            if (clientRequest.path == String.Empty)
            {
                responseObj.status += "4 Bad Request";
                return responseObj;
            }

            var apiPath = clientRequest.path.Split('\\');

            if (apiPath[0]  != "api")
            {
                responseObj.status += "4 Bad Request";
                return responseObj;
            }



        }

    }
}
