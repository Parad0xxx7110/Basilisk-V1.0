C# Torrent Client
Description
This project is a torrent client written entirely in C# from scratch. While currently functional, the code is in a raw state and requires significant refactoring to improve readability, modularity, and error handling.

Current Features
Handshake compliant with the BitTorrent protocol
The client establishes connections with peers following the standard handshake process.
Bitfield management
The client sends and receives bitfields to indicate owned pieces.
Basic peer communication
A rudimentary test server is used to validate message exchanges between peers.
Planned Features
Message type handling
Add logic to distinguish between different PeerWire message types (request, piece, choke, unchoke, etc.).
Piece downloading and uploading
Implement downloading requests (request) and piece responses (piece).
Tracker communication
Integrate tracker interaction to fetch the peer list.
Piece verification
Add SHA-1 validation for downloaded blocks.
Optimization and error handling
Handle disconnections, timeouts, and non-compliant peers gracefully.
Dependencies
No external dependencies required. The entire implementation is written in pure C#.

Installation
Clone this repository:
git clone <repository_url>  
cd <repository_folder>  

Build the project using your favorite C# IDE (Visual Studio, Rider, etc.) or with dotnet:
dotnet build  
