using Guppy.Domain;
using System.Text;
using Terminal.Gui.App;
using Terminal.Gui.Drawing;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;
using Color = Terminal.Gui.Drawing.Color;

using var app = Application.Create();
app.Init();

// Board.LoadFromFen("r1bk3r/p2pBpNp/n4n2/1p1NP2P/6P1/3P4/P1P1K3/q5b1 b - - 1 23");

using Window window = new()
{
    Title = "Guppy — chessboard (Esc to quit)"
};

ChessBoardView board = new()
{
    X = 0,
    Y = 0,
    Width = 36,
    Height = 18
};

window.Add(board);

app.Run(window);


public sealed class ChessBoardView : View
{
    public ChessBoardView()
    {
        Board.LoadFromFen("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
    }

    protected override bool OnDrawingContent(DrawContext? context)
    {
        var vp = Viewport;
        if (vp.Width < 12 || vp.Height < 6)
        {
            return true; // too small to draw a sensible board
        }

        const int labelW = 2; // columns reserved for rank digits
        const int labelH = 1; // rows reserved for file letters

        var cellW = Math.Max(2, (vp.Width - labelW) / 8);
        var cellH = Math.Max(1, (vp.Height - labelH) / 8);
        var boardW = cellW * 8;
        var boardH = cellH * 8;

        var originX = labelW + Math.Max(0, (vp.Width - labelW - boardW) / 2);
        var originY = Math.Max(0, (vp.Height - labelH - boardH) / 2);

        var lightSq = new Color(240, 217, 181);
        var darkSq = new Color(181, 136, 99);
        var whitePiece = new Color(250, 250, 250);
        var blackPiece = new Color(20, 20, 20);

        for (var rank = 0; rank < 8; rank++)
        {
            for (var file = 0; file < 8; file++)
            {
                var isLight = (rank + file) % 2 == 0; // a8 (top-left) is light
                var bg = isLight ? lightSq : darkSq;

                var piece = Board.GetByCoordinate(rank, file);
                var empty = piece is Piece.None;
                var fg = empty ? bg
                    : piece.HasFlag(Piece.White) ? whitePiece
                    : blackPiece;
                var cellX = originX + file * cellW;
                var cellY = originY + rank * cellH;

                SetAttribute(new Terminal.Gui.Drawing.Attribute(fg, bg));
                for (var dy = 0; dy < cellH; dy++)
                {
                    Move(cellX, cellY + dy);
                    AddStr(new string(' ', cellW));
                }

                if (!empty)
                {
                    Move(cellX + cellW / 2, cellY + cellH / 2);
                    AddRune(PieceToGlyph(piece));
                }
            }
        }

        SetAttributeForRole(VisualRole.Normal);
        SetRanksOnBoard(originX, originY, cellW, cellH);
        SetFilesOnBoard(originX, originY, cellW, boardH);

        return true;
    }

    /// <summary>
    /// Sets ranks 8..1 down the left.
    /// </summary>
    private void SetRanksOnBoard(int originX = 0, int originY = 0, int cellW = 2, int cellH = 1)
    {
        for (var rank = 0; rank < 8; rank++)
        {
            Move(originX - 2, originY + rank * cellH + cellH / 2);
            AddStr((8 - rank).ToString());
        }
    }

    /// <summary>
    /// Set ranks a..h across the bottom.
    /// </summary>
    private void SetFilesOnBoard(int originX = 0, int originY = 0, int cellW = 2, int boardH = 1)
    {
        for (var file = 0; file < 8; file++)
        {
            Move(originX + file * cellW + cellW / 2, originY + boardH);
            AddRune(new Rune((char)('a' + file)));
        }
    }

    private static Rune PieceToGlyph(Piece piece)
    {
        if (piece.HasFlag(Piece.King))
        {
            return new Rune('\u265A');
        }
        if (piece.HasFlag(Piece.Queen))
        {
            return new Rune('\u265B');
        }
        if (piece.HasFlag(Piece.Rook))
        {
            return new Rune('\u265C');
        }
        if (piece.HasFlag(Piece.Bishop))
        {
            return new Rune('\u265D');
        }
        if (piece.HasFlag(Piece.Knight))
        {
            return new Rune('\u265E');
        }
        if (piece.HasFlag(Piece.Pawn))
        {
            return new Rune('\u265F');
        }
        return new Rune(' ');
    }
    
    
}