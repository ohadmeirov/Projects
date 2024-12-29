using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Ex02
{
    public class Cell
    {
        private uint m_CellIndex;
        private eCheckerType m_CellContent;
        private int m_Row;
        private int m_Col;
        private bool m_IsKing;
        private char m_DisplayChar;

        public Cell(uint i_CellIndex, int i_Row, int i_Col)
        {
            m_CellIndex = i_CellIndex;
            m_Row = i_Row;
            m_Col = i_Col;
            m_CellContent = eCheckerType.emptyChecker;
            m_IsKing = false;
            m_DisplayChar = ' ';
        }

        public int Row
        {
            get { return m_Row; }
            set { m_Row = value; }
        }

        public int Col
        {
            get { return m_Col; }
            set { m_Col = value; }
        }

        public bool IsKing
        {
            get { return m_IsKing; }
            set { m_IsKing = value; }
        }

        public char DisplayChar
        {
            get { return m_DisplayChar; }
            set { m_DisplayChar = value; }
        }

        public void SetCellContent(eCheckerType i_Content)
        {
            m_CellContent = i_Content;

            if (i_Content == eCheckerType.kingWhiteChecker)
            {
                m_DisplayChar = 'U';
            }
            else if (i_Content == eCheckerType.kingBlackChecker)
            {
                m_DisplayChar = 'K';
            }
            else if (i_Content == eCheckerType.whiteChecker)
            {
                m_DisplayChar = 'O';
            }
            else if (i_Content == eCheckerType.blackChecker)
            {
                m_DisplayChar = 'X';
            }
            else
            {
                m_DisplayChar = ' ';
            }
        }

        public void SetDisplayChar(char i_DisplayChar)
        {
            m_DisplayChar = i_DisplayChar;
        }

        public eCheckerType GetCellContent()
        {
            return m_CellContent;
        }

        public string GetCellContentAsString()
        {
            return m_CellContent == eCheckerType.emptyChecker ? "   " :
                   m_CellContent == eCheckerType.whiteChecker || m_CellContent == eCheckerType.kingWhiteChecker ? " O " :
                   " X ";
        }
    }
}
