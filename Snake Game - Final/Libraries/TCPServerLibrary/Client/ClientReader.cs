using System.Net.Sockets;

namespace TCPServerLibrary.Client
{
    public class ClientReader
    {
        public   TcpClient client;

        public ClientReader(TcpClient client)
        {
            this.client = client;
        }

        public bool IsClientReady()
        {
            return client.Available > 0;
        }

        public byte[] ExtractMessage()
        {
            byte[] buffer = new byte[50000];
            int bytesRead = TryReadFromClient(buffer);
            if (bytesRead > 0)
            {
                byte[] receivedBytes = new byte[bytesRead];
                Array.Copy(buffer, receivedBytes, bytesRead);
                return receivedBytes;
            }
            return null;
        }

        public List<byte[]> ExtractAllMessages()
        {
            var messages = new List<byte[]>();
            while (IsClientReady())
            {
                byte[] message = ExtractMessage();
                if (message != null)
                {
                    if (message.Length>0)
                    {
                        messages.Add(message);
                    }
                }
            }
            return messages;
        }

        private int TryReadFromClient(byte[] buffer)
        {
            try
            {
                return client.GetStream().Read(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading from client: {ex.Message}");
                return 0;
            }
        }
    }
}