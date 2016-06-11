using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using Common.Containers;
using Common.EventArgs;
using Common.Exceptions;

namespace Common.Connection
{
    public delegate void MessageReceivedHandler(IConnector sender, string data);
    public delegate void ConnectionInterruptedHandler();
    public abstract class Connector : Initializer, IConnector
    {
        private const int maxReadBufferSize = 4096;
        private const int bytesReadInSingleChunk = 512;
        private int totalBytesRead = 0;
        private StringBuilder stringBuilder = new StringBuilder(maxReadBufferSize);
        private byte[] readBuffer = new byte[maxReadBufferSize];
        protected TcpClient client;
        protected MessageReceivedHandler onMessageReceived;
        protected bool ReadingWithBlocking { get; set; }
        protected string Disconnected { get; set; }
        private const char Carret = (char)13;
        private const char LineBreak = (char)10;

        public abstract void onDisconnected(ErrorCodeException e);

        public void CloseConnection()
        {
            if (client.Connected)
                client.Close();
        }

        protected override void Initialize()
        {
            BeginReading();
        }

        public void BeginReading()
        {
            try
            {
                client.GetStream().BeginRead(
                readBuffer, totalBytesRead, bytesReadInSingleChunk,
                new AsyncCallback(EndReading),
                null);
            }
            catch (Exception e)
            {
                onDisconnected(new LawLayerException()
                {
                    Error = ErrorCode.ReadOperation,
                    Exception = e
                });
            }
        }

        public void EndReading(IAsyncResult ar)
        {
            NetworkStream stream = null;
            int bytesRead = 0;
            try
            {
                stream = client.GetStream();
                bytesRead = stream.EndRead(ar);
            }
            catch (Exception e)
            {
                onDisconnected(new LawLayerException()
                {
                    Error = ErrorCode.ReadOperation,
                    Exception = e
                });
            }

            if (bytesRead == 0)
            {
                onDisconnected(new LawLayerException()
                {
                    Error = ErrorCode.ReadOperation,
                    Exception = new Exception("0 bytes read")
                });
            }

            try
            { 
                var messageRead = Encoding.ASCII.GetString(readBuffer, totalBytesRead, bytesRead);

                //if (stream.DataAvailable)
                //if(messageRead[messageRead.Length - 1] != LineBreak)
                var lineBrakPosition = messageRead.IndexOf(LineBreak);
                if (lineBrakPosition == -1)
                {
                    totalBytesRead += bytesRead;
                    stringBuilder.Append(messageRead);
                    BeginReading();
                }
                else
                {
                    messageRead = messageRead.Substring(0, lineBrakPosition); //messageRead.Length - 1);
                    stringBuilder.Append(messageRead);
                    //Message fully completed
                    onMessageReceived(this, stringBuilder.ToString());

                    int sourceIndex = totalBytesRead + lineBrakPosition + 1;
                    int length = bytesRead - lineBrakPosition - 1;
                    Array.Copy(readBuffer, sourceIndex, readBuffer, 0, length);
                    //clearing total bytes read and string builder cause we start new message
                    //totalBytesRead = 0;
                    totalBytesRead = length;
                    var m = Encoding.ASCII.GetString(readBuffer, 0, totalBytesRead);
                    if (length>0)
                    {
                       int a = 0;
                    }
                    stringBuilder = new StringBuilder(maxReadBufferSize);
                    stringBuilder.Append(m);
                    BeginReading();
                }
            }
            catch (Exception e)
            {
                onDisconnected(new LawLayerException()
                {
                    Error = ErrorCode.ReadOperation,
                    Exception = e
                });
            }
        }


        public void SendMessage(string data)
        {
            if (client == null)
                return;
            try
            {
                lock (client.GetStream())
                {
                    StreamWriter writer = new StreamWriter(client.GetStream());
                    writer.Write(data + LineBreak);
                    writer.Flush();
                }
            }
            catch (Exception e)
            {
                string a = e.Message;
                onDisconnected(new LawLayerException()
                {
                    Error = ErrorCode.WriteOperation,
                    Exception = e
                });
            }
        }
    }
}
