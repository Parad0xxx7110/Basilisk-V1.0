using Basilisk;
using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;

public enum PeerWireMessageType : byte
{
    Choke = 0,
    Unchoke = 1,
    Interested = 2,
    NotInterested = 3,
    Have = 4,
    Bitfield = 5,
    Request = 6,
    Piece = 7,
    Cancel = 8,
    Port = 9
}

public class PeerWireProtocol
{
    private static readonly string ProtocolID = "BitTorrent protocol";
    private const int HandshakeResponseLength = 68; // Handshake is always 68 bytes

    

    private async Task SendMessage(NetworkStream stream, PeerWireMessageType messageType, byte[] data)
    {
        int messageLength = 1 + data.Length;
        byte[] message = new byte[4 + messageLength];

        BitConverter.GetBytes(messageLength).CopyTo(message, 0);
        message[4] = (byte)messageType;
        Array.Copy(data, 0, message, 5, data.Length);

        await stream.WriteAsync(message, 0, message.Length);
        Console.WriteLine($"{messageType} message sent!");
    }

    // Méthode de handshake
    public byte[] Handshake(byte[] hash, string peerID)
    {
        if (peerID.Length != 20)
        {
            throw new ArgumentException("PeerID must be 20 bytes");
        }

        byte[] protocolBytes = Encoding.UTF8.GetBytes(ProtocolID);
        byte[] reserved = new byte[8];
        byte[] peerIDBytes = Encoding.UTF8.GetBytes(peerID);
        byte protocolLength = (byte)protocolBytes.Length;

        using (MemoryStream memStream = new MemoryStream())
        {
            memStream.WriteByte(protocolLength);
            memStream.Write(protocolBytes, 0, protocolLength);
            memStream.Write(reserved, 0, reserved.Length);
            memStream.Write(hash, 0, 20);
            memStream.Write(peerIDBytes, 0, 20);
            return memStream.ToArray();
        }
    }

    // Méthode pour envoyer le handshake
    public async Task SendHandshake(NetworkStream stream, byte[] hash, string peerID)
    {
        try
        {
            // Génère le message de handshake
            byte[] handshakeMessage = Handshake(hash, peerID);

            // Envoie le message de handshake
            await stream.WriteAsync(handshakeMessage, 0, handshakeMessage.Length);
            Console.WriteLine("Handshake sent!");

            // Réception de la réponse du handshake
            byte[] handshakeResponse = new byte[HandshakeResponseLength];
            int bytesRead = await stream.ReadAsync(handshakeResponse, 0, handshakeResponse.Length);
            Console.WriteLine($"Received {bytesRead} bytes.");

            // Vérification de la validité de la réponse
            if (bytesRead == HandshakeResponseLength)
            {
                Console.WriteLine("Valid handshake!");
            }
            else
            {
                Console.WriteLine("Error: Invalid handshake response!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending handshake: {ex.Message}");
        }
    }


    
    public async Task SendChokeMessage(NetworkStream stream)
    {
        await SendMessage(stream, PeerWireMessageType.Choke, new byte[0]);
    }

    
    public async Task SendUnchokeMessage(NetworkStream stream)
    {
        await SendMessage(stream, PeerWireMessageType.Unchoke, new byte[0]);
    }

    
    public async Task SendRequestMessage(NetworkStream stream, int pieceIndex, int offset, int length)
    {
        byte[] requestData = new byte[13];
        BitConverter.GetBytes(pieceIndex).CopyTo(requestData, 0);
        BitConverter.GetBytes(offset).CopyTo(requestData, 4);
        BitConverter.GetBytes(length).CopyTo(requestData, 8);

        await SendMessage(stream, PeerWireMessageType.Request, requestData);
    }

    
    public async Task SendPieceMessage(NetworkStream stream, int pieceIndex, int offset, byte[] data)
    {
        byte[] pieceData = new byte[data.Length + 9];
        BitConverter.GetBytes(pieceIndex).CopyTo(pieceData, 0);
        BitConverter.GetBytes(offset).CopyTo(pieceData, 4);
        Array.Copy(data, 0, pieceData, 8, data.Length);

        await SendMessage(stream, PeerWireMessageType.Piece, pieceData);
    }

   
    public async Task SendHaveMessage(NetworkStream stream, int pieceIndex)
    {
        byte[] haveData = new byte[4];
        BitConverter.GetBytes(pieceIndex).CopyTo(haveData, 0);

        await SendMessage(stream, PeerWireMessageType.Have, haveData);
    }

   
    public async Task SendCancelMessage(NetworkStream stream, int pieceIndex, int offset, int length)
    {
        byte[] cancelData = new byte[13];
        BitConverter.GetBytes(pieceIndex).CopyTo(cancelData, 0);
        BitConverter.GetBytes(offset).CopyTo(cancelData, 4);
        BitConverter.GetBytes(length).CopyTo(cancelData, 8);

        await SendMessage(stream, PeerWireMessageType.Cancel, cancelData);
    }

    /* 
    public async Task SendPortMessage(NetworkStream stream, int listenPort)
    {
        byte[] portData = new byte[2];
        BitConverter.GetBytes(listenPort).CopyTo(portData, 0);

        await SendMessage(stream, PeerWireMessageType.Port, portData);
    }
    */

    
    
}
