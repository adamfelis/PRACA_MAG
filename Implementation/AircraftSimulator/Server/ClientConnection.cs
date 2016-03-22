using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public delegate void MessageReceivedHandler(ClientConnection sender, string data);
    public class ClientConnection
    {
        private const int readBufferSize = 255;
        private const int offset = 0;
        private TcpClient client;
        private byte[] readBuffer = new byte[readBufferSize];
        private MessageReceivedHandler onMessageReceived;

        public ClientConnection(TcpClient client, MessageReceivedHandler onMessageReceivedHandler)
        {
            this.client = client;
            this.onMessageReceived = onMessageReceivedHandler;
            this.client.GetStream()
                .BeginRead(readBuffer, offset, readBufferSize, new AsyncCallback(MessageReceived), null);
        }
        
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
    }
}
