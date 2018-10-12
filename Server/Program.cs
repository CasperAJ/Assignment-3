using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
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
                var stream = newClient.GetStream();

                var buffer = new byte[newClient.ReceiveBufferSize];

                var readClient = stream.Read(buffer, 0, buffer.Length);
                var utf8ClientMsg = Encoding.UTF8.GetString(buffer, 0, readClient);
                Request clientMsg = JsonConvert.DeserializeObject<Request>(utf8ClientMsg);


                Console.WriteLine("Methid: {0}, Path: {1}, DateTime: {2}, Body: {3}", clientMsg.method, clientMsg.path, clientMsg.dateTime, clientMsg.body);
            }



            /* Echo Server fra lector test
            var server = new TcpListener(IPAddress.Parse("127.0.0.1"), 5000);
            server.Start();
            Console.WriteLine("Server started...");

            while (true)
            {
                var client = server.AcceptTcpClient();

                var stream = client.GetStream();
                var buffer = new byte[client.ReceiveBufferSize];

                var readCnt = stream.Read(buffer, 0, buffer.Length);
                var msg = Encoding.UTF8.GetString(buffer, 0, readCnt);

                Console.WriteLine("Messge: {0}", msg);

                var upMsg = Encoding.UTF8.GetBytes(msg.ToUpper());

                stream.Write(upMsg, 0, upMsg.Length);

                stream.Close();
                client.Close();

            }

             */

            /* Echo client
            var client = new TcpClient();

            client.Connect(IPAddress.Parse("127.0.0.1"), 5000);

            var msg = Encoding.UTF8.GetBytes("Hello");

            var stream = client.GetStream();

            stream.Write(msg, 0, msg.Length);

            var buffer = new byte[client.ReceiveBufferSize];
            //stream.Read(buffer, 0, buffer.Length);

            var readCnt = stream.Read(buffer, 0, buffer.Length);

            var resMsg = Encoding.UTF8.GetString(buffer, 0, readCnt);

            Console.WriteLine("Response: {0}", resMsg);

            stream.Close();
            client.Close();


             */
        }
    }
}
