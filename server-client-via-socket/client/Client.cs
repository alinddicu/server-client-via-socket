namespace server_client_via_socket
{
    using Common;
    using System;
    using System.Net.Sockets;
    using System.Windows.Forms;

    public partial class Client : Form
    {
        private readonly TcpClient _clientSocket = new TcpClient();
        
        public Client()
        {
            InitializeComponent();
        }

        private void Client_Load(object sender, EventArgs e)
        {
            messageFromServer("Client Started");
            _clientSocket.Connect(Constants.ServerAddress.ToString(), Constants.ServerPort);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NetworkStream serverStream = _clientSocket.GetStream();
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(toServer.Text + Constants.EndMessageSeparator);
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();

            byte[] inStream = new byte[10025];
            serverStream.Read(inStream, 0, (int)_clientSocket.ReceiveBufferSize);
            string returndata = System.Text.Encoding.ASCII.GetString(inStream);
            messageFromServer(returndata);
            toServer.Text = string.Empty;
            toServer.Focus();
        }

        private void messageFromServer(string message)
        {
            fromServer.Text = fromServer.Text + Environment.NewLine + " >> " + message;
        } 
    }
}
