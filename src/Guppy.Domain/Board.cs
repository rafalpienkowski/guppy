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

public class Board {
    public static Piece[] Square;

    public Board()
    {
        Square = new Piece[64];

        Square[0] = Piece.White | Piece.Rook;
    }
}

