using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex02
{
    public class Move
    {
        public int m_FromRow { get; set; }
        public int m_FromCol { get; set; }
        public int m_ToRow { get; set; }
        public int m_ToCol { get; set; }

        public Move(string i_MoveInput)
        {
            m_FromRow = char.ToUpper(i_MoveInput[0]) - 'A';
            m_FromCol = char.ToUpper(i_MoveInput[1]) - 'A';
            m_ToRow = char.ToUpper(i_MoveInput[3]) - 'A';
            m_ToCol = char.ToUpper(i_MoveInput[4]) - 'A';
        }

        public Move(int i_FromRow, int i_FromCol, int i_ToRow, int i_ToCol)
        {
            m_FromRow = i_FromRow;
            m_FromCol = i_FromCol;
            m_ToRow = i_ToRow;
            m_ToCol = i_ToCol;
        }
    }
}
