using System.Net.Sockets;

namespace TCPClientLibrary.Base
{
    public class TCPConnectionManager : IDisposable
    {
        public TcpClient client;
        private readonly string server;
        private readonly int port;
        private readonly CancellationToken cancellationToken;
        private bool disposed = false;

        public TCPConnectionManager(string server, int port, CancellationToken token)
        {
            this.server = server;
            this.port = port;
            cancellationToken = token;
        }

        public async Task<TcpClient> ConnectAsync()
        {
            if (DisposeCheck())
            {
                return null;
            }

            try
            {
                client = new TcpClient();
                await client.ConnectAsync(server, port, cancellationToken).ConfigureAwait(false);
                if (client.Connected)
                {
                    Console.WriteLine("Connected to the server.");
                    return client;
                }
                else
                {
                    Console.WriteLine("Connection failed but no exception was thrown.");
                    DisposeClient();
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Connection canceled.");
                DisposeClient();
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not connect to server: " + e.Message);
                DisposeClient();
            }

            return null;
        }

        public void Disconnect()
        {
            if (DisposeCheck())
            {
                return;
            }

            DisposeClient();
            Console.WriteLine("Disconnected from server.");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    DisposeClient();
                }
                disposed = true;
            }
        }

        private bool DisposeCheck()
        {
            if (disposed)
            {
                Console.WriteLine("Attempted to use a disposed instance of TCPConnectionManager.");
                return true; // Indicate that the object is disposed
            }
            return false; // Indicate that the object is not disposed
        }
         
        private void DisposeClient()
        {
            if (client != null)
            {
                client.Close();
                client = null;
            }
        }

        ~TCPConnectionManager()
        {
            Dispose(false);
        }
    }
}