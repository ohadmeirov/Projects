using System;

namespace CheckersGameLogic
{
    public class GameStatus
    {
        private bool m_IsGameOver;
        private GameUser m_Winner;
        private string m_Reason;
        private int m_ScoreDifference;

        public bool IsGameOver
        {
            get { return m_IsGameOver; }
            set { m_IsGameOver = value; }
        }

        public GameUser Winner
        {
            get { return m_Winner; }
            set { m_Winner = value; }
        }

        public string Reason
        {
            get { return m_Reason; }
            set { m_Reason = value; }
        }

        public int ScoreDifference
        {
            get { return m_ScoreDifference; }
            set { m_ScoreDifference = value; }
        }
    }
}
