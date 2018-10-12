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

                //Thread t = new Thread(HandleClient);
                //t.Start(newClient);

                Task.Factory.StartNew(() => HandleClient(newClient));
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

            Console.WriteLine("New thread created");
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(10000);
            TcpClient newClient = (TcpClient)client;
            var stream = newClient.GetStream();

            //TcpClient Nclient = (TcpClient)client;

            //sets two streams
            //StreamWriter sWriter = new StreamWriter(Nclient.GetStream(), Encoding.ASCII);
            //StreamReader sReader = new StreamReader(Nclient.GetStream(), Encoding.ASCII);

            //Console.WriteLine("client" + sReader.ReadLine());
            //var sReader = new StreamReader(newClient.GetStream());

            var buffer = new byte[newClient.ReceiveBufferSize];

            var readClient = stream.Read(buffer, 0, buffer.Length);
            var utf8ClientMsg = Encoding.UTF8.GetString(buffer, 0, readClient);
            RawDataJTP RWJTP = new RawDataJTP();
            var clientRequest = RWJTP.RWJTP_Request(utf8ClientMsg);
            var responseToClient = RWJTP.RWJTP_Response(clientRequest);

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
