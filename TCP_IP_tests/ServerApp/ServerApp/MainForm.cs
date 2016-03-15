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
using System.Globalization;

namespace ServerApp
{
    public class UpdateData
    {
        public ListBox Control 
        {
            get;
            set;
        }
        public string Message { get; set; }
    }

    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            //adjustCulture();
        }

        const int PORT_NUM = 10000;
        private Hashtable clients = new Hashtable();
        private TcpListener listener;
        private Thread listenerThread;
        private MLApp.MLApp mlApp;
        private int res1;
        private int res2;

        private void adjustCulture()
        {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";

            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
        }


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
            UpdateUsers(sender.Name);
            SendToClients("JOIN|");
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
                //do
                //{
                    UserConnection client = new UserConnection(listener.AcceptTcpClient());
                    client.LineReceived += new LineReceive(OnLineReceived);
                //} while (true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
                    deserializeData(dataArray[1]);
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

        private void deserializeData(string msg)
        {
            string[] dataArray;
            dataArray = msg.Split('\n');
            float[] tspan = wrapToFloatArray(dataArray[0]);
            float[] y0 = wrapToFloatArray(dataArray[1]);

            callODEInMatlab(tspan, y0);
        }

        private float[] wrapToFloatArray(string data)
        {
            data = data.Substring(data.IndexOf('=') + 1);
            string[] t = data.Split(';');
            int n = t.Length;
            float[] toRet = new float[n];
            for (int i = 0; i < n; i++)
            {
                toRet[i] = (float)Double.Parse(t[i], CultureInfo.InvariantCulture);
            }
            return toRet;
        }

        private void callODEInMatlab(float[] tspan, float[] y0)
        {
            object result = null;
            mlApp.Feval("ODE", 2, out result, tspan, y0);
            object[] res = result as object[];
            float[,] t = res[0] as float[,];
            float[,] y = res[1] as float[,];
            float b = 10;
            StringBuilder sb = new StringBuilder();
            int n = y.GetLength(0) - 1;
            int m = y.GetLength(1);
            for (int i = 0; i < m; i++)
            {
                sb.Append(y[n, i]);
                if (i < m - 1)
                    sb.Append(';');
            }
            SendToClients("DATA|" + sb.ToString());
        }


        // Send a chat message to all clients except sender.
        private void SendChat(string message, UserConnection sender)
        {
            UpdateStatus(sender.Name + ": " + message);

            //We do not need to inform other clients
            //SendToClients("DATA|" + sender.Name + ": " + message, sender);
        }

        // This subroutine sends a message to all attached clients except the sender.
        private void SendToClients(string strMessage)
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

        private Thread demoThread = null;
        delegate void UpdateControlCallback(UpdateData updateData);
        private void UpdateStatus(string statusMessage)
        {
            demoThread = new Thread(ThreadProcSafe);
            demoThread.Start(new UpdateData() {Control = listBox1, Message = statusMessage });
        }

        private void UpdateUsers(string statusMessage)
        {
            demoThread = new Thread(ThreadProcSafe);
            demoThread.Start(new UpdateData() { Control = listBox2, Message = statusMessage });
        }

        private void ThreadProcSafe(object updateData)
        {
            UpdateControl(updateData as UpdateData);
        }

        private void UpdateControl(UpdateData updateData)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (updateData.Control.InvokeRequired)
            {
                UpdateControlCallback d = new UpdateControlCallback(UpdateControl);
                Invoke(d, new object[] { updateData });
            }
            else
            {
                updateData.Control.Items.Add(updateData.Message);
            }
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            mlApp = new MLApp.MLApp();
            string path =
    @"C:\Users\Qba\Documents\Studia_magisterskie\magisterka\PRACA_MAG";
            mlApp.Execute("cd " + path);
            //object result = null;
            //float[] tspan = new float[]
            //{
            //    0, 1
            //};
            //float[] y0 = new float[]
            //{
            //    1, 0, 0, 0, 1, 0, 0, 0, 1
            //};
            //mlApp.PutWorkspaceData("tspan", "base", tspan);
            //mlApp.PutWorkspaceData("y0", "base", y0);
            //mlApp.Feval("ODE", 2, out result, tspan, y0);
            //object[] res = result as object[];
            //res1 = (int)res[0];
            //res2 = (int)res[1];

            listenerThread = new Thread(new ThreadStart(DoListen));
            listenerThread.Start();
            UpdateStatus("Listener started");
        }
    }
}
