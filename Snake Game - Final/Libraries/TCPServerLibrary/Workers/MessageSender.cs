using System.Collections.Concurrent;
using System.Net.Sockets;
using TCPServerLibrary.Client;
using TCPServerLibrary.DataContracts;
using TCPServerLibrary.Interfaces;

namespace TCPServerLibrary.Workers
{
    public class MessageSender : IWorker
    {
        private readonly ConcurrentDictionary<int, TcpClient> clients;
        private readonly ConcurrentQueue<byte[]> messages;
        private readonly ConcurrentQueue<ClientMessage> specificClientMessages;
        private readonly CancellationToken cancellationToken;

        public MessageSender(ConcurrentDictionary<int, TcpClient> clients, ConcurrentQueue<byte[]> messages, ConcurrentQueue<ClientMessage> specificClientMessages, CancellationToken cancellationToken)
        {
            this.clients = clients;
            this.messages = messages;
            this.specificClientMessages = specificClientMessages;
            this.cancellationToken = cancellationToken;
        }

        public void Run()
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                SendMessagesToAllClients();
                SendMessagesToSpecificClients();
                PauseBrieflyToReduceCPULoad();
            }
        }

        private void SendMessagesToSpecificClients()
        {
            while (specificClientMessages.TryDequeue(out var message))
            { 
                if (clients.TryGetValue(message.ClientID, out TcpClient client))
                {
                    ClientWriter writer = new ClientWriter(client);
                    writer.Send(message.Data);
                } 
            }
        }

        //private void SendMessagesToAllClients()
        //{
        //    while (messages.TryDequeue(out var message))
        //    {
        //        foreach (var client in clients.Values)
        //        {
        //            ClientWriter writer = new ClientWriter(client);
        //            if (message != null)
        //            {
        //                writer.Send(message);
        //            }
        //        }
        //    }
        //}


        private void SendMessagesToAllClients()
        {
            byte[] message;
            while (messages.TryDequeue(out message))
            {
                Parallel.ForEach(clients.Values, client =>
                {
                    ClientWriter writer = new ClientWriter(client);
                    writer.Send(message);
                });
            }
        }


        private void PauseBrieflyToReduceCPULoad()
        {
            Thread.Sleep(1);
        }
    }
}