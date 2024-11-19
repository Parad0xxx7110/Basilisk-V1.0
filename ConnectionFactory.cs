using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.IO;

public class ConnectionFactory
{
    // This class handles connections to peers and returns a usable stream to pass to the methods of the PeerWireProtocol class
    // to send the different messages to peers.

    private const int DefaultTimeout = 5000;  // Timeout 

    public async Task<NetworkStream> CreateConnectionAsync(string peerAddress, int peerPort, int timeout = DefaultTimeout)
    {
        try
        {
            
            TcpClient client = new TcpClient();

           
            var connectionTask = client.ConnectAsync(peerAddress, peerPort);
            var timeoutTask = Task.Delay(timeout);

           
            if (await Task.WhenAny(connectionTask, timeoutTask) == timeoutTask)
            {
                throw new TimeoutException($"Connection to {peerAddress}:{peerPort} timed out.");
            }

           
            Console.WriteLine($"Connected to peer at {peerAddress}:{peerPort}");
            return client.GetStream();
        }
        catch (TimeoutException ex)
        {
            Console.WriteLine($"Connection failed: {ex.Message}");
            throw;
        }
        catch (SocketException ex)
        {
            Console.WriteLine($"Socket error while connecting to {peerAddress}:{peerPort}: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to connect to peer: {ex.Message}");
            throw;
        }
    }
}
