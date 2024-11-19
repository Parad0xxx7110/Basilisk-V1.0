using Basilisk;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class Client
{
    public static async Task Main()
    {
        
        string serverIP = "localhost";
        int serverPort = 6881;

        
        TcpClient client = new TcpClient();
        await client.ConnectAsync(serverIP, serverPort);
        Console.WriteLine("Connected to server.");

        NetworkStream stream = client.GetStream();

        string peerID = "-CLIENT-01234567890-"; // 20 bytes
        byte[] infoHash = Encoding.ASCII.GetBytes("12345678901234567890"); // 20 bytes

        
        await SendHandshake(stream, infoHash, peerID);

        
        int totalPieces = 2000;
        Bitfield bitfield = new Bitfield(totalPieces);

        
            bitfield.SetPiece(8);  // Piece 9
            bitfield.SetPiece(9);  // Piece 10
            bitfield.SetPiece(15); // Piece 16

        byte[] bitfieldBytes = bitfield.GetBytes();

       
        Console.WriteLine("Bitfield generated :");
        bitfield.PrintBitfield();

        
        byte[] message = new byte[bitfieldBytes.Length + 1];
        message[0] = (byte)bitfieldBytes.Length;
        Array.Copy(bitfieldBytes, 0, message, 1, bitfieldBytes.Length);

       
        await stream.WriteAsync(message, 0, message.Length);
        Console.WriteLine("Bitfield sent !");

        
        client.Close();
    }

    
    public static async Task SendHandshake(NetworkStream stream, byte[] infoHash, string peerID)
    {
        byte[] handshake = new byte[68];
        handshake[0] = 19;  
        Encoding.ASCII.GetBytes("BitTorrent protocol").CopyTo(handshake, 1);

      
        byte[] reserved = new byte[8];
        Array.Copy(reserved, 0, handshake, 20, 8);

        // InfoHash (20 bytes)
        Array.Copy(infoHash, 0, handshake, 28, 20);

        // PeerID (20 bytes)
        Encoding.ASCII.GetBytes(peerID).CopyTo(handshake, 48);

        
        await stream.WriteAsync(handshake, 0, handshake.Length);
        Console.WriteLine("Handshake sent !");
    }
}


