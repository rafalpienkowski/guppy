using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Guppy.Domain;

namespace Guppy.UI.Views;

public partial class MainWindow : Window
{
    private const string StartFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
    private static readonly SolidColorBrush LightSquare = new(Color.FromRgb(240, 217, 181));
    private static readonly SolidColorBrush DarkSquare = new(Color.FromRgb(181, 136, 99));
    private static readonly SolidColorBrush WhitePiece = new(Colors.White);
    private static readonly SolidColorBrush BlackPiece = new(Color.FromRgb(30, 30, 30));
    
    public MainWindow()
    {
        InitializeComponent();
        Board.LoadFromFen(StartFen);
        // Board.LoadFromFen("r1bk3r/p2pBpNp/n4n2/1p1NP2P/6P1/3P4/P1P1K3/q5b1 b - - 1 23");
        BuildLabels();
        LoadBoard();
    }
    
    private void BuildLabels()
    {
        var brush = new SolidColorBrush(Color.FromRgb(170, 170, 170));
 
        RankLabels.Children.Clear();
        for (var rank = 0; rank < 8; rank++)
        {
            RankLabels.Children.Add(MakeLabel((8 - rank).ToString(), brush));
        }
 
        FileLabels.Children.Clear();
        for (var file = 0; file < 8; file++)
        {
            FileLabels.Children.Add(MakeLabel(((char)('a' + file)).ToString(), brush));
        }
    }
    
    private static TextBlock MakeLabel(string text, IBrush brush) => new()
    {
        Text = text,
        FontSize = 18,
        Foreground = brush,
        HorizontalAlignment = HorizontalAlignment.Center,
        VerticalAlignment = VerticalAlignment.Center
    };

    private void LoadBoard()
    {
        for (var rank = 0; rank < 8; rank++)
        {
            for (var file = 0; file < 8; file++)
            {
                var isLight = (rank + file) % 2 == 0;
                var square = new Border { Background = isLight ? LightSquare : DarkSquare };
                
                var piece = Board.GetByCoordinate(rank, file);
                if (piece != Piece.None)
                {
                    square.Child = new TextBlock
                    {
                        Text = PieceToGlyph(piece),
                        FontSize = 56,
                        Foreground = piece.HasFlag(Piece.White) ? WhitePiece : BlackPiece,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    }; 
                }
                
                BoardGrid.Children.Add(square);
            }
        }
    }
    
    
    private static string PieceToGlyph(Piece piece)
    {
        if (piece.HasFlag(Piece.King))
        {
            return "\u265A";
        }

        if (piece.HasFlag(Piece.Queen))
        {
            return "\u265B";
        }

        if (piece.HasFlag(Piece.Rook))
        {
            return "\u265C";
        }

        if (piece.HasFlag(Piece.Bishop))
        {
            return "\u265D";
        }

        if (piece.HasFlag(Piece.Knight))
        {
            return "\u265E";
        }

        if (piece.HasFlag(Piece.Pawn))
        {
            return "\u265F";
        }

        return string.Empty;
    }
}