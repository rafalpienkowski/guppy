# Building a Chess Engine — A Reading Roadmap

A curated, layered reading list to prepare yourself *before* writing engine code.

**How to use this:** The groups are ordered the way the engine itself comes together — big picture → board representation → move generation → testing → search → evaluation → talking to a GUI. Read Group 0 first to get the whole thing in your head, keep Group 1 open as a permanent reference, then work down. You don't need to finish a group before moving on; skim, build a mental model, and come back when you implement that part.

Resources marked **[C#]** are worth prioritizing since that's your language.

---

## 0. Get the whole picture first (watch/read before anything else)

*Goal: understand what an engine is and how the pieces fit, before drowning in detail.*

- **Coding Adventure: Chess — Sebastian Lague** (video, ~30 min) — The best "what is a chess engine" intro there is. Built in C#, beautifully visualized. https://www.youtube.com/watch?v=U4ogK0MIzqk **[C#]**
- **Coding Adventure: Making a Stronger Chess Engine — Sebastian Lague** (video, ~1 h) — The sequel: bitboards, magic bitboards, transposition tables, search extensions, then the bot deployed on Lichess against humans and Stockfish. https://www.youtube.com/watch?v=_vqlIPDR2TU **[C#]**
- **Rustic Chess Engine — the book** — A free, well-written online book that walks through building an engine end to end. The language is Rust, but the *explanations* of board representation, FEN, move generation and search are some of the clearest anywhere. https://rustic-chess.org/

## 1. Your permanent reference (bookmark this)

*You'll return here constantly — use it as a dictionary, don't read cover to cover.*

- **Chess Programming Wiki — main page** — The canonical reference for every topic in computer chess. Navigate via the left sidebar or the search box at the top. https://www.chessprogramming.org/Main_Page
- Key pages to know exist (all reachable from the wiki): Board Representation, Bitboards, FEN, Move Generation, Perft, Minimax/Negamax, Alpha-Beta, Quiescence Search, Iterative Deepening, Transposition Table, Evaluation, Piece-Square Tables, UCI.

## 2. Board representation & move generation (the foundation)

*Goal: represent a position and generate every legal move correctly. This is the hardest part to get exactly right.*

- **Bitboards — Chess Programming Wiki** — Packing the 64 squares into a 64-bit integer and manipulating pieces with bitwise operations. https://www.chessprogramming.org/Bitboards
- **Bitboards — Rustic book** — A gentle, worked version of the same idea (occupancy, combining boards, FEN parsing). https://rustic-chess.org/board_representation/bitboards.html
- **Writing a chess engine in C++ — Maarten Ameye** — A clear from-scratch write-up covering leaping vs. sliding pieces, pseudo-legal moves, en passant, castling, and check evasion. https://ameye.dev/notes/chess-engine/
- **Magic Bitboards — Chess Programming Wiki** — The fast technique for sliding-piece (rook/bishop) attacks. https://www.chessprogramming.org/Magic_Bitboards

> Tip: get a *simple, correct* move generator working before magic bitboards. Correctness first, speed later.

## 3. Test your move generator with perft (do this EARLY)

*Goal: prove move generation is bug-free before building anything on top of it. This step saves weeks.*

- **Perft — Chess Programming Wiki** — What perft is, plus the standard test positions and their exact node counts at each depth. https://www.chessprogramming.org/Perft
- **perftree — perft debugger** — Runs your engine's perft side by side with Stockfish and pinpoints exactly where move generation diverges. https://github.com/agausmann/perftree
- **Debugging Move Generation — Chess Engine Lab** — A practical account of hunting real movegen bugs with perft + Stockfish (castling rights, en passant edge cases). https://chessenginelab.substack.com/p/debugging-move-generation-and-optimization

## 4. Search (making it actually play)

*Goal: look ahead through the game tree and pick the best move.*

- **Minimax → Negamax — Chess Programming Wiki** — Start with minimax, then negamax (the compact zero-sum form everyone uses). Note the requirement that evaluation is from the side-to-move's perspective. https://www.chessprogramming.org/Negamax
- **Alpha-Beta — Chess Programming Wiki** — Pruning that cuts the tree dramatically without changing the result. https://www.chessprogramming.org/Alpha-Beta
- **α-β pruning and better search — dogeystamp** (blog, part of a build series) — One of the best single explainers of the *practical* search stack: alpha-beta, transposition tables, move ordering, iterative deepening, and quiescence — and why they reinforce each other. https://www.dogeystamp.com/chess4/

## 5. Evaluation (making it play *well*)

*Goal: score a position when you stop searching.*

- **Evaluation — Chess Programming Wiki** — Overview of what goes into a static evaluation. https://www.chessprogramming.org/Evaluation
- **Piece-Square Tables — Chess Programming Wiki** — The simplest big upgrade over counting material: positional bonuses per square. Many beginner engines get surprisingly strong on material + PSTs alone. https://www.chessprogramming.org/Piece-Square_Tables

## 6. Talking to a GUI — the UCI protocol

*Goal: let your engine plug into standard GUIs and play other engines. UCI is plain text over stdin/stdout.*

- **UCI specification — Shredder** (the original spec text) — The authoritative protocol description by its author. https://www.shredderchess.com/chess-features/uci-universal-chess-interface.html
- **Modern UCI docs** — A friendlier, well-organized re-presentation with the session flow explained. https://publish.obsidian.md/modern-uci-doc/UCI+Docs/Intro
- **UCI — Chess Programming Wiki** — Context, history, and how responsibilities split between engine and GUI. https://www.chessprogramming.org/UCI
- **Writing a UCI client — andreinc** — The command exchange step by step (uci → id → option → uciok, position, go depth …). Java, but protocol-focused and language-agnostic. https://www.andreinc.net/2021/04/22/writing-a-universal-chess-interface-client-in-java

## 7. GUIs & tools to run and test your engine

- **Cute Chess** — Free, scriptable GUI and command-line tool for running engine-vs-engine matches and tournaments. https://github.com/cutechess/cutechess
- **Arena** — Popular free Windows UCI GUI for playing against your engine and watching games. (Search "Arena Chess GUI" for the official download.)

## 8. Reference engines to read (learn from clean code) — C# first

*Reading a small, well-written engine teaches more than any article. Start with the most minimal.*

- **MinimalChess — Thomas Jahn (lithander)** — A didactic UCI engine in only a few hundred lines of idiomatic C#, written specifically as a starting point for C# programmers new to chess programming. Versions 0.2 → 0.6 each add one concept (move ordering, quiescence, PSTs, transposition table, null-move, LMR), with a companion YouTube video per milestone. **This is your ideal model.** https://github.com/lithander/MinimalChessEngine **[C#]**
- **Leorik — Thomas Jahn (lithander)** — MinimalChess's stronger successor (bitboards, NNUE). Read after MinimalChess to see how a serious C# engine is structured. https://github.com/lithander/Leorik **[C#]**
- **Sebastian Lague's engine + UCI port** — The engine from the videos above, with UCI added so you can actually run it. https://github.com/lithander/SebLagueChessEngine **[C#]**
- **Sunfish — Thomas Ahle** — A famously tiny, very readable engine in ~100 lines of Python. Great for grasping search + evaluation as pure ideas, stripped of optimization. https://github.com/thomasahle/sunfish

## 9. Books (optional, for a deeper single source)

- **How to Write a Bitboard Chess Engine — Bill Jordan** — A beginner-friendly book that builds a complete bitboard engine with fully annotated source. C++, but concept-transferable. https://www.amazon.com/How-Write-Bitboard-Chess-Engine/dp/B08BDZ2K8B

## 10. Where to ask when you're stuck

- **TalkChess (Computer Chess Club forum)** — The main gathering place for engine authors; very knowledgeable and beginner-tolerant if you show your work. https://talkchess.com/
- **Chess Programming Wiki Discord / forums** — Linked from the wiki's main page; good for quick questions.

---

### A suggested path through all of this
1. Watch both Sebastian Lague videos (Group 0).
2. Skim the Rustic book's structure to see the whole arc.
3. Implement board + move generation (Group 2), then **immediately** validate with perft (Group 3) until your counts match the reference exactly.
4. Add search (Group 4): negamax → alpha-beta → iterative deepening + move ordering + quiescence.
5. Add evaluation (Group 5): material → piece-square tables.
6. Wire up UCI (Group 6) and play it in a GUI (Group 7).
7. Keep MinimalChess open the entire time as a reference (Group 8).
