using TCPServerLibrary.Interfaces;
using TCPServerLibrary.Utilities;
using System.Collections.Concurrent;
using System.Net.Sockets;

namespace TCPServerLibrary.Workers
{ 
    public class DisconnectClientsManager : IWorker
    {
        private readonly DisconnectedClientRemover clientManager;
        private readonly CancellationToken cancellationToken;

        public DisconnectClientsManager(ConcurrentDictionary<int, TcpClient> clients, BlockingCollection<int> ClientDisconnectNotifications, CancellationToken cancellationToken)
        {

            clientManager = new DisconnectedClientRemover(clients, ClientDisconnectNotifications);
            this.cancellationToken = cancellationToken;
        }

        public void Run()
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    clientManager.CheckAndRemoveDisconnectedClients();
                    Thread.Sleep(5); 
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Connection monitor is shutting down.");
            }
        }
    }
}