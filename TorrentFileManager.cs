using BencodeNET.Parsing;
using BencodeNET.Torrents;
using System;
using System.Collections.Generic;
using System.Linq;

public class TorrentManager
{
    public required string TorrentName { get; set; }
    public required string TorrentInfoHash { get; set; }
    public required List<string> Trackers { get; set; }
    public required int NumberOfPieces { get; set; }

    public long PieceSizeKo
    {
        get
        {
            return PieceSize > 0 ? PieceSize / 1024 : 0;
        }
        set
        {
            PieceSize = value * 1024;
        }
    }

    private long PieceSize { get; set; }

    public TorrentManager(string torrentFilePath)
    {
        Trackers = new List<string>();
        LoadTorrentData(torrentFilePath);
    }

    private void LoadTorrentData(string torrentFilePath)
    {
        try
        {
            var parser = new BencodeParser();
            Torrent torrent = parser.Parse<Torrent>(torrentFilePath);

            TorrentName = torrent.DisplayName;
            TorrentInfoHash = torrent.GetInfoHash();
            Trackers = FlattenTrackers(torrent.Trackers);
            NumberOfPieces = torrent.NumberOfPieces;
            PieceSize = torrent.PieceSize;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error twhile reading Bencoded data : {ex.Message}");
        }
    }

    private List<string> FlattenTrackers(IList<IList<string>> trackers)
    {
        return trackers?.SelectMany(sublist => sublist ?? Enumerable.Empty<string>())
                        .ToList() ?? new List<string>();
    }

    public void DisplayTorrentInfo()
    {
        Console.WriteLine($"Torrent name -> {TorrentName}");

        foreach (var tracker in Trackers)
        {
            Console.WriteLine($"Tracker -> {tracker}");
        }

        Console.WriteLine($"Pieces number -> {NumberOfPieces}");
        Console.WriteLine($"Piece size (Ko) -> {PieceSizeKo} Ko");
    }
}
