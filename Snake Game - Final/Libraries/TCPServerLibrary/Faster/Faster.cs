using System.Collections.Concurrent;
using System.Net.Sockets;
using TCPServerLibrary.Interfaces;
using TCPServerLibrary.Client;
using System.Net;
using SerializerLibrary;
using Contracts.DataContracts;


namespace TCPServerLibrary.Faster
{


    public class FastClientConnectionListener : IWorker
    {
      
        private readonly TcpListener tcpListener;
        private readonly ConcurrentDictionary<int, TcpClient> clients;
        private readonly BlockingCollection<int> newClientNotifications;
        private readonly ConcurrentQueue<Input> messagesQueue;
        private readonly CancellationToken cancellationToken;
        private int nextIntID = 0;

        public FastClientConnectionListener(int port, ConcurrentDictionary<int, TcpClient> clients, BlockingCollection<int> newClientNotifications,   ConcurrentQueue<Input> messagesQueue, CancellationToken cancellationToken)
        {
      
            tcpListener = new TcpListener(IPAddress.Any, port);
            this.clients = clients;
            this.messagesQueue = messagesQueue;
            this.newClientNotifications = newClientNotifications;
            this.cancellationToken = cancellationToken;
            tcpListener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        }

        public void Run()
        {
            tcpListener.Start();
            while (!cancellationToken.IsCancellationRequested)
            {
                ListenForNewClient();
                PauseBrieflyToReduceCPULoad();
            }
            tcpListener.Stop();
        }

        private void ListenForNewClient()
        {
            if (tcpListener.Pending())
            {
                var client = tcpListener.AcceptTcpClient();
                var clientId = nextIntID++;
                if (clients.TryAdd(clientId, client))
                {
                    Console.WriteLine($"New client connected: {clientId}");
                    newClientNotifications.Add(clientId);
                    // Start a new thread for each client
                    var clientThread = new Thread(() => HandleClient(client, clientId));
                    clientThread.Start();
                }
            }
        }

        private void HandleClient(TcpClient client, int clientId)
        {
            // Replace MessageReceiver with your method to handle client
            var messageReceiver = new FastMessageReceiver(clients, clientId, messagesQueue, cancellationToken);
            messageReceiver.Run();
        }

        private void PauseBrieflyToReduceCPULoad()
        {
            Thread.Sleep(100);
        }
    }




    public class FastMessageReceiver : IWorker
    {
  
        private readonly ByteToInputConverter converter;
        private readonly TcpClient client;
        private readonly ConcurrentQueue<Input> messagesQueue;
        private readonly CancellationToken cancellationToken;
        private readonly ClientReader clientReader;  
        private readonly int clientId;

        public FastMessageReceiver(ConcurrentDictionary<int, TcpClient> clients, int clientId, ConcurrentQueue<Input> messagesQueue,  CancellationToken cancellationToken)
        {
          
            converter = new ByteToInputConverter();
            this.client = clients[clientId];
            this.clientId = clientId;
            this.messagesQueue = messagesQueue;
            this.cancellationToken = cancellationToken;
          clientReader = new ClientReader(client);  
        }

        public void Run()
        {
            while (!cancellationToken.IsCancellationRequested && client.Connected)
            {
                ReadMessagesFromClient();
                PauseBrieflyToReduceCPULoad();
 
            }
        }

        private void ReadMessagesFromClient()
        {
       
                if (clientReader.IsClientReady())
                {
                    var messages = clientReader.ExtractAllMessages();
                    if (messages != null)
                    {
                        foreach (var message in messages)
                        {

                            if (message != null)
                            {
                                if (message.Length > 0)
                                {
                                    converter.ReceiveData(message);

                                }

                            }
                        }

                        Input input = new Input();
                        List<Input> inputs = converter.GetDeserializedInputs(true);

                        for (int i = 0; i < inputs.Count; i++)
                        {

                            input.directionChangeRequestDatas.AddRange(inputs[i].directionChangeRequestDatas);

                            input.clientJoinRequestDatas.AddRange(inputs[i].clientJoinRequestDatas);

                            input.reviveSnakeRequestDatas.AddRange(inputs[i].reviveSnakeRequestDatas);

                    }

                    messagesQueue.Enqueue(input);
                    }
                }
           
        }

        private void PauseBrieflyToReduceCPULoad()
        {
            Thread.Sleep(1);
        }
    }


}
