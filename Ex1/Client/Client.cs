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
        // frequency in Hz
        private int frequency;

        public Client()
        {
            client = new TcpClient();
            currentLine = 0;
            // data sampled at 10 Hz
            frequency = 10;
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

        public string sendNextLine()
        {
            try
            {
                streamWriter.WriteLine(data[currentLine]);
                return data[currentLine++];
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            } // finally
            return null;
        }

        public void disconnect()
        {
            streamWriter.Close();
            client.Close();
        }

        public string getCurrentLine()
        {
            if (data != null)
                return data[currentLine];
            return null;
        }
        public int getNumOfCurrentLine()
        {
            return currentLine;
        }
        public void skipForward(int seconds)
        {
            int skipped = currentLine + (seconds * frequency);
            if (skipped < data.Length)
                currentLine = skipped;
            else
                currentLine = data.Length - 1;
        }

        public void skipBackwards(int seconds)
        {
            int skipped = currentLine - (seconds * frequency);
            if (skipped >= 0)
                currentLine = skipped;
            else
                currentLine = 0;
        }
    }
}
