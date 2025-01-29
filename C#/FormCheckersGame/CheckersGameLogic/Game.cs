using System;
using System.Collections.Generic;

namespace CheckersGameLogic
{
    public class Game
    {
        private readonly int r_Dimension;
        private readonly eSingleCheckerOrSquare[,] m_EnumBoard;
        private GameUser m_Player1;
        private GameUser m_Player2;

        public Game(int i_Dimension)
        {
            if (i_Dimension != 6 && i_Dimension != 8 && i_Dimension != 10)
            {
                throw new ArgumentException("Board dimension must be 6, 8, or 10");
            }

            r_Dimension = i_Dimension;
            m_EnumBoard = new eSingleCheckerOrSquare[r_Dimension, r_Dimension];
            initializeBoard();
        }

        public eSingleCheckerOrSquare[,] EnumBoard
        {
            get { return m_EnumBoard; }
        }

        public void InitializePlayers(string i_Player1Name, string i_Player2Name, bool i_IsPlayer2Computer)
        {
            m_Player1 = new GameUser(i_Player1Name);
            m_Player1.CheckerType = eSingleCheckerOrSquare.BlackChecker;
            m_Player1.IsHasTheCurrentTurnNow = true;
            m_Player2 = new GameUser(i_Player2Name);
            m_Player2.CheckerType = eSingleCheckerOrSquare.WhiteChecker;
            m_Player2.IsHasTheCurrentTurnNow = false;
            m_Player2.IsPlayerAComputer = i_IsPlayer2Computer;
        }

        public GameUser GetPlayer1()
        {
            return m_Player1;
        }

        public GameUser GetPlayer2()
        {
            return m_Player2;
        }

        private void initializeBoard()
        {
            int numRowsPerSide = (r_Dimension - 2) / 2;
            for (int i = 0; i < r_Dimension; i++)
            {
                for (int j = 0; j < r_Dimension; j++)
                {
                    m_EnumBoard[i, j] = (i + j) % 2 == 1
                        ? eSingleCheckerOrSquare.BlackEmptySquare
                        : eSingleCheckerOrSquare.WhiteInvalidSquare;
                }
            }

            placePieces(0, numRowsPerSide, eSingleCheckerOrSquare.WhiteChecker);
            placePieces(r_Dimension - numRowsPerSide, r_Dimension, eSingleCheckerOrSquare.BlackChecker);
        }

        private void placePieces(int i_startRow, int i_endRow, eSingleCheckerOrSquare i_pieceType)
        {
            for (int i = i_startRow; i < i_endRow; i++)
            {
                for (int j = 0; j < r_Dimension; j++)
                {
                    if ((i + j) % 2 == 1)
                    {
                        m_EnumBoard[i, j] = i_pieceType;
                    }
                }
            }
        }

        public bool TryMoveInBoard(int[] i_Coordinates)
        {
            if (!areCoordinatesValid(i_Coordinates))
            {
                return false;
            }

            Move move = new Move(
                i_Coordinates[0], i_Coordinates[1],
                i_Coordinates[2], i_Coordinates[3]);

            return tryExecuteMove(move);
        }

        private bool areCoordinatesValid(int[] i_Coords)
        {
            if (i_Coords == null || i_Coords.Length != 4)
            {
                return false;
            }

            foreach (int c in i_Coords)
            {
                if (c < 0 || c >= r_Dimension)
                {
                    return false;
                }
            }

            return true;
        }

        public bool CheckIfTurnIsLegal(eSingleCheckerOrSquare i_CurrentMarkedCheckerType)
        {
            GameUser currentPlayer = m_Player1.IsHasTheCurrentTurnNow ? m_Player1 : m_Player2;
            bool turnIsLegal = false;
            if (currentPlayer.CheckerType == eSingleCheckerOrSquare.BlackChecker)
            {
                turnIsLegal = i_CurrentMarkedCheckerType == eSingleCheckerOrSquare.BlackChecker ||
                             i_CurrentMarkedCheckerType == eSingleCheckerOrSquare.BlackKingChecker;
            }
            else if (currentPlayer.CheckerType == eSingleCheckerOrSquare.WhiteChecker)
            {
                turnIsLegal = i_CurrentMarkedCheckerType == eSingleCheckerOrSquare.WhiteChecker ||
                i_CurrentMarkedCheckerType == eSingleCheckerOrSquare.WhiteKingChecker;
            }

            return turnIsLegal;
        }

