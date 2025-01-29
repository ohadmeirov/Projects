using System;
using System.Drawing;
using System.Windows.Forms;
using CheckersGameLogic;

namespace CheckersGameUI
{
    public partial class FormCheckersGame : Form
    {
        private IndexButton[,] m_CheckersBoard;
        private Game m_Game;
        private FormStartingConditionInfo m_FormStartingGameSettings;
        private int[] m_CoordinatesOfTheMove = new int[4];
        private bool m_IsThereATarget = false;

        public FormCheckersGame()
        {
            InitializeComponent();
            gettingStartingGameSettings();
            manageUserInterfaceBoard();
        }

        private void gettingStartingGameSettings()
        {
            m_FormStartingGameSettings = new FormStartingConditionInfo();
            m_FormStartingGameSettings.ShowDialog();
            m_Game = new Game(m_FormStartingGameSettings.Dimension);
            m_Game.InitializePlayers(
                m_FormStartingGameSettings.Player1,
                m_FormStartingGameSettings.Player2,
                m_FormStartingGameSettings.IsPlayer2AComputer);
            labelNamePlayer1.Text = m_Game.GetPlayer1().UserName;
            labelScorePlayer1.Text = m_Game.GetPlayer1().TotalScore.ToString();
            labelNamePlayer2.Text = m_Game.GetPlayer2().UserName;
            labelScorePlayer2.Text = m_Game.GetPlayer2().TotalScore.ToString();
        }

        private void manageUserInterfaceBoard()
        {
            m_CheckersBoard = new IndexButton[m_FormStartingGameSettings.Dimension, m_FormStartingGameSettings.Dimension];
            int paintingFlagOfBoard = 0;
            for (int i = 0; i < m_CheckersBoard.GetLength(0); i++)
            {
                paintingFlagOfBoard++;
                for (int j = 0; j < m_CheckersBoard.GetLength(1); j++)
                {
                    IndexButton singleButton = new IndexButton();
                    singleButton.Click += new EventHandler(this.click_Button);

                    if (paintingFlagOfBoard % 2 == 0)
                    {
                        singleButton.BackColor = Color.Gray;
                        singleButton.TypeOfCheckerOrSquare = m_Game.EnumBoard[i, j];
                        updateButtonText(singleButton);
                    }
                    else
                    {
                        singleButton.BackColor = Color.White;
                        singleButton.Enabled = false;
                        singleButton.TypeOfCheckerOrSquare = m_Game.EnumBoard[i, j];
                    }

                    paintingFlagOfBoard++;
                    singleButton.Location = new Point(j * 59 + 10 + m_CheckersBoard.GetLength(1), i * 40 + 10);
                    singleButton.Size = new Size(60, 48);
                    m_CheckersBoard[i, j] = singleButton;
                    this.Controls.Add(singleButton);
                    singleButton.CoordinateOfRow = i;
                    singleButton.CoordinateOfColumn = j;
                }
            }
        }

        private void updateButtonText(IndexButton i_Button)
        {
            switch (i_Button.TypeOfCheckerOrSquare)
            {
                case eSingleCheckerOrSquare.BlackChecker:
                    i_Button.Text = "X";
                    break;
                case eSingleCheckerOrSquare.WhiteChecker:
                    i_Button.Text = "O";
                    break;
                case eSingleCheckerOrSquare.BlackKingChecker:
                    i_Button.Text = "K";
                    break;
                case eSingleCheckerOrSquare.WhiteKingChecker:
                    i_Button.Text = "U";
                    break;
                default:
                    i_Button.Text = null;
                    break;
            }
        }

        private void click_Button(object sender, EventArgs e)
        {
            if (!m_IsThereATarget)
            {
                handleFirstClick(sender as IndexButton);
            }
            else
            {
                handleSecondClick(sender as IndexButton);
            }
        }

