namespace CheckersGameLogic
{
    public class Move
    {
        private int m_FromCol;
        private int m_FromRow;
        private int m_ToCol;
        private int m_ToRow;

        public int FromCol
        {
            get { return m_FromCol; }
            set { m_FromCol = value; }
        }

        public int FromRow
        {
            get { return m_FromRow; }
            set { m_FromRow = value; } 
        }

        public int ToCol
        {
            get { return m_ToCol; }
            set { m_ToCol = value; }
        }

        public int ToRow
        {
            get { return m_ToRow; }
            set { m_ToRow = value; }
        }

        public Move(int i_FromCol, int i_FromRow, int i_ToCol, int i_ToRow)
        {
            m_FromCol = i_FromCol;
            m_FromRow = i_FromRow;
            m_ToCol = i_ToCol;
            m_ToRow = i_ToRow;
        }
    }
}
