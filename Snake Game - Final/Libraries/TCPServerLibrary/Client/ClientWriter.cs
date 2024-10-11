using System.Net.Sockets;

namespace TCPServerLibrary.Client
{
    public class ClientWriter
    {
        private readonly TcpClient client;

        public ClientWriter(TcpClient client)
        {
            this.client = client; 
        }

        public void Send(byte[] message)
        {
            if (client.Connected)
            {  
                try
                {
                    NetworkStream stream = client.GetStream();
                    if(stream != null)
                    {
                        stream.Write(message, 0, message.Length);
                    }
                    else
                    {
                        Console.WriteLine("STREAM WAS NULL!");
                    }

                }
                catch (Exception ex)
                {
                   // Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}
