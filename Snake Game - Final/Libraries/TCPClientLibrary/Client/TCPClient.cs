using System.Collections.Concurrent;
using System.Net.Sockets;
using TCPClientLibrary.Base;

namespace TCPClientLibrary.Client
{
    public class TCPClient : IDisposable, ITCPClient
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ConcurrentQueue<byte[]> sendQueue = new ConcurrentQueue<byte[]>();
        private readonly ConcurrentQueue<byte[]> receiveQueue = new ConcurrentQueue<byte[]>();
        private TCPConnectionManager connectionManager;
        private Thread writerThread;
        private Thread readerThread;

        DataWriter writer;
        DataReader reader;

        public async Task<bool> ConnectAsync(string ipAddress, int port)
        {
            try
            {
                connectionManager = new TCPConnectionManager(ipAddress, port, cancellationTokenSource.Token);
                TcpClient client = await connectionManager.ConnectAsync();
                if (client == null)
                {
                    return false;
                }

                InitializeDataHandlers(client);
                return true;
            }
            catch
            {
                Dispose();
                return false;
            }
        }

        private void InitializeDataHandlers(TcpClient client)
        {
            writer = new DataWriter(client, sendQueue, cancellationTokenSource.Token);
            reader = new DataReader(client, receiveQueue, cancellationTokenSource.Token);

            writerThread = new Thread(writer.StartWriting) { IsBackground = true };
            readerThread = new Thread(reader.StartReading) { IsBackground = true };

            writerThread.Start();
            readerThread.Start();
        }

        public byte[] GetMessage()
        {
            byte[] message = null;
            if (receiveQueue.TryDequeue(out message))
            {
                return message;
            }
            return null; // Return null to clearly indicate no message is available
        }

        public List<byte[]> GetMessages()
        {
            List<byte[]> messages = new List<byte[]>();
            byte[] message;
            while (receiveQueue.TryDequeue(out message))
            {
                messages.Add(message);
            }
            return messages;
        }

        public void SendMessage(byte[] message)
        {
            if (message != null)
            {
                sendQueue.Enqueue(message);
            }
        }

        public void Disconnect()
        {
            cancellationTokenSource.Cancel();

            if (writerThread != null)
            {
                writerThread.Join();
            }

            if (readerThread != null)
            {
                readerThread.Join();
            }

            if (connectionManager != null)
            {
                connectionManager.Disconnect();
            }

            Dispose();
        }

        public void Dispose()
        {
            cancellationTokenSource.Dispose();

            if (connectionManager != null)
            {
                
                try
                {
                    if (connectionManager.client != null)
                    {
                        connectionManager.client.Close();
                    }
                    connectionManager.Dispose();
                    connectionManager = null;
                }
                catch (Exception e) 
                { 
                    
                }
            }

            try
            {
                if (writerThread != null)
                {
                    writerThread.Abort();
                }
                if (readerThread != null)
                {
                    readerThread.Abort();
                }
            }
            catch (Exception e)
            {

            }
            

            writerThread = null;
            readerThread = null;
        }
    }
}
