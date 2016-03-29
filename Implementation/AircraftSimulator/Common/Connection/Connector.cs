using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using Common.Containers;
using Common.EventArgs;

namespace Common.Connection
{
    public delegate void MessageReceivedHandler(IConnector sender, string data);
    public delegate void ConnectionInterruptedHandler();
    public abstract class Connector : Initializer, IConnector
    {
        private const int readBufferSize = 2048;
        private const int offset = 0;
        private byte[] readBuffer = new byte[readBufferSize];
        protected TcpClient client;
        protected MessageReceivedHandler onMessageReceived;
        protected bool ReadingWithBlocking { get; set; }
        protected string Disconnected { get; set; }
        private const char Carret = (char)13;
        private const char LineBreak = (char)10;

        protected abstract void onDisconnected();

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
                if (bytesRead < 2)
                {
                    //DISCONNECTED
                    SendMessage(Disconnected);
                    return;
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
                //DISCONNECTED FROM THE CLIENT
                onDisconnected();
            }
        }

        protected void MessageReceivedNonBlocking(IAsyncResult asyncResult)
        {
            int bytesRead;
            string messageRead;
            try
            {
                bytesRead = client.GetStream().EndRead(asyncResult);
                if (bytesRead < 2)
                {
                    //DISCONNECTED
                    SendMessage(Disconnected);
                    return;
                }
                messageRead = Encoding.ASCII.GetString(readBuffer, 0, bytesRead - 2);
                onMessageReceived(this, messageRead);
                client.GetStream()
                    .BeginRead(readBuffer, offset, readBufferSize, new AsyncCallback(MessageReceivedNonBlocking), null);
            }
            catch (Exception e)
            {
                //DISCONNECTED FROM THE SERVER
                onDisconnected();
                return;
            }
        }

        public void SendMessage(string data)
        {
            if (client == null)
                return;
            lock (client.GetStream())
            {
                StreamWriter writer = new StreamWriter(client.GetStream());
                writer.Write(data + Carret + LineBreak);
                writer.Flush();
            }
        }
    }
}
