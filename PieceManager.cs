using System;
using System.Collections.Generic;

public class PieceManager
{
    private readonly int totalPieces;
    private readonly bool[] piecesOwned;
    private readonly HashSet<int> piecesDownloading;

    
    public PieceManager(int totalPieces)
    {
        this.totalPieces = totalPieces;
        this.piecesOwned = new bool[totalPieces];
        this.piecesDownloading = new HashSet<int>();
    }

    public List<int> GetPiecesToDownload(byte[] bitfield)
    {
        var piecesToDownload = new List<int>();

        for (int i = 0; i < totalPieces; i++)
        {
            if (((bitfield[i / 8] >> (7 - i % 8)) & 1) == 1 && !piecesOwned[i])
            {
                piecesToDownload.Add(i);
            }
        }

        return piecesToDownload;
    }

   
    public void MarkPieceAsDownloaded(int pieceIndex)
    {
        
        if (pieceIndex < 0 || pieceIndex >= totalPieces)
        {
            Console.WriteLine($"Error index {pieceIndex} OOB");
            return;
        }

        if (!piecesOwned[pieceIndex])
        {
            piecesOwned[pieceIndex] = true;
            piecesDownloading.Remove(pieceIndex);
            Console.WriteLine($"Piece {pieceIndex} downloaded & added.");
        }
    }

    
    public void AddPieceToDownloading(int pieceIndex)
    {
       
        if (pieceIndex < 0 || pieceIndex >= totalPieces)
        {
            Console.WriteLine($"Error {pieceIndex} OOB");
            return;
        }

        if (!piecesOwned[pieceIndex])
        {
            piecesDownloading.Add(pieceIndex);
        }
    }

    
    public bool IsPieceDownloading(int pieceIndex)
    {
        if (pieceIndex < 0 || pieceIndex >= totalPieces)
        {
            Console.WriteLine($"Error  {pieceIndex} OOB");
            return false;
        }

        return piecesDownloading.Contains(pieceIndex);
    }

    
    public IReadOnlyCollection<int> PiecesDownloading => piecesDownloading;
}

