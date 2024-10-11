namespace TCPClientLibrary.Client
{
    public interface ITCPClient
    {
        Task<bool> ConnectAsync(string ipAddress, int port);
         
        byte[] GetMessage();

        List<byte[]> GetMessages();

        void SendMessage(byte[] message);

        void Disconnect(); 
    }
}
