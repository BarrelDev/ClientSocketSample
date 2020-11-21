using System;
using System.Text;
using System.Net.Sockets;
using System.Net;
namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            ExecuteServer();
        }

        static void ExecuteServer()
        {
            //Get the IP Address and Select a Remote End Point
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress address = host.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(address, 12345);

            Socket listener = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                //Associate an End Point with the socket
                listener.Bind(endPoint);
                listener.Listen(10);

                while (true) 
                {
                    Console.WriteLine("Waiting for client connection ...");

                    //Set the senderSocket to the socket that connects
                    Socket senderSocket = listener.Accept();

                    //Setup a buffer for incoming data
                    byte[] bytes = new byte[1024];
                    string data = null;

                    //Go through all of the messages until "<End>" appears
                    while (true) 
                    {
                        int numByte = senderSocket.Receive(bytes);
                        data += Encoding.ASCII.GetString(bytes,
                                                   0, numByte);
                        if (data.IndexOf("<End>") > -1)
                            break;
                    }

                    //Send message back to client
                    Console.WriteLine("Text Recieved -> {0}", data);
                    byte[] message = Encoding.ASCII.GetBytes("Server Ping");

                    senderSocket.Send(message);

                    senderSocket.Shutdown(SocketShutdown.Both);
                    senderSocket.Close();
                }
            }
            catch (Exception e) {
                Console.WriteLine(e);
            }
        }
    }
}
