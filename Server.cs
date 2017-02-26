using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server
{
    class Program
    {
        static bool exitthread = false;// exit from thread
        static void ThreadProc()//Thread Function
        {
            IPEndPoint ip = new IPEndPoint(IPAddress.Parse("192.168.74.1"), 234);//Allocate server's iP address and port number
            Socket newsock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//Local Socket, type is Tcp
            newsock.Bind(ip);//Bind Socket to port
            IPEndPoint remoteip = new IPEndPoint(IPAddress.Parse("179.19.28.55"), 567);//Allocate Client's iP address and port number
            newsock.Connect(remoteip);//Connect to the client
            byte[] data = new byte[1024];//Data buffer for incoming data
            while (!exitthread)//if while(true):continue the thread
            {
                newsock.Send(System.Text.Encoding.Default.GetBytes("R"));//Send Request"R"
                newsock.Receive(data);//Receive data from client
                Console.WriteLine("receive client random {0}", System.Text.Encoding.Default.GetString(data));//Display Random number
                newsock.Send(System.Text.Encoding.Default.GetBytes(DateTime.Now.ToString("hh:mm:ss.fff")));//Send time information
                Thread.Sleep(3000);//every three seconds
            }
            newsock.Send(System.Text.Encoding.Default.GetBytes("E"));//Send"E"
            newsock.Close();//Terminate Socket
            exitthread = false;// exit from thread
        }

        static void Main(string[] args)
        {
            new Thread(ThreadProc).Start();//Start Socket
            if (Console.Read() == 'E')//If press "E"
            {
                exitthread = true;//to make while loop false: get out of the Threadpro while loop
            }
            while (exitthread)//wait false to the thread
            { 
                Thread.Sleep(1000);//Close Socket every 1 second
            }
        }
    }
}
