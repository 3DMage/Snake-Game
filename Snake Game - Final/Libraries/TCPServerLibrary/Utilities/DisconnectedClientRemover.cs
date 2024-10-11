using System.Collections.Concurrent;
using System.Net.Sockets;

namespace TCPServerLibrary.Utilities
{
    public class DisconnectedClientRemover
    {
        private readonly BlockingCollection<int> ClientDisconnectNotifications;
        private readonly ConcurrentDictionary<int, TcpClient> clients;

        public DisconnectedClientRemover(ConcurrentDictionary<int, TcpClient> clients, BlockingCollection<int> clientDisconnectNotifications)
        {
            this.clients = clients;
            ClientDisconnectNotifications = clientDisconnectNotifications;
        }

        public void CheckAndRemoveDisconnectedClients()
        {
            List<int> disconnectedClientIds = GetDisconnectedClientIds();
            RemoveAndCleanUpClients(disconnectedClientIds);
        }

        private List<int> GetDisconnectedClientIds()
        {
            List<int> disconnectedClientIds = new List<int>();
            foreach (KeyValuePair<int, TcpClient> clientPair in clients)
            {
                if (!ConnectionChecker.IsClientConnected(clientPair.Value))
                {

                    ClientDisconnectNotifications.Add(clientPair.Key);

                    disconnectedClientIds.Add(clientPair.Key);
                    Console.WriteLine($"Client disconnected: {clientPair.Key}");
                }
            }
            return disconnectedClientIds;
        }

        private void RemoveAndCleanUpClients(List<int> disconnectedClientIds)
        {
            foreach (int clientId in disconnectedClientIds)
            {
                if (clients.TryRemove(clientId, out TcpClient removedClient))
                {
                    CloseClientConnection(removedClient, clientId);
                }
            }
        }

        private static void CloseClientConnection(TcpClient client, int clientId)
        {
            client.Close();
            client.Dispose();
            Console.WriteLine($"Resources freed for client: {clientId}");
        }
    }
}