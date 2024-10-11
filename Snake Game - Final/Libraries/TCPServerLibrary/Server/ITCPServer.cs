using Contracts.DataContracts;
using System;
using TCPServerLibrary.DataContracts;

namespace TCPServerLibrary.Server
{
    public interface ITCPServer
    {
        void Start(int port);

        void Stop();
         
        List<Input> GetAllMessages();

        void SendMessage(byte[] message);

        void SendMessageToClient(ClientMessage message);

        event EventHandler<ClientConnectionReceivedEventArgs> OnClientConnectionReceived;


        event EventHandler<ClientDisconnectionReceivedEventArgs> OnClientDisconnectionReceived;
    }


    public class ClientConnectionReceivedEventArgs : EventArgs
    {
        public int clientID { get; set; } 

        public ClientConnectionReceivedEventArgs(int clientID) 
        { 
            this.clientID = clientID;
        }
    }


    public class ClientDisconnectionReceivedEventArgs : EventArgs
    {
        public int clientID { get; set; }

        public ClientDisconnectionReceivedEventArgs(int clientID)
        {
            this.clientID = clientID;
        }
    }
}
