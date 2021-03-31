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
        private TcpClient client;
        private StreamWriter streamWriter;
        private string[] data;
        private int currentLine;

        public Client()
        {
            client = new TcpClient();
            currentLine = 0;
        }

        public void connect(string server, int port)
        {
            try
            {
                if (client != null)
                {
                    client.Connect(server, port);
                    streamWriter = new StreamWriter(client.GetStream());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void setData(string fileName)
        {
            try
            {
                data = File.ReadAllLines(fileName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public int dataSize()
        {
            if (data != null)
                return data.Length;
            return 0;
        }

        public void sendNextLine()
        {
            try
            {
                streamWriter.WriteLine(data[currentLine++]); 
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void disconnect()
        {
            streamWriter.Close();
            client.Close();
        }

        public string getCurrentLine()
        {
            return data[currentLine];
        }
    
        public void skipForward()
        {

        }
    }
}
