﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Containers;
using Common.DataParser;

namespace Client.Priveleges
{
    public class ClientInputPriveleges : IClientInputPriveleges
    {
        private IDataParser dataParser;
        private IServerConnection serverConnection;
        public ClientInputPriveleges(ref IDataParser dataParser, ref IServerConnection serverConnection)
        {
            this.dataParser = dataParser;
            this.serverConnection = serverConnection;
        }
        public string SendDataRequest(IDataList data)
        {
            try
            {
                var stringData = dataParser.Serialize(data);
                serverConnection.SendMessage(stringData);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return "Client data sent to the server";
        }
    }
}
