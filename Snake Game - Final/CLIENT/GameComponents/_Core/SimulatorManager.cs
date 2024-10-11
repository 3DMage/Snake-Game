using Contracts.DataContracts;
using Contracts.Interfaces;
using NetworkSimulator;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.SimulatorStuff
{
    public static class SimulatorManager
    {
        public static ISimulator simulator;
        public static Input input;
        public static Output output;

        public static void Initialize()
        {
            simulator = new Network();
            input = new Input(TimeSpan.Zero);
            output = new Output();
        }

        public static async Task<bool> Connect()
        {
            return await simulator.Connect();
        }

        public static void Disconnect()
        {
            simulator.Disconnect();
            simulator = new Network();
        }

        public static void SendInput()
        {
            simulator.SendInput(input);
        }

        public static List<Output> GetOutput()
        {
            List<Output> outputs = simulator.GetOutput();

            return outputs;
        }


        public static void CleanInput()
        {
            input.directionChangeRequestDatas.Clear();
            input.clientJoinRequestDatas.Clear();
            input.reviveSnakeRequestDatas.Clear();
        }
    }
}
