using System.Text;

namespace Guppy.Domain;

[Flags]
public enum Piece
{
    None = 0,
    Pawn = 1 << 0,      // 0b_00000001 // 1
    Rook = 1 << 1,      // 0b_00000010 // 2
    Knight = 1 << 2,    // 0b_00000100 // 4
    Bishop = 1 << 3,    // 0b_00001000 // 8
    Queen = 1 << 4,     // 0b_00010000 // 16
    King = 1 << 5,      // 0b_00100000 // 32

    White = 1 << 6,     // 0b_01000000 // 64
    Black = 1 << 7      //  0b_10000000 // 128

    // Why white and black with those values
    // 0 0 | 0 0 0 0 0 0
    // color = none | type = none
    // 0 1 | 0 0 0 1 0 0
    // color = white | type = knight
}

public class Board
{
    private static readonly Piece[] Square = new Piece[64];
    private static readonly Dictionary<char, Piece> Symbols = new()
    {
            { 'k', Piece.King | Piece.Black },
            { 'K', Piece.King | Piece.White },
            { 'p', Piece.Pawn | Piece.Black },
            { 'P', Piece.Pawn | Piece.White },
            { 'b', Piece.Bishop | Piece.Black },
            { 'B', Piece.Bishop | Piece.White },
            { 'r', Piece.Rook | Piece.Black },
            { 'R', Piece.Rook | Piece.White },
            { 'q', Piece.Queen | Piece.Black },
            { 'Q', Piece.Queen | Piece.White },
            { 'n', Piece.Knight | Piece.Black },
            { 'N', Piece.Knight | Piece.White }
        };

    public static void LoadFromFen(string fen)
    {
        var fenBoard = fen.Split(' ')[0];
        var file = 0;
        var rank = 7;
        foreach (var symbol in fenBoard)
        {
            if (symbol == '/')
            {
                file = 0;
                rank--;
            }
            else
            {
                if (char.IsDigit(symbol))
                {
                    file += (int)char.GetNumericValue(symbol);
                }
                else
                {
                    Square[(rank * 8) + file] = Symbols[symbol];
                    file++;
                }
            }
        }
    }

    /// <summary>
    /// For UI only
    /// </summary>
    public static Piece GetByCoordinate(int rank, int file) =>
        Square[(rank * 8) + file];

    public static void SetByCoordinate(int rank, int file, Piece piece) =>
        Square[(rank * 8) + file] = piece;
}

