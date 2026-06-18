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
    private const Piece Piece = default;
    private static Piece[]? square;

    public Board()
    {
        square = new Piece[64];
        square[0] = Piece.White | Piece.Rook;
    }

    /*
    public void LoadFen(string fen)
    {
    }
    */

    public void Print()
    {
        Console.OutputEncoding = Encoding.UTF8;
        // required for the glyphs (esp. on Windows)
        //

        for (int rank = 7; rank >= 0; rank--)
        {
            Console.Write(" ");
            for (int file = 0; file < 8; file++)
            {
                Console.BackgroundColor = (rank + file) % 2 == 1
                    ? ConsoleColor.DarkGray
                    : ConsoleColor.Gray;
                Console.ForegroundColor = (rank + file) % 2 == 0
                    ? ConsoleColor.White
                    : ConsoleColor.Black;
                //Console.Write($" {Glyph(square[(rank * 8) + file])} ");  // ♚♛♜♝♞♟ for both sides
                //Console.Write(" ");
                Console.Write($"{(rank * 8 + file):00}");
            }

            Console.ResetColor();
            Console.WriteLine();
        }
    }

    private static string Glyph(Piece piece)
    {
        return piece switch
        {
            Piece.King => "♚",
            Piece.Queen => "♛",
            Piece.Rook => "♜",
            Piece.Bishop => "♝",
            Piece.Knight => "♞",
            Piece.Pawn => "♟",
            Piece.None => " ",
            _ => ""
        };
    }
}