        private void handleFirstClick(IndexButton i_ClickedButton)
        {
            if (!m_Game.CheckIfTurnIsLegal(i_ClickedButton.TypeOfCheckerOrSquare))
            {
                MessageBox.Show("It is not your move! Please select your own piece.", "Invalid Move",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            m_CoordinatesOfTheMove[0] = i_ClickedButton.CoordinateOfColumn;
            m_CoordinatesOfTheMove[1] = i_ClickedButton.CoordinateOfRow;
            i_ClickedButton.BackColor = Color.Blue;
            m_IsThereATarget = true;
        }

        private void handleSecondClick(IndexButton i_ClickedButton)
        {
            if (i_ClickedButton.CoordinateOfColumn == m_CoordinatesOfTheMove[0] &&
                i_ClickedButton.CoordinateOfRow == m_CoordinatesOfTheMove[1])
            {
                i_ClickedButton.BackColor = Color.Gray;
                m_IsThereATarget = false;
                return;
            }

            m_CoordinatesOfTheMove[2] = i_ClickedButton.CoordinateOfColumn;
            m_CoordinatesOfTheMove[3] = i_ClickedButton.CoordinateOfRow;
            bool isACorrectMove = m_Game.TryMoveInBoard(m_CoordinatesOfTheMove);
            if (isACorrectMove)
            {
                manageBoardWhileInAPlay();
                GameStatus status = m_Game.CheckGameStatus();
                if (status.IsGameOver)
                {
                    handleGameEnd(status);
                }
                else
                {
                    handleNextTurn();
                }
                m_IsThereATarget = false;
            }
            else
            {
                MessageBox.Show("The move was invalid.", "Please try again.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                IndexButton originalButton = m_CheckersBoard[m_CoordinatesOfTheMove[1], m_CoordinatesOfTheMove[0]];
                originalButton.BackColor = Color.Gray;
                m_IsThereATarget = false;
            }
        }

        private void handleNextTurn()
        {
            if (m_Game.GetPlayer1().IsHasTheCurrentTurnNow)
            {
                if (m_Game.GetPlayer2().IsPlayerAComputer)
                {
                    m_Game.MoveOfComputer();
                    manageBoardWhileInAPlay();
                    GameStatus status = m_Game.CheckGameStatus();
                    if (status.IsGameOver)
                    {
                        handleGameEnd(status);
                    }
                }
                else
                {
                    m_Game.SwitchTurns();
                }
            }
            else if (m_Game.GetPlayer2().IsHasTheCurrentTurnNow)
            {
                m_Game.SwitchTurns();
            }
        }

        private void manageBoardWhileInAPlay()
        {
            int colorToggleIndex = 0;
            for (int i = 0; i < m_CheckersBoard.GetLength(0); i++)
            {
                colorToggleIndex++;
                for (int j = 0; j < m_CheckersBoard.GetLength(1); j++)
                {
                    if (colorToggleIndex % 2 == 0)
                    {
                        m_CheckersBoard[i, j].BackColor = Color.Gray;
                        m_CheckersBoard[i, j].TypeOfCheckerOrSquare = m_Game.EnumBoard[i, j];
                        updateButtonText(m_CheckersBoard[i, j]);
                    }
                    else
                    {
                        m_CheckersBoard[i, j].BackColor = Color.White;
                        m_CheckersBoard[i, j].Enabled = false;
                        m_CheckersBoard[i, j].TypeOfCheckerOrSquare = m_Game.EnumBoard[i, j];
                    }

                    colorToggleIndex++;
                    m_CheckersBoard[i, j].Location = new Point(j * 59 + 10 + m_CheckersBoard.GetLength(1), i * 40 + 10);
                    m_CheckersBoard[i, j].Size = new Size(60, 48);
                    m_CheckersBoard[i, j].CoordinateOfRow = i;
                    m_CheckersBoard[i, j].CoordinateOfColumn = j;
                }
            }
        }

        private void handleGameEnd(GameStatus i_Status)
        {
            string message;
            if (i_Status.Winner == null)
            {
                message = string.Format("{0}\nWould you like to play another game?", i_Status.Reason);
            }
            else
            {
                i_Status.Winner.TotalScore += i_Status.ScoreDifference;
                if (i_Status.Winner == m_Game.GetPlayer1())
                {
                    labelScorePlayer1.Text = m_Game.GetPlayer1().TotalScore.ToString();
                }
                else
                {
                    labelScorePlayer2.Text = m_Game.GetPlayer2().TotalScore.ToString();
                }

                message = string.Format("{0} wins! {1}\nPoints earned: {2}\nTotal score: {3}\n\nWould you like to play another game?",
                    i_Status.Winner.UserName, i_Status.Reason, i_Status.ScoreDifference, i_Status.Winner.TotalScore);
            }

            DialogResult result = MessageBox.Show(
                message,
                "Game Over",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                restartGame();
            }
            else
            {
                Application.Exit();
            }
        }

        private void restartGame()
        {
            m_Game.RestartGame();
            labelScorePlayer1.Text = m_Game.GetPlayer1().TotalScore.ToString();
            labelScorePlayer2.Text = m_Game.GetPlayer2().TotalScore.ToString();
            clearBoardUI();
            manageUserInterfaceBoard();
            m_IsThereATarget = false;
        }

        private void clearBoardUI()
        {
            foreach (var button in m_CheckersBoard)
            {
                this.Controls.Remove(button);
            }
        }
    }
}