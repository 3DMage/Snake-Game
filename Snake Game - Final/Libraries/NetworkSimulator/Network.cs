using Contracts.DataContracts;
using Contracts.Interfaces;
using SerializerLibrary;
using TCPClientLibrary.Client;

namespace NetworkSimulator
{

    public class Network : ISimulator
    {
        ITCPClient client;

        ByteToOutputConverter OutputDeserializer;

        public Network()
        {
            client = new TCPClient();
            OutputDeserializer = new ByteToOutputConverter();
        }

        public async Task<bool> Connect()
        {
            return await client.ConnectAsync("127.0.0.1", 3000);
        }

        public void Disconnect()
        {
            client.Disconnect();

        }

        public bool IsNetwork()
        {
            return true;
        }

        public List<Output> GetOutput()
        {
            List<byte[]> messages = client.GetMessages();

            for (int i = 0; i < messages.Count; i++)
            {
                OutputDeserializer.ReceiveData(messages[i]);
            }

            return OutputDeserializer.GetDeserializedOutputs(true);
        }


        public void SendInput(Input input)
        {
            InputToByteConverter inputToByteConverter = new InputToByteConverter();

            byte[] message = inputToByteConverter.ConvertToBytes(input);

            client.SendMessage(message);
        }

        public Output RemoveSnakeRequest(int clientID)
        {
            throw new NotImplementedException();
        }

    }
}