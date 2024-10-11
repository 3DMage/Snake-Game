using System.Collections.Concurrent;
using System.Net.Sockets;

namespace TCPClientLibrary.Base
{
    public class DataReader
    {
        public NetworkStream stream;
        private ConcurrentQueue<byte[]> messageQueue;
        private CancellationToken cancellationToken;

        public DataReader(TcpClient client, ConcurrentQueue<byte[]> queue, CancellationToken token)
        {
            stream = client.GetStream();
            messageQueue = queue;
            cancellationToken = token;
        }

        public void StartReading()
        {
            try
            {
                byte[] buffer = new byte[50000];
                while (!cancellationToken.IsCancellationRequested)
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead > 0)
                    { 
                        byte[] receivedBytes = new byte[bytesRead];
                        Array.Copy(buffer, receivedBytes, bytesRead);

                        messageQueue.Enqueue(receivedBytes);
                    }
                    Thread.Sleep(1);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in DataReader: " + e.Message);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }
    }
}