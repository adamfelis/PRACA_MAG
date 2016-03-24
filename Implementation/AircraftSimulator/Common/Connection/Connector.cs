using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace Common.Connection
{
    public delegate void MessageReceivedHandler(IConnector sender, string data);
    public abstract class Connector : Initializer, IConnector
    {
        private const int readBufferSize = 2048;
        private const int offset = 0;
        private byte[] readBuffer = new byte[readBufferSize];
        protected TcpClient client;
        protected MessageReceivedHandler onMessageReceived;
        protected bool ReadingWithBlocking { get; set; }

        protected override void Initialize()
        {
            if (ReadingWithBlocking)
                client.GetStream().BeginRead(readBuffer, offset, readBufferSize, new AsyncCallback(MessageReceivedBlocking), null);
            else
            {
                client.GetStream().BeginRead(readBuffer, offset, readBufferSize, new AsyncCallback(MessageReceivedNonBlocking), null);
            }
        }

        protected void MessageReceivedBlocking(IAsyncResult asyncResult)
        {
            int bytesRead;
            string messageRead;
            try
            {
                lock (client.GetStream())
                {
                    bytesRead = client.GetStream().EndRead(asyncResult);
                }
                messageRead = Encoding.ASCII.GetString(readBuffer, 0, bytesRead - 2);
                onMessageReceived(this, messageRead);
                lock (client.GetStream())
                {
                    client.GetStream()
                        .BeginRead(readBuffer, offset, readBufferSize, new AsyncCallback(MessageReceivedBlocking), null);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Failure in data reading.");
            }
        }

        protected void MessageReceivedNonBlocking(IAsyncResult asyncResult)
        {
            int bytesRead;
            string messageRead;
            bytesRead = client.GetStream().EndRead(asyncResult);
            if (bytesRead < 1)
            {
                // Disconnected
                return;
            }
            messageRead = Encoding.ASCII.GetString(readBuffer, 0, bytesRead - 2);
            onMessageReceived(this, messageRead);
            client.GetStream().BeginRead(readBuffer, offset, readBufferSize, new AsyncCallback(MessageReceivedNonBlocking), null);
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
