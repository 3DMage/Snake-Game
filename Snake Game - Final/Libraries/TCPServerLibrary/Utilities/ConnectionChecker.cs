using System.Net.Sockets;

namespace TCPServerLibrary.Utilities
{
    public class ConnectionChecker
    {
        public static bool IsClientConnected(TcpClient client)
        {
            if (client != null && client.Client != null && client.Client.Connected)
            {
                return IsSocketConnected(client.Client);
            }
            return false;
        }

        private static bool IsSocketConnected(Socket socket)
        {
            try
            {
                if (socket.Poll(0, SelectMode.SelectRead))
                {
                    byte[] buff = new byte[1];
                    if (socket.Receive(buff, SocketFlags.Peek) != 0)
                    {
                        return true;
                    }
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}