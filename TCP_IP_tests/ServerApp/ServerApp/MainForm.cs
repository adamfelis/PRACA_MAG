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

        // This subroutine sends the contents of the Broadcast textbox to all clients, if
        // it is not empty, and clears the textbox
        private void btnBroadcast_Click(object sender, System.EventArgs e)
        {
            if (textBox1.Text != "")
            {
                UpdateStatus("Broadcasting: " + textBox1.Text);
                Broadcast("BROAD|" + textBox1.Text);
                textBox1.Text = string.Empty;
            }
        }

        // This subroutine checks to see if username already exists in the clients 
        // Hashtable.  if it does, send a REFUSE message, otherwise confirm with a JOIN.
        private void ConnectUser(string userName, UserConnection sender)
        {
            if (clients.Contains(userName))
            {
                ReplyToSender("REFUSE", sender);
            }
            else
            {
                sender.Name = userName;
                UpdateStatus(userName + " has joined the chat.");
                clients.Add(userName, sender);
                listBox2.Items.Add(sender.Name);
                // Send a JOIN to sender, and notify all other clients that sender joined
                ReplyToSender("JOIN", sender);
                SendToClients("CHAT|" + sender.Name + " has joined the chat.", sender);
            }
        }

        // This subroutine notifies other clients that sender left the chat, and removes
        // the name from the clients Hashtable
        private void DisconnectUser(UserConnection sender)
        {
            UpdateStatus(sender.Name + " has left the chat.");
            SendToClients("CHAT|" + sender.Name + " has left the chat.", sender);
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
                    // Create a new user connection using TcpClient returned by
                    // TcpListener.AcceptTcpClient()
                    UserConnection client = new UserConnection(listener.AcceptTcpClient());
                    // Create an event handler to allow the UserConnection to communicate
                    // with the window.
                    client.LineReceived += new LineReceive(OnLineReceived);
                    //AddHandler client.LineReceived, AddressOf OnLineReceived;
                    //UpdateStatus("new connection found: waiting for log-in");
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

        // Concatenate all the client names and send them to the user who requested user list
        private void ListUsers(UserConnection sender)
        {
            UserConnection client;
            string strUserList;
            UpdateStatus("Sending " + sender.Name + " a list of users online.");
            strUserList = "LISTUSERS";
            // All entries in the clients Hashtable are UserConnection so it is possible
            // to assign it safely.

            foreach (DictionaryEntry entry in clients)
            {
                client = (UserConnection)entry.Value;
                strUserList = strUserList + "|" + client.Name;
            }

            // Send the list to the sender.
            ReplyToSender(strUserList, sender);
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
            return;
            dataArray = dataArray[0].Split((char)124);

            // dataArray(0) is the command.
            switch (dataArray[0])
            {
                case "CONNECT":
                    ConnectUser(dataArray[1], sender);
                    break;
                case "CHAT":
                    SendChat(dataArray[1], sender);
                    break;
                case "DISCONNECT":
                    DisconnectUser(sender);
                    break;
                case "REQUESTUSERS":
                    ListUsers(sender);
                    break;
                default:
                    // Message is junk do nothing with it.
                    break;
            }
        }

        // This subroutine sends a response to the sender.
        private void ReplyToSender(string strMessage, UserConnection sender)
        {
            sender.SendData(strMessage);
        }

        // Send a chat message to all clients except sender.
        private void SendChat(string message, UserConnection sender)
        {
            UpdateStatus(sender.Name + ": " + message);
            SendToClients("CHAT|" + sender.Name + ": " + message, sender);
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
                // Exclude the sender.
                if (client.Name != sender.Name)
                {
                    client.SendData(strMessage);
                }
                //else
                //{
                //    client.SendData(res1.ToString());
                //    client.SendData(res2.ToString());
                //}
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
            object result = null;
            mlApp.Feval("CalcInverse", 2, out result, 2, 5);
            object[] res = result as object[];
            res1 = (int)res[0];
            res2 = (int)res[1];

            listenerThread = new Thread(new ThreadStart(DoListen));
            listenerThread.Start();
            UpdateStatus("Listener started");
        }
    }
}
