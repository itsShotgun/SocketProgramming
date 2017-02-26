using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            IPEndPoint ip = new IPEndPoint(IPAddress.Parse("172.19.28.55"), 567);//Allocate client's iP address, port number
            Socket newsock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//Local socket, Type is Tcp
            newsock.Bind(ip);//Bind Sock to ip
            newsock.Listen(2);//Listening
            Socket remotesock = newsock.Accept();//Accept new port
            EndPoint remoteip = remotesock.RemoteEndPoint;//Get new port
            byte[] data = new byte[1024];//Data buffer for incoming data
            Random random = new Random();//Initialize random integer
            DateTime dt = new DateTime();//Initialize date time
            string value;//temporary variable
            while (true)
            {
                remotesock.Receive(data);//Get data from server
                value = System.Text.Encoding.Default.GetString(data);//Transfer data to string
                if (value.StartsWith("R"))//if starts with R
                {
                    Console.WriteLine("receive server sending requirement...");//Display the statement
                    value = random.Next(0, 100).ToString();//Randomly get a number, transfer to string
                    Console.WriteLine("send random value {0} to server...", value);//Display the number
                    remotesock.Send(System.Text.Encoding.Default.GetBytes(value));//Send the number to server
                    dt = DateTime.Now;//Record sending time
                }
                else if (value.StartsWith("E"))//If start with E
                {
                    Console.WriteLine("receive server closing requirement...");//Display the statement
                    break;//break the loop
                }
                else//Send time information to server
                {
                    Console.WriteLine("receive server timestamp {0}...", value);//Display the time information
                    TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - dt.Ticks);//Calculate the delay
                    Console.WriteLine("round trip delay {0}...", ts.ToString());//Display the information
                }
            }
            remotesock.Close();//Close the server socket
            newsock.Close();//Close the local socket
        }
    }
}