        private bool tryExecuteMove(Move i_Move)
        {
            var sourcePiece = m_EnumBoard[i_Move.FromRow, i_Move.FromCol];
            if (!isValidPiece(sourcePiece))
            {
                return false;
            }

            if (isValidBasicMove(i_Move))
            {
                executeBasicMove(i_Move);
                checkForKingPromotion(i_Move.ToRow, i_Move.ToCol);
                return true;
            }

            if (isValidJumpMove(i_Move))
            {
                executeJumpMove(i_Move);
                checkForKingPromotion(i_Move.ToRow, i_Move.ToCol);
                return true;
            }

            return false;
        }

        private bool isValidPiece(eSingleCheckerOrSquare i_Piece)
        {
            return i_Piece == eSingleCheckerOrSquare.WhiteChecker ||
                   i_Piece == eSingleCheckerOrSquare.BlackChecker ||
                   i_Piece == eSingleCheckerOrSquare.WhiteKingChecker ||
                   i_Piece == eSingleCheckerOrSquare.BlackKingChecker;
        }

        private bool isValidBasicMove(Move i_Move)
        {
            var piece = m_EnumBoard[i_Move.FromRow, i_Move.FromCol];
            var targetSquare = m_EnumBoard[i_Move.ToRow, i_Move.ToCol];
            if (targetSquare != eSingleCheckerOrSquare.BlackEmptySquare)
            {
                return false;
            }

            int rowDiff = i_Move.ToRow - i_Move.FromRow;
            int colDiff = Math.Abs(i_Move.ToCol - i_Move.FromCol);
            if (colDiff != 1)
            {
                return false;
            }

            switch (piece)
            {
                case eSingleCheckerOrSquare.WhiteChecker:
                    return rowDiff == 1;
                case eSingleCheckerOrSquare.BlackChecker:
                    return rowDiff == -1;
                case eSingleCheckerOrSquare.WhiteKingChecker:
                case eSingleCheckerOrSquare.BlackKingChecker:
                    return Math.Abs(rowDiff) == 1;
                default:
                    return false;
            }
        }

        private bool isValidJumpMove(Move i_Move)
        {
            var piece = m_EnumBoard[i_Move.FromRow, i_Move.FromCol];
            var targetSquare = m_EnumBoard[i_Move.ToRow, i_Move.ToCol];
            if (targetSquare != eSingleCheckerOrSquare.BlackEmptySquare)
            {
                return false;
            }

            int rowDiff = i_Move.ToRow - i_Move.FromRow;
            int colDiff = Math.Abs(i_Move.ToCol - i_Move.FromCol);
            if (colDiff != 2 || Math.Abs(rowDiff) != 2)
            {
                return false;
            }

            if (piece == eSingleCheckerOrSquare.WhiteChecker && rowDiff != 2)
            {
                return false;
            }

            if (piece == eSingleCheckerOrSquare.BlackChecker && rowDiff != -2)
            {
                return false;
            }

            int jumpedRow = (i_Move.FromRow + i_Move.ToRow) / 2;
            int jumpedCol = (i_Move.FromCol + i_Move.ToCol) / 2;
            var jumpedPiece = m_EnumBoard[jumpedRow, jumpedCol];

            return isEnemyPiece(piece, jumpedPiece);
        }

