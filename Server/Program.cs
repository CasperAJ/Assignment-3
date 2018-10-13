using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new TcpListener(IPAddress.Parse("127.0.0.1"), 5000);
            server.Start();
            Console.WriteLine("Server is ready");

            while (true)
            {
                var newClient = server.AcceptTcpClient();
                if (newClient.Connected)
                {
                    Task.Factory.StartNew(() => HandleClient(newClient));
                }

                //Thread t = new Thread(HandleClient);
                //t.Start(newClient);

                #region Tested unthread code
                //                var stream = newClient.GetStream();
                //
                //                var buffer = new byte[newClient.ReceiveBufferSize];
                //
                //                var readClient = stream.Read(buffer, 0, buffer.Length);
                //                var utf8ClientMsg = Encoding.UTF8.GetString(buffer, 0, readClient);
                //                RawDataJTP RWJTP = new RawDataJTP();
                //                var clientRequest = RWJTP.RWJTP_Request(utf8ClientMsg);
                //
                //
                //                Console.WriteLine("Method: {0}, Path: {1}, DateTime: {2}, Body: {3}", clientRequest.method, clientRequest.path, clientRequest.dateTime, clientRequest.body);

                #endregion
            }

        }

        public static void HandleClient(object client)
        {
            // Tells new connection has been received and a new thread has been created to take care of it, post it in cw along with thread id
            Console.WriteLine("New thread created");
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            // Initiate a instance of the RawData Json Transport Protocol (RWJTP) used for converting to/from JSON
            RawDataJTP RWJTP = new RawDataJTP();

            // Casts the connection back to a tcpClient and gets the clients stream
            TcpClient newClient = (TcpClient)client;
            var stream = newClient.GetStream();

            // Creates the buffer for the stream
            var buffer = new byte[newClient.ReceiveBufferSize];

            // Reads the clients msg from the stream, and encodes it back to a string in JSON format
            var readClient = stream.Read(buffer, 0, buffer.Length);
            var utf8ClientMsg = Encoding.UTF8.GetString(buffer, 0, readClient);

            // Hands over the request to WRJTP for deserialization, and returns a Request Object
            var clientRequest = RWJTP.RWJTP_Request(utf8ClientMsg);
            // Hands over the Request Object to the WRJTP again in order to perform logic and return a Response Object that can be returned to the requester
            var responseToClient = RWJTP.RWJTP_Response(clientRequest);

            // Writes the response back to the requester
            stream.WriteAsync(buffer, 0 ,responseToClient.Length);

            Console.WriteLine(responseToClient);
            Console.WriteLine();
            if (clientRequest.method != String.Empty)
            {
                Console.WriteLine("Method: {0}, Path: {1}, DateTime: {2}, Body: {3}", clientRequest.method, clientRequest.path, clientRequest.dateTime, clientRequest.body);

            }
            Console.WriteLine("Task completed " +Thread.CurrentThread.ManagedThreadId);
            
        }
    }

}
