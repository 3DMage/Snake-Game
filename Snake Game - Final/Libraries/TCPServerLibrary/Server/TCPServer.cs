using TCPServerLibrary.Interfaces;
using TCPServerLibrary.Workers;
using System.Collections.Concurrent;
using System.Net.Sockets;
using TCPServerLibrary.DataContracts;
using TCPServerLibrary.Faster;
using Contracts.DataContracts;

namespace TCPServerLibrary.Server
{
    public class TCPServer : IDisposable, ITCPServer
    {
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ConcurrentQueue<Input> incomingMessages = new ConcurrentQueue<Input>();
        private readonly ConcurrentQueue<byte[]> outgoingMessages = new ConcurrentQueue<byte[]>();
        private readonly ConcurrentQueue<ClientMessage> specificClientMessages = new ConcurrentQueue<ClientMessage>();
        private readonly ConcurrentDictionary<int, TcpClient> clients = new ConcurrentDictionary<int, TcpClient>();
        private BlockingCollection<int> newClientNotifications = new BlockingCollection<int>();
        private BlockingCollection<int> ClientDisconnectNotifications = new BlockingCollection<int>();


        private List<Thread> workerThreads = new List<Thread>();
        private bool IsDisposed;

        public event EventHandler<ClientConnectionReceivedEventArgs> OnClientConnectionReceived;
        public event EventHandler<ClientDisconnectionReceivedEventArgs> OnClientDisconnectionReceived;

        public TCPServer( )
        {
            IsDisposed = false;
        }

        public void Start(int port)
        {
            IsDisposed = false;
            InitializeServerWorkers(port);
            StartListeningForNewClients();
            StartListeningForDisconnectingClients();
        }

        private void InitializeServerWorkers(int port)
        { 
            StartWorker(new FastClientConnectionListener(port, clients, newClientNotifications, incomingMessages, cancellationTokenSource.Token));
            StartWorker(new MessageSender(clients, outgoingMessages, specificClientMessages, cancellationTokenSource.Token));
     
            StartWorker(new DisconnectClientsManager(clients, ClientDisconnectNotifications, cancellationTokenSource.Token));
        }

        private void StartWorker(IWorker worker)
        {
            Thread thread = new Thread(worker.Run);
            workerThreads.Add(thread);
            thread.Start();
        }
         
 

        public List<Input> GetAllMessages()
        {
            List<Input> allMessages = new List<Input>();
            while (incomingMessages.TryDequeue(out Input message))
            {
                allMessages.Add(message);
            }
            return allMessages;  
        }

       
        protected void RaiseOnClientConnectionReceived(int clientID)
        {
            var args = new ClientConnectionReceivedEventArgs(clientID);

            // Check if there are any subscribers
            OnClientConnectionReceived?.Invoke(this,args);
        }

        protected void RaiseOnClientDisconnectionReceived(int clientID)
        {
            var args = new ClientDisconnectionReceivedEventArgs(clientID);

            // Check if there are any subscribers
            OnClientDisconnectionReceived?.Invoke(this, args);
        }

       

        public void StartListeningForNewClients()
        {
            Task.Run(() =>
            {
                try
                {
                    foreach (var clientId in newClientNotifications.GetConsumingEnumerable())
                    {
                        if (clients.TryGetValue(clientId, out TcpClient client))
                        {
                            try
                            {
                                RaiseOnClientConnectionReceived(clientId);
                                
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error handling client {clientId}: {ex.Message}");
                               
                            }
                        }
                    }
                }
                catch (InvalidOperationException ex)
                {
                 
                    Console.WriteLine($"Client Connect Listener stopped: {ex.Message}");
                }
            });
        }



        public void StartListeningForDisconnectingClients()
        {
            Task.Run(() =>
            {
                try
                {
                    foreach (var clientId in ClientDisconnectNotifications.GetConsumingEnumerable())
                    {
                      
                            try
                            {
                            RaiseOnClientDisconnectionReceived(clientId);

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error handling client {clientId}: {ex.Message}");

                            }
                         
                    }
                }
                catch (InvalidOperationException ex)
                {

                    Console.WriteLine($"Client Disconnect Listener stopped: {ex.Message}");
                }
            });
        }


        public void SendMessage(byte[] message)
        {
            if (message != null)
            {
                if (message.Length > 0)
                {
                    outgoingMessages.Enqueue(message);
                }
            }
        }
         

        public void SendMessageToClient(ClientMessage message)
        {
            if(message != null)
            {
                if (message.Data.Length > 0)
                {
                    specificClientMessages.Enqueue(message);
                }
            }
        }

        public void Stop()
        {
            Dispose(); 
        }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                cancellationTokenSource.Cancel();
                workerThreads.ForEach(thread => thread.Join());

                newClientNotifications.CompleteAdding();

                cancellationTokenSource.Dispose();
                IsDisposed = true;
                Console.WriteLine("Server has stopped.");
            }
        }

       
    }
}