        private bool isEnemyPiece(eSingleCheckerOrSquare i_Piece, eSingleCheckerOrSquare i_OtherPiece)
        {
            if (i_OtherPiece == eSingleCheckerOrSquare.BlackEmptySquare ||
                i_OtherPiece == eSingleCheckerOrSquare.WhiteInvalidSquare)
            {
                return false;
            }

            bool isWhitePiece = i_Piece == eSingleCheckerOrSquare.WhiteChecker || i_Piece == eSingleCheckerOrSquare.WhiteKingChecker;
            bool isBlackPiece = i_Piece == eSingleCheckerOrSquare.BlackChecker || i_Piece == eSingleCheckerOrSquare.BlackKingChecker;
            bool isWhiteEnemy = i_OtherPiece == eSingleCheckerOrSquare.BlackChecker || i_OtherPiece == eSingleCheckerOrSquare.BlackKingChecker;
            bool isBlackEnemy = i_OtherPiece == eSingleCheckerOrSquare.WhiteChecker || i_OtherPiece == eSingleCheckerOrSquare.WhiteKingChecker;

            return (isWhitePiece && isWhiteEnemy) || (isBlackPiece && isBlackEnemy);
        }

        private void executeBasicMove(Move i_Move)
        {
            m_EnumBoard[i_Move.ToRow, i_Move.ToCol] = m_EnumBoard[i_Move.FromRow, i_Move.FromCol];
            m_EnumBoard[i_Move.FromRow, i_Move.FromCol] = eSingleCheckerOrSquare.BlackEmptySquare;
        }

        private void executeJumpMove(Move i_Move)
        {
            int jumpedRow = (i_Move.FromRow + i_Move.ToRow) / 2;
            int jumpedCol = (i_Move.FromCol + i_Move.ToCol) / 2;
            m_EnumBoard[i_Move.ToRow, i_Move.ToCol] = m_EnumBoard[i_Move.FromRow, i_Move.FromCol];
            m_EnumBoard[i_Move.FromRow, i_Move.FromCol] = eSingleCheckerOrSquare.BlackEmptySquare;
            m_EnumBoard[jumpedRow, jumpedCol] = eSingleCheckerOrSquare.BlackEmptySquare;
        }

        private void checkForKingPromotion(int i_Row, int i_Col)
        {
            var piece = m_EnumBoard[i_Row, i_Col];

            if (i_Row == r_Dimension - 1 && piece == eSingleCheckerOrSquare.WhiteChecker)
            {
                m_EnumBoard[i_Row, i_Col] = eSingleCheckerOrSquare.WhiteKingChecker;
            }
            else if (i_Row == 0 && piece == eSingleCheckerOrSquare.BlackChecker)
            {
                m_EnumBoard[i_Row, i_Col] = eSingleCheckerOrSquare.BlackKingChecker;
            }
        }

        public void MoveOfComputer()
        {
            List<Move> possibleMoves = GetAllPossibleMoves(eSingleCheckerOrSquare.WhiteChecker);
            if (possibleMoves.Count > 0)
            {
                List<Move> jumpMoves = new List<Move>();
                foreach (var move in possibleMoves)
                {
                    if (Math.Abs(move.ToRow - move.FromRow) == 2)
                    {
                        jumpMoves.Add(move);
                    }
                }

                Move moveToMake;
                if (jumpMoves.Count > 0)
                {
                    moveToMake = jumpMoves[0];
                }
                else
                {
                    moveToMake = possibleMoves[0];
                }
                tryExecuteMove(moveToMake);
            }
        }

        public List<Move> GetAllPossibleMoves(eSingleCheckerOrSquare i_PlayerType)
        {
            List<Move> moves = new List<Move>();
            for (int i = 0; i < r_Dimension; i++)
            {
                for (int j = 0; j < r_Dimension; j++)
                {
                    if (m_EnumBoard[i, j] == i_PlayerType ||
                        m_EnumBoard[i, j] == getKingType(i_PlayerType))
                    {
                        addPossibleMovesForPiece(moves, i, j);
                    }
                }
            }

            return moves;
        }

