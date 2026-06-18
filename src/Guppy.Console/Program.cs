using Guppy.Domain;

Console.WriteLine("Hello, World!");
Console.WriteLine();


var board = new Board();
//Board.LoadFromFen("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
Board.LoadFromFen("r1bk3r/p2pBpNp/n4n2/1p1NP2P/6P1/3P4/P1P1K3/q5b1 b - - 1 23");

Board.Print();


