namespace CheckersGameLogic
{
    public class GameUser
    {
        private string m_UserName;
        private int m_TotalScore = 0;
        private eSingleCheckerOrSquare m_CheckerType;
        private bool m_IsPlayerAComputer = false;
        private bool m_IsHasTheCurrentTurnNow = false;

        public GameUser(string i_UserName)
        {
            m_UserName = i_UserName;
        }

        public string UserName
        {
            get { return m_UserName; }
        }

        public int TotalScore
        {
            get { return m_TotalScore; }
            set { m_TotalScore = value; }
        }

        public eSingleCheckerOrSquare CheckerType
        {
            get { return m_CheckerType; }
            set { m_CheckerType = value; }
        }

        public bool IsPlayerAComputer
        {
            get { return m_IsPlayerAComputer; }
            set { m_IsPlayerAComputer = value; }
        }

        public bool IsHasTheCurrentTurnNow
        {
            get { return m_IsHasTheCurrentTurnNow; }
            set { m_IsHasTheCurrentTurnNow = value; }
        }

        public static GameUser CreatingNewUserForGame(string i_NewUsername)
        {
            GameUser Player = new GameUser(i_NewUsername);

            return Player;
        }
    }
}
