using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ex02.ConsoleUtils;

namespace Ex02
{
    public class UI
    {
        private static string s_PlayerOneName;
        private static string s_PlayerTwoName;
        private static int s_PlayerOneWins;
        private static int s_PlayerTwoWins;
        private static int s_BoardSize;
        private static bool s_IsTwoPlayerMode;
        public static string PlayerOneName
        {
            get { return s_PlayerOneName; }
            set { s_PlayerOneName = value; }
        }

        public static string PlayerTwoName
        {
            get { return s_PlayerTwoName; }
            set { s_PlayerTwoName = value; }
        }

        public static int PlayerOneWins
        {
            get { return s_PlayerOneWins; }
            set { s_PlayerOneWins = value; }
        }

        public static int PlayerTwoWins
        {
            get { return s_PlayerTwoWins; }
            set { s_PlayerTwoWins = value; }
        }

        public static int BoardSize
        {
            get { return s_BoardSize; }
            set { s_BoardSize = value; }
        }

        public bool IsTwoPlayerMode
        {
            get { return s_IsTwoPlayerMode; }
            set { s_IsTwoPlayerMode = value; }
        }

        private Board m_GameBoard;
        private eCheckerType m_CurrentPlayer;

        public UI()
        {
            SetPlayerName("One");
            SetBoardSize();
            SetGameMode();
            m_GameBoard = new Board(s_BoardSize);
            m_CurrentPlayer = eCheckerType.whiteChecker;

            if (s_IsTwoPlayerMode)
            {
                SetPlayerName("Two");
                Game.RunGame(m_GameBoard, m_CurrentPlayer);
            }
            else
            {
                Game.RunGameAgainstComputer(m_GameBoard, m_CurrentPlayer);
            }
        }

        private void SetPlayerName(string i_PlayerNumber)
        {
            Console.Write($"Enter Player {i_PlayerNumber}'s Name (max 20 characters, no spaces): ");
            string playerName = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(playerName) || playerName.Length > 20 || playerName.Contains(' '))
            {
                Console.Write("Invalid input. Please enter a valid name: ");
                playerName = Console.ReadLine();
            }

            if (i_PlayerNumber == "One")
            {
                s_PlayerOneName = playerName;
            }
            else
            {
                s_PlayerTwoName = playerName;
            }
        }

        private void SetBoardSize()
        {
            Console.Write("Enter Board Size (6, 8, or 10): ");
            int size;
            while (!int.TryParse(Console.ReadLine(), out size) || (size != 6 && size != 8 && size != 10))
            {
                Console.Write("Invalid input. Please enter a valid board size: ");
            }
            s_BoardSize = size;
        }

        private void SetGameMode()
        {
            Console.Write("Choose Game Mode (1 for Two Players, 0 for Against Computer): ");
            int mode;
            while (!int.TryParse(Console.ReadLine(), out mode) || (mode != 0 && mode != 1))
            {
                Console.Write("Invalid input. Please choose a valid mode: ");
            }
            s_IsTwoPlayerMode = mode == 1;
        }
    }

}
