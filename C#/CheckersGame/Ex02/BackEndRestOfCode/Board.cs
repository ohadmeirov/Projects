using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex02
{
    public class Board
    {
        private Cell[] m_Board;
        private int m_BoardSize;

        public Board(int i_BoardSize)
        {
            m_BoardSize = i_BoardSize;
            m_Board = new Cell[m_BoardSize * m_BoardSize];

            for (int i = 0; i < m_Board.Length; i++)
            {
                int row = i / m_BoardSize;  
                int col = i % m_BoardSize;  
                m_Board[i] = new Cell((uint)i, row, col);  
            }

            InitializeBoard();
        }

        public void InitializeBoard()
        {
            int playerRows = m_BoardSize / 2 - 1;

            for (int row = 0; row < m_BoardSize; row++)
            {
                for (int col = 0; col < m_BoardSize; col++)
                {
                    if (row < playerRows)
                    {
                        if ((row + col) % 2 != 0)
                        {
                            m_Board[row * m_BoardSize + col].SetCellContent(eCheckerType.whiteChecker);
                        }
                    }
                    else if (row >= m_BoardSize - playerRows)
                    {
                        if ((row + col) % 2 != 0)
                        {
                            m_Board[row * m_BoardSize + col].SetCellContent(eCheckerType.blackChecker);
                        }
                    }
                    else
                    {
                        m_Board[row * m_BoardSize + col].SetCellContent(eCheckerType.emptyChecker);
                    }
                }
            }
        }


        public void BuildBoard()
        {
            PrintColumnHeaders();
            PrintBoardSeparator();
            PrintRows();
        }

        public void PrintColumnHeaders()
        {
            Console.Write("  ");
            for (char column = 'A'; column < 'A' + m_BoardSize; column++)
            {
                Console.Write($" {column}  ");
            }
            Console.WriteLine();
        }

        public void PrintBoardSeparator()
        {
            Console.WriteLine(new string('=', m_BoardSize * 4 + 2));
        }

        public void PrintRows()
        {
            for (int row = 0; row < m_BoardSize; row++)
            {
                PrintRowLabel(row);
                PrintRowContent(row);
                PrintBoardSeparator();
            }
        }

        public bool IsValidMove(Move i_Move, eCheckerType i_CurrentPlayer)
        {
            Cell fromCell = GetCell(i_Move.m_FromRow, i_Move.m_FromCol);
            Cell toCell = GetCell(i_Move.m_ToRow, i_Move.m_ToCol);

            if (fromCell.GetCellContent() != i_CurrentPlayer && fromCell.GetCellContent() != GetKingForPlayer(i_CurrentPlayer))
            {
                return false;
            }
            bool isKingMove = fromCell.GetCellContent() == eCheckerType.kingWhiteChecker || fromCell.GetCellContent() == eCheckerType.kingBlackChecker;
            int rowDiff = i_Move.m_ToRow - i_Move.m_FromRow; 
            int colDiff = Math.Abs(i_Move.m_ToCol - i_Move.m_FromCol); 

            if (Math.Abs(rowDiff) == 1 && colDiff == 1 && toCell.GetCellContent() == eCheckerType.emptyChecker)
            {
                if (isKingMove)
                {
                    return true;
                }

                if ((i_CurrentPlayer == eCheckerType.whiteChecker && rowDiff == 1) || 
                    (i_CurrentPlayer == eCheckerType.blackChecker && rowDiff == -1)) 
                {
                    return true;
                }

                return false;
            }

            if (Math.Abs(rowDiff) == 2 && colDiff == 2)
            {
                int middleRow = (i_Move.m_FromRow + i_Move.m_ToRow) / 2;
                int middleCol = (i_Move.m_FromCol + i_Move.m_ToCol) / 2;
                Cell middleCell = GetCell(middleRow, middleCol);

                if (middleCell.GetCellContent() != i_CurrentPlayer &&
                    middleCell.GetCellContent() != eCheckerType.emptyChecker &&
                    toCell.GetCellContent() == eCheckerType.emptyChecker)
                {
                    if (isKingMove)
                    {
                        return true;
                    }

                    if ((i_CurrentPlayer == eCheckerType.whiteChecker && rowDiff == 2) || 
                        (i_CurrentPlayer == eCheckerType.blackChecker && rowDiff == -2)) 
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static eCheckerType GetKingForPlayer(eCheckerType i_CurrentPlayer)
        {
            if (i_CurrentPlayer == eCheckerType.whiteChecker)
            {
                return eCheckerType.kingWhiteChecker;
            }
            return eCheckerType.kingBlackChecker;
        }

        public void MakeMove(Move i_Move)
        {
            Cell fromCell = GetCell(i_Move.m_FromRow, i_Move.m_FromCol);
            Cell toCell = GetCell(i_Move.m_ToRow, i_Move.m_ToCol);

            if (Math.Abs(i_Move.m_FromRow - i_Move.m_ToRow) == 2 && Math.Abs(i_Move.m_FromCol - i_Move.m_ToCol) == 2)
            {
                int midRow = (i_Move.m_FromRow + i_Move.m_ToRow) / 2;
                int midCol = (i_Move.m_FromCol + i_Move.m_ToCol) / 2;
                GetCell(midRow, midCol).SetCellContent(eCheckerType.emptyChecker);
            }

            toCell.SetCellContent(fromCell.GetCellContent());
            fromCell.SetCellContent(eCheckerType.emptyChecker);

            if ((toCell.GetCellContent() == eCheckerType.whiteChecker && i_Move.m_ToRow == 0) ||
                (toCell.GetCellContent() == eCheckerType.blackChecker && i_Move.m_ToRow == m_BoardSize - 1))
            {
                eCheckerType newKing = GetKingForPlayer(fromCell.GetCellContent());
                toCell.SetCellContent(newKing);

                if (newKing == eCheckerType.kingWhiteChecker)
                {
                    toCell.SetDisplayChar('U');
                }
                else if (newKing == eCheckerType.kingBlackChecker)
                {
                    toCell.SetDisplayChar('K');
                }
            }
        }


        public Cell GetCell(int i_Row, int i_Col)
        {
            return m_Board[i_Row * m_BoardSize + i_Col];
        }

        public void PrintRowLabel(int i_Row)
        {
            Console.Write((char)('a' + i_Row) + "|");
        }

        public void PrintRowContent(int i_Row)
        {
            for (int col = 0; col < m_BoardSize; col++)
            {
                Console.Write(m_Board[i_Row * m_BoardSize + col].GetCellContentAsString());
                Console.Write("|");
            }
            Console.WriteLine();
        }

        public bool HasValidMoves(eCheckerType i_Player)
        {
            for (int row = 0; row < m_BoardSize; row++)
            {
                for (int col = 0; col < m_BoardSize; col++)
                {
                    Cell currentCell = GetCell(row, col);

                    if (currentCell.GetCellContent() == i_Player)
                    {
                        for (int toRow = 0; toRow < m_BoardSize; toRow++)
                        {
                            for (int toCol = 0; toCol < m_BoardSize; toCol++)
                            {
                                Move potentialMove = new Move(row, col, toRow, toCol);
                                if (IsValidMove(potentialMove, i_Player))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
       
        public List<Move> GetValidMoves(eCheckerType i_Player)
        {
            List<Move> o_ValidMoves = new List<Move>();

            for (int row = 0; row < m_BoardSize; row++)
            {
                for (int col = 0; col < m_BoardSize; col++)
                {
                    Cell currentCell = GetCell(row, col);

                    if (currentCell.GetCellContent() == i_Player)
                    {
                        for (int toRow = 0; toRow < m_BoardSize; toRow++)
                        {
                            for (int toCol = 0; toCol < m_BoardSize; toCol++)
                            {
                                Move potentialMove = new Move(row, col, toRow, toCol);
                                if (IsValidMove(potentialMove, i_Player))
                                {
                                    o_ValidMoves.Add(potentialMove);
                                }
                            }
                        }
                    }
                }
            }
            return o_ValidMoves;
        }


        public bool HasPlayerPieces(eCheckerType i_Player)
        {
            foreach (Cell cell in m_Board)
            {
                if (cell.GetCellContent() == i_Player || cell.GetCellContent() == GetKingForPlayer(i_Player))
                {
                    return true;
                }
            }
            return false;
        }

        public List<Cell> GetPiecesByPlayer(eCheckerType i_Player)
        {
            List<Cell> pieces = new List<Cell>();
            foreach (Cell cell in m_Board)
            {
                if (cell.GetCellContent() == i_Player || cell.GetCellContent() == GetKingForPlayer(i_Player))
                {
                    pieces.Add(cell);
                }
            }
            return pieces;
        }

        public void MakeKing(int i_Row, int i_Col)
        {
            Cell cell = GetCell(i_Row, i_Col);
            if (cell.GetCellContent() == eCheckerType.whiteChecker)
            {
                cell.SetCellContent(eCheckerType.kingWhiteChecker);
            }
            else if (cell.GetCellContent() == eCheckerType.blackChecker)
            {
                cell.SetCellContent(eCheckerType.kingBlackChecker);
            }
        }


        public bool HasCapturedPiece(Move i_Move)
        {
            int midRow = (i_Move.m_FromRow + i_Move.m_ToRow) / 2;
            int midCol = (i_Move.m_FromCol + i_Move.m_ToCol) / 2;
            Cell midCell = GetCell(midRow, midCol);
            return midCell.GetCellContent() != eCheckerType.emptyChecker;
        }
    }
}
