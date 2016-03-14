using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using MLApp;
namespace ServerApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        const int PORT_NUM = 10000;
        private Hashtable clients = new Hashtable();
        private TcpListener listener;
        private Thread listenerThread;
        private MLApp.MLApp mlApp;
        private int res1;
        private int res2;


        // This subroutine sends a message to all attached clients
        private void Broadcast(string strMessage)
        {
            UserConnection client;
            // All entries in the clients Hashtable are UserConnection so it is possible
            // to assign it safely.
            foreach (DictionaryEntry entry in clients)
            {
                client = (UserConnection)entry.Value;
                client.SendData(strMessage);
            }
        }

        private void ConnectUser(string userName, UserConnection sender)
        {
            sender.Name = userName;
            UpdateStatus(userName + " connected to the server.");
            clients.Add(userName, sender);
            listBox2.Items.Add(sender.Name);
            SendToClients("JOIN|", sender);
        }

        // This subroutine notifies other clients that sender left the chat, and removes
        // the name from the clients Hashtable
        private void DisconnectUser(UserConnection sender)
        {
            UpdateStatus(sender.Name + " disconnected from the server.");
            clients.Remove(sender.Name);
            listBox2.Items.Remove(sender.Name);
        }

        // This subroutine is used a background listener thread to allow reading incoming
        // messages without lagging the user interface.
        private void DoListen()
        {
            try
            {
                // Listen for new connections.
                listener = new TcpListener(System.Net.IPAddress.Any, PORT_NUM);
                listener.Start();
                do
                {
                    UserConnection client = new UserConnection(listener.AcceptTcpClient());
                    client.LineReceived += new LineReceive(OnLineReceived);
                } while (true);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }
        }

        // When the window closes, stop the listener.
        private void frmMain_Closing(object sender, System.ComponentModel.CancelEventArgs e) //base.Closing;
        {
            listener.Stop();
        }

        // This is the event handler for the UserConnection when it receives a full line.
        // Parse the cammand and parameters and take appropriate action.
        private void OnLineReceived(UserConnection sender, string data)
        {
            string[] dataArray;
            // Message parts are divided by "|"  Break the string into an array accordingly.
            // Basically what happens here is that it is possible to get a flood of data during
            // the lock where we have combined commands and overflow
            // to simplify this proble, all I do is split the response by char 13 and then look
            // at the command, if the command is unknown, I consider it a junk message
            // and dump it, otherwise I act on it
            dataArray = data.Split((char)13);

            dataArray = dataArray[0].Split('|');

            // dataArray(0) is the command.
            switch (dataArray[0])
            {
                case "CONNECT":
                    ConnectUser(dataArray[1], sender);
                    break;
                case "DATA":
                    SendChat(dataArray[1], sender);
                    break;
                case "DISCONNECT":
                    DisconnectUser(sender);
                    break;
                default:
                    // Message is junk do nothing with it.
                    break;
            }
        }

      
        // Send a chat message to all clients except sender.
        private void SendChat(string message, UserConnection sender)
        {
            UpdateStatus(sender.Name + ": " + message);

            //We do not need to inform other clients
            //SendToClients("DATA|" + sender.Name + ": " + message, sender);
        }

        // This subroutine sends a message to all attached clients except the sender.
        private void SendToClients(string strMessage, UserConnection sender)
        {
            UserConnection client;
            // All entries in the clients Hashtable are UserConnection so it is possible
            // to assign it safely.
            foreach (DictionaryEntry entry in clients)
            {
                client = (UserConnection)entry.Value;
                client.SendData(strMessage);
            }
        }

        // This subroutine adds line to the Status listbox
        private void UpdateStatus(string statusMessage)
        {
            listBox1.Items.Add(statusMessage);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            mlApp = new MLApp.MLApp();
            //object result = null;
            //mlApp.Feval("CalcInverse", 2, out result, 2, 5);
            //object[] res = result as object[];
            //res1 = (int)res[0];
            //res2 = (int)res[1];

            listenerThread = new Thread(new ThreadStart(DoListen));
            listenerThread.Start();
            UpdateStatus("Listener started");
        }
    }
}
