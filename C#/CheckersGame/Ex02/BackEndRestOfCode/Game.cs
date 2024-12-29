using Ex02.ConsoleUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex02
{
    public class Game
    {
        public static void RunGame(Board i_GameBoard, eCheckerType i_CurrentPlayer)
        {
            bool gameRunning = true;
            string lastMove = "";
            int playerOneScore = 0;
            int playerTwoScore = 0;

            while (gameRunning)
            {
                Screen.Clear();
                i_GameBoard.BuildBoard();

                if (!string.IsNullOrEmpty(lastMove))
                {
                    Console.WriteLine(lastMove);
                }

                string currentPlayerName =
                    (i_CurrentPlayer == eCheckerType.whiteChecker || i_CurrentPlayer == eCheckerType.kingWhiteChecker)
                    ? UI.PlayerOneName
                    : UI.PlayerTwoName;
                string playerSymbol =
                    (i_CurrentPlayer == eCheckerType.whiteChecker || i_CurrentPlayer == eCheckerType.kingWhiteChecker)
                    ? "O"
                    : "X";
                bool continueTurn = true;
                List<Move> mandatoryCaptures = GetAllMandatoryCaptures(i_CurrentPlayer, i_GameBoard);

                while (continueTurn)
                {
                    Console.WriteLine($"{currentPlayerName}'s Turn ({playerSymbol}): ");
                    string moveInput = Console.ReadLine().Trim();

                    if (moveInput.ToUpper() == "Q")
                    {
                        Console.WriteLine($"{currentPlayerName} has quit the game. {playerOneScore} - {playerTwoScore} is the score.");
                        Console.WriteLine("Would you like to play another round? (y/n): ");

                        string answer = Console.ReadLine().ToLower();
                        answer = ShouldContinueGame(answer);

                        if (answer != "y")
                        {
                            gameRunning = false;
                            break;
                        }
                    }

                    if (!IsValidMoveFormat(moveInput))
                    {
                        Console.WriteLine("Invalid move format. Please use format like 'Fb>Fa'.");
                        continue;
                    }
                    Move move = new Move(moveInput);

                    if (mandatoryCaptures.Count > 0)
                    {
                        if (!mandatoryCaptures.Any(m => m.m_FromRow == move.m_FromRow && m.m_FromCol == move.m_FromCol &&
                                                      m.m_ToRow == move.m_ToRow && m.m_ToCol == move.m_ToCol))
                        {
                            Console.WriteLine("Capture move is mandatory. Please make a capture.");
                            continue;
                        }
                    }
                    else if (!i_GameBoard.IsValidMove(move, i_CurrentPlayer))
                    {
                        Console.WriteLine("Invalid move. Please try again.");
                        continue;
                    }

                    i_GameBoard.MakeMove(move);
                    lastMove = $"{currentPlayerName}'s move was ({playerSymbol}): {moveInput}";

                    if (i_CurrentPlayer == eCheckerType.whiteChecker && move.m_ToRow == 0)
                    {
                        i_GameBoard.MakeKing(move.m_ToRow, move.m_ToCol);
                    }
                    else if (i_CurrentPlayer == eCheckerType.blackChecker && move.m_ToRow == UI.BoardSize - 1)
                    {
                        i_GameBoard.MakeKing(move.m_ToRow, move.m_ToCol);
                    }
                    bool hasCaptured = i_GameBoard.HasCapturedPiece(move);

                    if (hasCaptured)
                    {
                        List<Move> additionalCaptures = GetValidCaptureMoves(move.m_ToRow, move.m_ToCol, i_CurrentPlayer, i_GameBoard);
                        if (additionalCaptures.Count > 0)
                        {
                            mandatoryCaptures = additionalCaptures;
                            Screen.Clear();
                            i_GameBoard.BuildBoard();
                            Console.WriteLine("Additional capture is mandatory. You must take it.");
                            continue;
                        }
                    }

                    continueTurn = false;
                }
                playerOneScore = CalculateScore(eCheckerType.whiteChecker, i_GameBoard);
                playerTwoScore = CalculateScore(eCheckerType.blackChecker, i_GameBoard);

                if (CheckForWin(i_GameBoard))
                {
                    Console.WriteLine($"{currentPlayerName} wins! {playerOneScore} - {playerTwoScore} is the score.");
                    Console.WriteLine("Would you like to play another round? (y/n): ");

                    string answer = Console.ReadLine().ToLower();
                    answer = ShouldContinueGame(answer);

                    if (answer != "y")
                    {
                        gameRunning = false;
                        break;
                    }
                }
                else if (IsDraw(i_GameBoard))
                {
                    Console.WriteLine($"It's a draw! {playerOneScore} - {playerTwoScore} is the score.");
                    Console.WriteLine("Would you like to play another round? (y/n): ");
                    string answer = Console.ReadLine().ToLower();
                    answer = ShouldContinueGame(answer);

                    if (answer != "y")
                    {
                        gameRunning = false;
                        break;
                    }
                }

                if (i_CurrentPlayer == eCheckerType.whiteChecker)
                {
                    i_CurrentPlayer = eCheckerType.blackChecker;
                }
                else
                {
                    i_CurrentPlayer = eCheckerType.whiteChecker;
                }
            }
        }

        public static void RunGameAgainstComputer(Board i_GameBoard, eCheckerType i_CurrentPlayer)
        {
            bool gameRunning = true;
            string lastMove = "";
            int playerOneScore = 0;
            int computerScore = 0;

            while (gameRunning)
            {
                Screen.Clear();
                i_GameBoard.BuildBoard();

                if (!string.IsNullOrEmpty(lastMove))
                {
                    Console.WriteLine(lastMove);
                }

                string currentPlayerName = (i_CurrentPlayer == eCheckerType.whiteChecker) ? UI.PlayerOneName : "Computer";
                string playerSymbol = (i_CurrentPlayer == eCheckerType.whiteChecker) ? "O" : "X";
                bool continueTurn = true;
                List<Move> mandatoryCaptures = GetAllMandatoryCaptures(i_CurrentPlayer, i_GameBoard);

                while (continueTurn)
                {
                    if (i_CurrentPlayer == eCheckerType.whiteChecker)
                    {
                        Console.WriteLine($"{currentPlayerName}'s Turn ({playerSymbol}): ");
                        string moveInput = Console.ReadLine().Trim();

                        if (moveInput.ToUpper() == "Q")
                        {
                            Console.WriteLine($"{currentPlayerName} has quit the game. {playerOneScore} - {computerScore} is the score.");
                            Console.WriteLine("Would you like to play another round? (y/n): ");

                            string answer = Console.ReadLine().ToLower();
                            answer = ShouldContinueGame(answer);

                            if (answer != "y")
                            {
                                gameRunning = false;
                                break;
                            }
                            gameRunning = false;
                            break;
                        }

                        if (!IsValidMoveFormat(moveInput))
                        {
                            Console.WriteLine("Invalid move format. Please use format like 'Fb>Fa'.");
                            continue;
                        }

                        Move move = new Move(moveInput);

                        if (mandatoryCaptures.Count > 0)
                        {
                            if (!mandatoryCaptures.Any(m => m.m_FromRow == move.m_FromRow && m.m_FromCol == move.m_FromCol &&
                                                          m.m_ToRow == move.m_ToRow && m.m_ToCol == move.m_ToCol))
                            {
                                Console.WriteLine("Capture move is mandatory. Please make a capture.");
                                continue;
                            }
                        }
                        else if (!i_GameBoard.IsValidMove(move, i_CurrentPlayer))
                        {
                            Console.WriteLine("Invalid move. Please try again.");
                            continue;
                        }

                        i_GameBoard.MakeMove(move);
                        lastMove = $"{currentPlayerName}'s move was ({playerSymbol}): {moveInput}";

                        if (i_CurrentPlayer == eCheckerType.whiteChecker && move.m_ToRow == 0)
                        {
                            i_GameBoard.MakeKing(move.m_ToRow, move.m_ToCol);
                        }
                        else if (i_CurrentPlayer == eCheckerType.blackChecker && move.m_ToRow == UI.BoardSize - 1)
                        {
                            i_GameBoard.MakeKing(move.m_ToRow, move.m_ToCol);
                        }
                        bool hasCaptured = i_GameBoard.HasCapturedPiece(move);

                        if (hasCaptured)
                        {
                            List<Move> additionalCaptures = GetValidCaptureMoves(move.m_ToRow, move.m_ToCol, i_CurrentPlayer, i_GameBoard);
                            if (additionalCaptures.Count > 0)
                            {
                                mandatoryCaptures = additionalCaptures;
                                Screen.Clear();
                                i_GameBoard.BuildBoard();
                                Console.WriteLine("Additional capture is mandatory. You must take it.");
                                continue;
                            }
                        }
                        continueTurn = false;
                        i_CurrentPlayer = eCheckerType.blackChecker;  
                    }
                    else 
                    {
                        Console.WriteLine("Computer's Turn (press 'Enter' to see its move): ");
                        Console.ReadLine();

                        Move computerMove;
                        if (mandatoryCaptures.Count > 0)
                        {
                            Random rand = new Random();
                            computerMove = mandatoryCaptures[rand.Next(mandatoryCaptures.Count)];
                        }
                        else
                        {
                            List<Move> validMoves = GetValidMovesForComputer(i_GameBoard);
                            if (validMoves.Count == 0)
                            {
                                Console.WriteLine("No valid moves left for the computer.");
                                gameRunning = false;
                                break;
                            }
                            Random rand = new Random();
                            computerMove = validMoves[rand.Next(validMoves.Count)];
                        }

                        i_GameBoard.MakeMove(computerMove);
                        string moveStr = ConvertMoveToString(computerMove);
                        lastMove = $"Computer's move was: {moveStr}";

                        if (computerMove.m_ToRow == UI.BoardSize - 1)
                        {
                            i_GameBoard.MakeKing(computerMove.m_ToRow, computerMove.m_ToCol);
                        }

                        bool hasCaptured = i_GameBoard.HasCapturedPiece(computerMove);
                        if (hasCaptured)
                        {
                            List<Move> additionalCaptures = GetValidCaptureMoves(computerMove.m_ToRow, computerMove.m_ToCol, i_CurrentPlayer, i_GameBoard);
                            if (additionalCaptures.Count > 0)
                            {
                                mandatoryCaptures = additionalCaptures;
                                Screen.Clear();
                                i_GameBoard.BuildBoard();
                                Console.WriteLine("Computer has additional capture. Press 'Enter' to continue.");
                                Console.ReadLine();
                                continue;
                            }
                        }
                        continueTurn = false;
                        i_CurrentPlayer = eCheckerType.whiteChecker;  
                    }
                }

                playerOneScore = CalculateScore(eCheckerType.whiteChecker, i_GameBoard);
                computerScore = CalculateScore(eCheckerType.blackChecker, i_GameBoard);

                if (CheckForWin(i_GameBoard))
                {
                    Console.WriteLine($"Game Over: {(i_CurrentPlayer == eCheckerType.whiteChecker ? UI.PlayerOneName : "Computer")} wins! {playerOneScore} - {computerScore}");
                    Console.WriteLine("Would you like to play another round? (y/n): ");
                    string answer = Console.ReadLine().ToLower();
                    if (answer == "y")
                    {
                        i_GameBoard.InitializeBoard(); 
                        continue; 
                    }
                    else
                    {
                        gameRunning = false;
                        break;
                    }
                }
                else if (IsDraw(i_GameBoard))
                {
                    Console.WriteLine($"It's a draw! {playerOneScore} - {computerScore}");
                    Console.WriteLine("Would you like to play another round? (y/n): ");
                    string answer = Console.ReadLine().ToLower();
                    if (answer == "y")
                    {
                        i_GameBoard.InitializeBoard(); 
                        continue; 
                    }
                    else
                    {
                        gameRunning = false;
                        break;
                    }
                }
            }
        }

        public static string ShouldContinueGame(string io_Answer)
        {
            while (io_Answer != "y" && io_Answer != "n")
            {
                Console.WriteLine("Type only y/n: ");
                io_Answer = Console.ReadLine().ToLower();
            }
            return io_Answer;
        }

        private static List<Move> GetAllMandatoryCaptures(eCheckerType i_Player, Board i_GameBoard)
        {
            List<Move> o_AllCaptures = new List<Move>();

            for (int row = 0; row < UI.BoardSize; row++)
            {
                for (int col = 0; col < UI.BoardSize; col++)
                {
                    if (i_GameBoard.GetCell(row, col).GetCellContent() == i_Player)
                    {
                        List<Move> pieceCapturesMoves = GetValidCaptureMoves(row, col, i_Player, i_GameBoard);
                        o_AllCaptures.AddRange(pieceCapturesMoves);
                    }
                }
            }

            return o_AllCaptures;
        }

        private static List<Move> GetValidCaptureMoves(int i_FromRow, int i_FromCol, eCheckerType i_Player, Board i_GameBoard)
        {
            List<Move> o_CaptureMoves = new List<Move>();
            int[] rowDirections = { -2, 2 };  
            int[] colDirections = { -2, 2 };  
            bool isKing = i_GameBoard.GetCell(i_FromRow, i_FromCol).IsKing;

            foreach (int rowDir in rowDirections)
            {
                if (!isKing && ((i_Player == eCheckerType.whiteChecker && rowDir > 0) ||
                                (i_Player == eCheckerType.blackChecker && rowDir < 0)))
                {
                    continue;  
                }

                foreach (int colDir in colDirections)
                {
                    int toRow = i_FromRow + rowDir;
                    int toCol = i_FromCol + colDir;
                    int middleRow = i_FromRow + rowDir / 2;
                    int middleCol = i_FromCol + colDir / 2;

                    if (toRow >= 0 && toRow < UI.BoardSize && toCol >= 0 && toCol < UI.BoardSize)
                    {
                        if (i_GameBoard.GetCell(middleRow, middleCol).GetCellContent() ==
                            (i_Player == eCheckerType.whiteChecker ? eCheckerType.blackChecker : eCheckerType.whiteChecker) &&
                            i_GameBoard.GetCell(toRow, toCol).GetCellContent() == eCheckerType.emptyChecker)
                        {
                            o_CaptureMoves.Add(new Move(i_FromRow, i_FromCol, toRow, toCol));
                        }
                    }
                }
            }

            return o_CaptureMoves;
        }

        private static List<Move> GetValidMovesForComputer(Board i_GameBoard)
        {
            List<Move> validMoves = new List<Move>();
            List<Move> captureMoves = new List<Move>();

            for (int row = 0; row < UI.BoardSize; row++)
            {
                for (int col = 0; col < UI.BoardSize; col++)
                {
                    Cell currentCell = i_GameBoard.GetCell(row, col);

                    if (currentCell.GetCellContent() == eCheckerType.blackChecker)
                    {
                        List<Move> currentPieceMoves = GetValidCaptureMoves(row, col, eCheckerType.blackChecker, i_GameBoard);
                        if (currentPieceMoves.Count > 0)
                        {
                            captureMoves.AddRange(currentPieceMoves);
                        }
                        else
                        {
                            for (int toRow = 0; toRow < UI.BoardSize; toRow++)
                            {
                                for (int toCol = 0; toCol < UI.BoardSize; toCol++)
                                {
                                    Move potentialMove = new Move(row, col, toRow, toCol);
                                    if (i_GameBoard.IsValidMove(potentialMove, eCheckerType.blackChecker))
                                    {
                                        validMoves.Add(potentialMove);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return captureMoves.Count > 0 ? captureMoves : validMoves;
        }

        private static string ConvertMoveToString(Move i_Move)
        {
            char fromCol = (char)('a' + i_Move.m_FromCol);
            char toCol = (char)('a' + i_Move.m_ToCol);
            string fromPosition = $"{fromCol}{(char)('A' + i_Move.m_FromRow)}";
            string toPosition = $"{toCol}{(char)('A' + i_Move.m_ToRow)}";
            return $"{fromPosition}>{toPosition}";
        }
        public static int CalculateScore(eCheckerType i_Player, Board i_GameBoard)
        {
            int o_Score = 0;
            foreach (var piece in i_GameBoard.GetPiecesByPlayer(i_Player))
            {
                eCheckerType pieceType = piece.GetCellContent(); 

                if (pieceType == eCheckerType.kingWhiteChecker || pieceType == eCheckerType.kingBlackChecker)
                {
                    o_Score += 4; 
                }
                else
                {
                    o_Score += 1; 
                }
            }
            return o_Score;
        }

        public static bool CheckForWin(Board i_GameBoard)
        {
            bool opponentHasNoPieces = !i_GameBoard.HasPlayerPieces(eCheckerType.whiteChecker) || !i_GameBoard.HasPlayerPieces(eCheckerType.blackChecker);
            bool opponentHasNoValidMoves = !i_GameBoard.HasValidMoves(eCheckerType.whiteChecker) || !i_GameBoard.HasValidMoves(eCheckerType.blackChecker);

            return opponentHasNoPieces || opponentHasNoValidMoves;
        }

        public static bool IsDraw(Board i_GameBoard)
        {
            return !i_GameBoard.HasValidMoves(eCheckerType.whiteChecker) && !i_GameBoard.HasValidMoves(eCheckerType.blackChecker);
        }

        private static bool IsValidMoveFormat(string i_Move)
        {
            if (i_Move.Length != 5) return false;
            if (i_Move[2] != '>') return false;

            char maxLetter = (char)('A' + UI.BoardSize - 1);
            char fromRow = char.ToUpper(i_Move[0]);
            char fromCol = char.ToUpper(i_Move[1]);
            char toRow = char.ToUpper(i_Move[3]);
            char toCol = char.ToUpper(i_Move[4]);

            return fromRow >= 'A' && fromRow <= maxLetter &&
                   fromCol >= 'A' && fromCol <= maxLetter &&
                   toRow >= 'A' && toRow <= maxLetter &&
                   toCol >= 'A' && toCol <= maxLetter;
        }
    }
}

