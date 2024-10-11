namespace TCPServerLibrary.DataContracts
{
    public class ClientMessage
    {
        public ClientMessage() 
        {
            Data = new byte[0];
            ClientID = -1;
        }

        public byte[] Data { get; set; }
        public int ClientID { get; set; }
    }
}
