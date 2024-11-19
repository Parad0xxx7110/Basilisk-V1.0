    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;

    namespace Basilisk
    {
        public class MessageHandler
        {
            private readonly NetworkStream _stream;

            public MessageHandler(NetworkStream stream)
            {
                _stream = stream;
            }

            public async Task HandleIncomingMessages()
            {
                byte[] msgBuffer = new byte[4096];
                while (true)
                {
                    try
                    {
                        int bytesRead = await _stream.ReadAsync(msgBuffer, 0, msgBuffer.Length);
                        if (bytesRead == 0)
                            continue; 

                        int messageLength = BitConverter.ToInt32(msgBuffer, 0);
                        byte messageType = msgBuffer[4];

                        if (messageLength > bytesRead)
                        {
                            Console.WriteLine($"Incomplete message received ({messageLength} expected, {bytesRead} received).");
                            continue;
                        }

                    
                        switch (messageType)
                        {
                            case 0: // Choke
                                await HandleChoke();
                                break;
                            case 1: // Unchoke
                                await HandleUnchoke();
                                break;
                            case 2: // Interested
                                await HandleInterested();
                                break;
                            case 3: // Not Interested
                                await HandleNotInterested();
                                break;
                            case 4: // Have
                                await HandleHave(msgBuffer);
                                break;
                            case 5: // Bitfield
                                await HandleBitfield(msgBuffer);
                                break;
                            case 6: // Request
                                await HandleRequest(msgBuffer);
                                break;
                            case 7: // Piece
                                await HandlePiece(msgBuffer);
                                break;
                            default:
                                Console.WriteLine($"Unknown {messageType} received.");
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error handling message : " + ex.Message);
                    }
                }
            }

        
            private async Task HandleChoke()
            {
                Console.WriteLine("Choke message received");
            
                await Task.CompletedTask;
            }

      
            private async Task HandleUnchoke()
            {
                Console.WriteLine("Unchoke message received");
            
                await Task.CompletedTask;
            }

        
            private async Task HandleInterested()
            {
                Console.WriteLine("Interested message received");
            
                await Task.CompletedTask;
            }

            private async Task HandleNotInterested()
            {
                Console.WriteLine("Not interested message received");
            
                await Task.CompletedTask;
            }

        
            private async Task HandleHave(byte[] buffer)
            {
                int pieceIndex = BitConverter.ToInt32(buffer, 5);
                Console.WriteLine($"Have message received for piece {pieceIndex}.");
            
                await Task.CompletedTask;
            }

        
            private async Task HandleBitfield(byte[] buffer)
            {
                Console.WriteLine("Bitfield messgae received");

                await Task.CompletedTask;
            
            }

        
            private async Task HandleRequest(byte[] buffer)
            {
                Console.WriteLine("Request message received");
            
                await Task.CompletedTask;
            }

        
            private async Task HandlePiece(byte[] buffer)
            {
                Console.WriteLine("Piece message received");
            
                await Task.CompletedTask;
            }
        }

    }
