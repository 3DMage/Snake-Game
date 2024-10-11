using Contracts.Interfaces;
using GameComponents;
using System.Collections.Generic;

namespace Client.GameComponents.Network
{
    public static class _Client
    {
        private static Sc_SnakeGame snakeGame;
      //  private static ISimulator simulator;

        //public static List<byte[]> RawMessagesFromServer;

        public static bool isInitialized = false;


        public static void Initialize() 
        {
            //RawMessagesFromServer = new List<byte[]>();
        }

        public static void Inject(Sc_SnakeGame snakeGame, ISimulator simulator)
        {
            _Client.snakeGame = snakeGame;
         //   _Client.simulator = simulator;
        }

        //public async static Task Connect()
        //{
        //    //? MAY NEED THAT DYNAMIC CODE FOR IP ADDRESS CONFIG?
        //  await simulator.ConnectAsync("127.0.0.1", port);
        //}

        //public static void Disconnect() 
        //{
        //    simulator.Disconnect();
        //}

        //public static void SendMessage(DataPacketBase message)
        //{
        //    simulator.SendMessage(message.Serialize());
        //}

        //public static void GetAndProcessMessages()
        //{
        //    List<byte[]> RawMessagesFromServer = simulator.GetMessages();

        //    for(int i =0; i <  RawMessagesFromServer.Count; i++) 
        //    {
        //        DataPacketType dataPacketType = MessageTypeDetector.DetermineDataPacketType(RawMessagesFromServer[i]);

        //        receivedMessageProcessingDirectory[dataPacketType](RawMessagesFromServer[i]);
        //    }
           
        //}

       


















































    }
}
