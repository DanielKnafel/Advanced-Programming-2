using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;
using System.ComponentModel;

namespace Ex1
{
    public class Client
    {
        private TcpClient client;
        private StreamWriter streamWriter;
        private DataFileReader reader;
        public Client(DataFileReader reader)
        {
            client = new TcpClient();
            this.reader = reader;
            reader.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e)
                {
                    if (e.PropertyName == "Line")
                        sendLine(reader.Line);
                };
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
            finally
            {
                disconnect();
            }
        }
        public void sendLine(string line)
        {
            try
            {
                streamWriter.WriteLine(line);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            } 
            finally
            {
                disconnect();
            }
        }
        public void disconnect()
        {
            try
            {
                streamWriter.Close();
                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}