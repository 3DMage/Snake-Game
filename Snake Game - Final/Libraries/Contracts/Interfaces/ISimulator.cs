using Contracts.DataContracts;

namespace Contracts.Interfaces
{
    public interface ISimulator
    {
        bool IsNetwork();

        Task<bool> Connect();

        void Disconnect();

        List<Output> GetOutput();
         
        void SendInput(Input input);

        Output RemoveSnakeRequest(int clientID);
    }
}
