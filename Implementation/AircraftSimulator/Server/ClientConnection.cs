using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public delegate void MessageReceivedHandler(IClientConnection sender, string data);
    public class ClientConnection : IClientConnection
    {
        private const int readBufferSize = 255;
        private const int offset = 0;
        private TcpClient client;
        private byte[] readBuffer = new byte[readBufferSize];
        private MessageReceivedHandler onMessageReceived;
        private static int id_counter = 0;

        public ClientConnection(TcpClient client, MessageReceivedHandler onMessageReceivedHandler)
        {
            this.client = client;
            this.onMessageReceived = onMessageReceivedHandler;
            this.Id = id_counter++;
            this.client.GetStream()
                .BeginRead(readBuffer, offset, readBufferSize, new AsyncCallback(MessageReceived), null);
        }

        public int Id { get; }

        public string ClientName { get; set; }

        private void MessageReceived(IAsyncResult asyncResult)
        {
            int bytesRead;
            string messageRead;
            try
            {
                lock (client.GetStream())
                {
                    bytesRead = client.GetStream().EndRead(asyncResult);
                }
                messageRead = Encoding.ASCII.GetString(readBuffer, 0, bytesRead - 1);
                onMessageReceived(this, messageRead);
                lock (client.GetStream())
                {
                    client.GetStream()
                        .BeginRead(readBuffer, offset, readBufferSize, new AsyncCallback(MessageReceived), null);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Failure in data reading.");
            }
        }

        public void SendMessage(string data)
        {
            lock (client.GetStream())
            {
                StreamWriter writer = new StreamWriter(client.GetStream());
                writer.Write(data + (char)13 + (char)10);
                writer.Flush();
            }
        }

    }
}
