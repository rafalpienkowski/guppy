using System.Drawing;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Guppy.Domain;
using Color = Avalonia.Media.Color;

namespace Guppy.UI.Views;

public partial class MainWindow : Window
{
    private const string StartFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
    private static readonly SolidColorBrush LightSquare = new(Color.FromRgb(240, 217, 181));
    private static readonly SolidColorBrush DarkSquare = new(Color.FromRgb(181, 136, 99));
    private static readonly SolidColorBrush WhitePiece = new(Colors.White);
    private static readonly SolidColorBrush BlackPiece = new(Color.FromRgb(30, 30, 30));

    private const double CellSize = 80;
    private const double BoardSize = 640;

    // Drag state.
    private (int rank, int file)? _dragSource;
    private Piece _dragPiece;
    private Border? _dragVisual;

    public MainWindow()
    {
        InitializeComponent();
        Board.LoadFromFen(StartFen);
        // Board.LoadFromFen("r1bk3r/p2pBpNp/n4n2/1p1NP2P/6P1/3P4/P1P1K3/q5b1 b - - 1 23");
        BuildLabels();
        Render();

        BoardGrid.PointerPressed += OnPointerPressed;
        BoardGrid.PointerMoved += OnPointerMoved;
        BoardGrid.PointerReleased += OnPointerReleased;
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

    /* Pieces drag and drop */
    private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (!e.GetCurrentPoint(BoardGrid).Properties.IsLeftButtonPressed)
        {
            return;
        }

        var p = e.GetPosition(BoardGrid);
        if (!TryGetSquare(p, out var rank, out var file))
        {
            return;
        }

        var piece = Board.GetByCoordinate(rank, file);
        if (piece is Piece.None)
        {
            return;
        }

        _dragSource = (rank, file);
        _dragPiece = piece;

        _dragVisual = MakeDragVisual(piece);
        DragLayer.Children.Add(_dragVisual);
        MoveDragVisual(p);

        Render(); // re-render so the source square shows empty
        e.Pointer.Capture(BoardGrid); // capture so moves + release keep reaching us
        e.Handled = true;
    }

    private void OnPointerMoved(object? sender, PointerEventArgs e)
    {
        if (_dragSource is null)
        {
            return;
        }

        MoveDragVisual(e.GetPosition(BoardGrid));
    }

    private void OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (_dragSource is not { } from)
        {
            return;
        }

        var p = e.GetPosition(BoardGrid);
        if (TryGetSquare(p, out var rank, out var file) && (rank, file) != from)
        {
            Board.SetByCoordinate(from.rank, from.file, Piece.None);
            Board.SetByCoordinate(rank, file, _dragPiece);
        }

        if (_dragVisual is not null)
        {
            DragLayer.Children.Remove(_dragVisual);
            _dragVisual = null;
        }

        _dragSource = null;

        Render(); // source returns, or piece lands on the target
        e.Pointer.Capture(null);
        e.Handled = true;
    }

    private void MoveDragVisual(Avalonia.Point boardPoint)
    {
        if (_dragVisual is null)
        {
            return;
        }

        // Centre the dragged piece on the cursor.
        Canvas.SetLeft(_dragVisual, boardPoint.X - CellSize / 2);
        Canvas.SetTop(_dragVisual, boardPoint.Y - CellSize / 2);
    }

    // Pointer position (in the board's own 640x640 space) -> square indices.
    private static bool TryGetSquare(Avalonia.Point boardPoint, out int rank, out int file)
    {
        rank = (int)(boardPoint.Y / CellSize);
        file = (int)(boardPoint.X / CellSize);
        return boardPoint.X is >= 0 and < BoardSize
               && boardPoint.Y is >= 0 and < BoardSize;
    }

    private static Border MakeDragVisual(Piece piece) => new()
    {
        Width = CellSize,
        Height = CellSize,
        IsHitTestVisible = false,
        Child = new TextBlock
        {
            Text = PieceToGlyph(piece),
            FontSize = 56,
            Foreground = piece.HasFlag(Piece.White)
                ? new SolidColorBrush(Colors.White)
                : new SolidColorBrush(Color.FromRgb(30, 30, 30)),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        }
    };

    private void Render()
    {
        BoardGrid.Children.Clear();

        for (var rank = 0; rank < 8; rank++)
        {
            for (var file = 0; file < 8; file++)
            {
                var isLight = (rank + file) % 2 == 0;
                var square = new Border { Background = isLight ? LightSquare : DarkSquare };

                var piece = Board.GetByCoordinate(rank, file);
                var isDragSource = _dragSource is { } s && s.rank == rank && s.file == file;

                if (piece is not Piece.None && !isDragSource)
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