        private eSingleCheckerOrSquare getKingType(eSingleCheckerOrSquare i_PieceType)
        {
            return i_PieceType == eSingleCheckerOrSquare.WhiteChecker
                ? eSingleCheckerOrSquare.WhiteKingChecker
                : eSingleCheckerOrSquare.BlackKingChecker;
        }

        private void addPossibleMovesForPiece(List<Move> i_Moves, int i_Row, int i_Col)
        {
            int[] rowOffsets = { 1, -1 };
            int[] colOffsets = { 1, -1 };
            foreach (int rowOffset in rowOffsets)
            {
                foreach (int colOffset in colOffsets)
                {
                    Move move = new Move(i_Col, i_Row, i_Col + colOffset, i_Row + rowOffset);
                    if (areCoordinatesValid(new[] { move.FromCol, move.FromRow, move.ToCol, move.ToRow }) &&
                        isValidBasicMove(move))
                    {
                        i_Moves.Add(move);
                    }

                    Move jumpMove = new Move(i_Col, i_Row, i_Col + 2 * colOffset, i_Row + 2 * rowOffset);
                    if (areCoordinatesValid(new[] { jumpMove.FromCol, jumpMove.FromRow, jumpMove.ToCol, jumpMove.ToRow }) &&
                        isValidJumpMove(jumpMove))
                    {
                        i_Moves.Add(jumpMove);
                    }
                }
            }
        }

        public GameStatus CheckGameStatus()
        {
            GameStatus status = new GameStatus();
            int player1Points = 0;
            int player2Points = 0;
            bool player1HasMoves = false;
            bool player2HasMoves = false;

            for (int i = 0; i < EnumBoard.GetLength(0); i++)
            {
                for (int j = 0; j < EnumBoard.GetLength(1); j++)
                {
                    var piece = EnumBoard[i, j];
                    switch (piece)
                    {
                        case eSingleCheckerOrSquare.WhiteChecker:
                            player2Points += 1;
                            break;
                        case eSingleCheckerOrSquare.WhiteKingChecker:
                            player2Points += 4;
                            break;
                        case eSingleCheckerOrSquare.BlackChecker:
                            player1Points += 1;
                            break;
                        case eSingleCheckerOrSquare.BlackKingChecker:
                            player1Points += 4;
                            break;
                    }
                }
            }

            var player1Moves = GetAllPossibleMoves(eSingleCheckerOrSquare.BlackChecker);
            var player2Moves = GetAllPossibleMoves(eSingleCheckerOrSquare.WhiteChecker);
            player1HasMoves = player1Moves.Count > 0;
            player2HasMoves = player2Moves.Count > 0;
            if (!player1HasMoves && !player2HasMoves)
            {
                status.IsGameOver = true;
                status.Reason = "Game ends in a draw! Both players have no legal moves.";
                status.ScoreDifference = 0;
            }
            else if (!player1HasMoves || !player1HasMoves || player1Points == 0 || player2Points == 0)
            {
                status.IsGameOver = true;
                if (player1Points == 0 || !player1HasMoves)
                {
                    status.Winner = m_Player2;
                    status.ScoreDifference = player2Points - player1Points;
                    status.Reason = player1Points == 0 ?
                        "Player 2 captured all of Player 1's pieces!" :
                        "Player 1 has no legal moves!";
                }
                else
                {
                    status.Winner = m_Player1;
                    status.ScoreDifference = player1Points - player2Points;
                    status.Reason = player2Points == 0 ?
                        "Player 1 captured all of Player 2's pieces!" :
                        "Player 2 has no legal moves!";
                }
            }

            return status;
        }

        public void SwitchTurns()
        {
            m_Player1.IsHasTheCurrentTurnNow = !m_Player1.IsHasTheCurrentTurnNow;
            m_Player2.IsHasTheCurrentTurnNow = !m_Player2.IsHasTheCurrentTurnNow;
        }

        public void RestartGame()
        {
            initializeBoard();
            m_Player1.IsHasTheCurrentTurnNow = true;
            m_Player2.IsHasTheCurrentTurnNow = false;
        }
    }
}