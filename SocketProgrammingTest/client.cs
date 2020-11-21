using System;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace Client {
    class Program
        {
        static void Main(string[] args)
        {
            ExecuteClient();
        }

        static void ExecuteClient() 
        {
            try
              {

                //Get the IP Address and Select a Remote End Point
                IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress address = host.AddressList[0];
                IPEndPoint endPoint= new IPEndPoint(address, 12345);
                //Open the Socket
                Socket sender = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    sender.Connect(endPoint);
                    Console.WriteLine("Connected to Socket: {0}", sender.RemoteEndPoint.ToString());

                    byte[] sentMessage = Encoding.ASCII.GetBytes("Test Message<End>");
                    int sentBytes = sender.Send(sentMessage);

                    byte[] recievedMessage = new byte[1024];

                    int recievedBytes = sender.Receive(recievedMessage);
                    Console.WriteLine("Message from Server -> {0}",
                          Encoding.ASCII.GetString(recievedMessage,
                                                     0, recievedBytes));

                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                }
                catch (ArgumentNullException ane) 
                {
                    Console.WriteLine(ane);
                }
                catch (SocketException se)
                {
                    Console.WriteLine(se);
                }
                catch (Exception ane)
                {
                    Console.WriteLine(ane);
                }
            }
              catch (Exception e) 
              {
                 Console.WriteLine(e);
              }
            }
        }
    }
    

