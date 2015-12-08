using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Tanks
{
    class Program
    {
        static void Main(string[] args)
        {
            startGame();
        }

        public static void startGame()
        {
            NetworkStream net = null;
            TcpClient conn = null;
            String msg = null;
            try
            {
                Console.WriteLine("connecting to game server");
                String command = "JOIN#";
                conn = new TcpClient();
                //connect to server listening on port 6000 in localhost
                conn.Connect("localhost", 6000);
                ASCIIEncoding encode = new ASCIIEncoding();
                byte[] sending_msg = encode.GetBytes(command);
                net = conn.GetStream();
                //sending join command
                net.Write(sending_msg, 0, sending_msg.Length);

            }
            catch (Exception e)
            {
                Console.WriteLine("Error:" + e.StackTrace);
            }
            finally 
            { 
            if(net!=null)
            {
                net.Close();
                TcpListener client_listener = new TcpListener(IPAddress.Any,7000);
                client_listener.Start();
                Byte[] incoming_msg = new Byte[100];

                while(true)
                {
                    TcpClient client = client_listener.AcceptTcpClient();
                    NetworkStream stream = client.GetStream();
                    msg = System.Text.Encoding.ASCII.GetString(incoming_msg,0,stream.Read(incoming_msg,0,incoming_msg.Length));
                    Console.WriteLine("server send: "+msg);
                    client.Close();
                    stream.Close();
                }

               

            }
            conn.Close();
            }
            
        }
    }
}
