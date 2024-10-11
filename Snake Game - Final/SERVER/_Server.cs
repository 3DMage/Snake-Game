using Contracts.DataContracts;
using Contracts.Interfaces;
using GameComponents;
using GameSimulator;
using SerializerLibrary;
using SerializerLibrary.SerializerComponents;
using TCPServerLibrary.DataContracts;
using TCPServerLibrary.Server;

namespace ServerConsole
{

    public static class _Server
    {
        private static CancellationToken cancellationToken;
        public static int port;

        public static ISimulator simulator;
        private static List<byte> dataBuffer = new List<byte>();

        static TimeSpan SIMULATION_UPDATE_RATE_MS = TimeSpan.FromMilliseconds(1);



        static DateTime previousTime;

        static DateTime currentTime;
        static TimeSpan elapsedTime;

        static TCPServer server;



        public static void Initialize(CancellationToken cancellationToken, int port)
        {

            _Server.cancellationToken = cancellationToken;
            _Server.port = port;
            simulator = new Simulator();

        }

        public static void Run()
        {

            server = new TCPServer();
            server.OnClientConnectionReceived += Server_OnClientConnectionReceived;
            server.OnClientDisconnectionReceived += Server_OnClientDisconnectionReceived;
            server.Start(port);

            Console.WriteLine("Started server at port: " + port);


            long inputCount = 0;

            while (!cancellationToken.IsCancellationRequested)
            {
                currentTime = DateTime.Now;
                elapsedTime = currentTime - previousTime;


                #region Prepare Input
                List<Input> inputs = server.GetAllMessages();


                Input finalInput = new Input(elapsedTime);

                for (int i = 0; i < inputs.Count; i++)
                {
                    finalInput.clientJoinRequestDatas.AddRange(inputs[i].clientJoinRequestDatas);
                    finalInput.directionChangeRequestDatas.AddRange(inputs[i].directionChangeRequestDatas);
                    finalInput.reviveSnakeRequestDatas.AddRange(inputs[i].reviveSnakeRequestDatas);
                    for(int k = 0; k < inputs[i].reviveSnakeRequestDatas.Count; k++)
                    {
                        Console.WriteLine("REVIVE SNAKE REQUEST FOR: " + inputs[i].reviveSnakeRequestDatas[k].clientID);
                    }
                }
                 
             
             #endregion


                    Output output = new Output();
                simulator.SendInput(finalInput);
                List<Output> outputList = simulator.GetOutput();

                if (outputList.Count > 0)
                {
                    output = outputList[0];
                }


                OutputToByteConverter outputToByteConverter = new OutputToByteConverter();

                for (int i = 0; i < output.foodStateDatas.Count; i++)
                {
                    ClientMessage clientMessage = new ClientMessage();
                    clientMessage.ClientID = output.foodStateDatas[i].clientID;
                    Output tempOutput = new Output();
                    tempOutput.foodStateDatas.Add(output.foodStateDatas[i]);

                    clientMessage.Data = outputToByteConverter.ConvertToBytes(tempOutput);



                    server.SendMessageToClient(clientMessage);

                    Console.WriteLine("Sending FoodState to ClientID: " + clientMessage.ClientID);
                }

                for (int i = 0; i < output.snakeSystemStatesDatas.Count; i++)
                {
                    ClientMessage clientMessage = new ClientMessage();
                    clientMessage.ClientID = output.snakeSystemStatesDatas[i].sendToClientID;
                    Output tempOutput = new Output();
                    tempOutput.snakeSystemStatesDatas.Add(output.snakeSystemStatesDatas[i]);


                    clientMessage.Data = outputToByteConverter.ConvertToBytes(tempOutput);
                    server.SendMessageToClient(clientMessage);

                    Console.WriteLine("Sending SnakeSystemState to ClientID: " + clientMessage.ClientID);
                }

                output.connectedSignalDatas.Clear();
                output.foodStateDatas.Clear();
                output.snakeSystemStatesDatas.Clear();





                byte[] message = outputToByteConverter.ConvertToBytes(output);

                server.SendMessage(message);


                previousTime = DateTime.Now;

                Thread.Sleep(1);
            }

            server.Stop();
        }

        private static void Server_OnClientDisconnectionReceived(object? sender, ClientDisconnectionReceivedEventArgs e)
        {
            Console.WriteLine("Client has Disconnected:" + e.clientID);

            OutputToByteConverter outputToByteConverter = new OutputToByteConverter();

            Output output = simulator.RemoveSnakeRequest(e.clientID);

            byte[] message = outputToByteConverter.ConvertToBytes(output);

            server.SendMessage(message);
        }

        private static void Server_OnClientConnectionReceived(object? sender, ClientConnectionReceivedEventArgs e)
        {
            OutputToByteConverter outputToByteConverter = new OutputToByteConverter();

            Output output = new Output();

            ClientMessage clientMessage = new ClientMessage();
            clientMessage.ClientID = e.clientID;
            ConnectedSignalData connectedSignalData = new ConnectedSignalData(e.clientID);
            output.connectedSignalDatas.Add(connectedSignalData);


            clientMessage.Data = outputToByteConverter.ConvertToBytes(output);



            server.SendMessageToClient(clientMessage);
            Console.WriteLine("Sending First Messsage to ClientID: " + e.clientID);
        }
    }
}
