namespace server
{
    using System;
    using System.Net.Sockets;
    using System.Text;
    using Common;

    public class Program
    {
        private static int RequestCount = 0;
        private static TcpListener ServerSocket = new TcpListener(Constants.ServerAddress, Constants.ServerPort);
        private static TcpClient ClientSocket = default(TcpClient);

        public static void Main(string[] args)
        {
            ServerSocket.Start();
            Console.WriteLine(" >> Server Started");
            ClientSocket = ServerSocket.AcceptTcpClient();
            Console.WriteLine(" >> Accepting connection from client");

            while ((true))
            {
                try
                {
                    RequestCount++;
                    var networkStream = ClientSocket.GetStream();
                    var bytesFrom = new byte[10025];
                    networkStream.Read(bytesFrom, 0, (int)ClientSocket.ReceiveBufferSize);
                    var dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                    dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf(Constants.EndMessageSeparator));
                    Console.WriteLine(" >> Data from client - " + dataFromClient);
                    var serverResponse = string.Format("Last Message from client '{0}'", dataFromClient);
                    var sendBytes = Encoding.ASCII.GetBytes(serverResponse);
                    networkStream.Write(sendBytes, 0, sendBytes.Length);
                    networkStream.Flush();
                    Console.WriteLine(" >> " + serverResponse);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            ClientSocket.Close();
            ServerSocket.Stop();
            Console.WriteLine(" >> exit");
            Console.ReadLine();
        }
    }
}
