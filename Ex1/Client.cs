using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;

namespace Ex1
{
    class Client
    {
       // private NetworkStream stream;
        private TcpClient client;
        private StreamWriter streamWriter;
        public void sendFile(string fileName)
        {
            try
            {
                System.IO.StreamReader file = new System.IO.StreamReader(fileName);
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    streamWriter.WriteLine(line);
                    // Send the message to the connected TcpServer.
                    //Byte[] data = System.Text.Encoding.ASCII.GetBytes(line+"\n");
                    //stream.Write(data, 0, data.Length); 
                    Thread.Sleep(100);
                }
                streamWriter.Close();
                client.Close();

            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }
        }

        public Client()
        {
            try
            {
                client = new TcpClient();
                client.Connect("localhost", 5400);
                streamWriter = new StreamWriter(client.GetStream());
                //client = new TcpClient("localhost", 5400);
                //stream = client.GetStream();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
