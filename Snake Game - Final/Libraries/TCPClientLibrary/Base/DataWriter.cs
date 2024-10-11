using System.Collections.Concurrent;
using System.Net.Sockets;

namespace TCPClientLibrary.Base
{
    public class DataWriter
    {
        public NetworkStream stream;
        private ConcurrentQueue<byte[]> messageQueue;
        private CancellationToken cancellationToken;

        public DataWriter(TcpClient client, ConcurrentQueue<byte[]> queue, CancellationToken token)
        {
            stream = client.GetStream();
            messageQueue = queue;
            cancellationToken = token;
        }

        public void StartWriting()
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    if (messageQueue.TryDequeue(out byte[] message))
                    { 
                        stream.Write(message, 0, message.Length); 
                    }
                    Thread.Sleep(1);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in DataWriter: " + e.Message);
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