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
        private bool stop, pause;

        public Client()
        {
            client = new TcpClient();
            currentLine = 0;
            // data sampled at 10 Hz
            frequency = 10;
            stop = false;
            pause = false;
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

        public int getDataSize()
        {
            if (data != null)
                return data.Length;
            return 0;
        }

        public void start()
        {
            Thread t = new Thread(() =>
            {
                while (!stop)
                {
                    if (currentLine == data.Length - 1)
                        pause = true;
                    else
                        pause = false;

                    if (!pause)
                    {
                        sendNextLine();
                        Thread.Sleep(frequency * 10);
                    }
                }
            });
            t.Start();
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
