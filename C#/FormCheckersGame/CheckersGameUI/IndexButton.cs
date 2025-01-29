using System.Windows.Forms;
using CheckersGameLogic;

namespace CheckersGameUI
{
    public class IndexButton : Button
    {
        private int m_CoordinateOfColumn;
        private int m_CoordinateOfRow;
        private eSingleCheckerOrSquare m_TypeOfCheckerOrSquare;

        public int CoordinateOfColumn
        {
            get { return m_CoordinateOfColumn; }
            set { m_CoordinateOfColumn = value; }
        }

        public int CoordinateOfRow
        {
            get { return m_CoordinateOfRow; }
            set { m_CoordinateOfRow = value; }
        }

        public eSingleCheckerOrSquare TypeOfCheckerOrSquare
        {
            get { return m_TypeOfCheckerOrSquare; }
            set { m_TypeOfCheckerOrSquare = value; }
        }
    }
}
