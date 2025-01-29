using System;
using System.Windows.Forms;

namespace CheckersGameUI
{
    public partial class FormStartingConditionInfo : Form
    {
        private int m_Dimension;
        private string m_Player1;
        private string m_Player2 = "Computer";
        private bool m_IsPlayer2AComputer = true;

        public string Player1
        {
            get { return m_Player1; }
        }

        public string Player2
        {
            get { return m_Player2; }
        }

        public int Dimension
        {
            get { return m_Dimension; }
        }

        public bool IsPlayer2AComputer
        {
            get { return m_IsPlayer2AComputer; }
        }

        public FormStartingConditionInfo()
        {
            InitializeComponent();
        }

        private void textBoxPlayer1_TextChanged(object sender, EventArgs e)
        {
            m_Player1 = textBoxPlayer1.Text;
        }

        private void checkBoxIsRealPlayer_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxIsRealPlayer.Checked == false)
            {
                textBoxPlayer2.Enabled = false;
                textBoxPlayer2.TabStop = false;
                m_IsPlayer2AComputer = true;
                textBoxPlayer2.Text = "Computer";
                m_Player2 = textBoxPlayer2.Text;
            }
            else if (checkBoxIsRealPlayer.Checked == true)
            {
                m_IsPlayer2AComputer = false;
                textBoxPlayer2.Text = null;
                m_Player2 = textBoxPlayer2.Text;
                textBoxPlayer2.Enabled = true;
                textBoxPlayer2.TabStop = true;
            }
        }

        private void textBoxPlayer2_TextChanged(object sender, EventArgs e)
        {
            m_Player2 = textBoxPlayer2.Text;
        }

        private void buttonDone_Click(object sender, EventArgs e)
        {
            if (!validatePlayerName(m_Player1, "Player1") ||
                (checkBoxIsRealPlayer.Checked && !validatePlayerName(m_Player2, "Player2")))
            {
                return;
            }

            setBoardDimension();
            DialogResult = DialogResult.OK;
            Close();
        }

        private bool validatePlayerName(string playerName, string playerLabel)
        {
            if (string.IsNullOrEmpty(playerName))
            {
                var result = MessageBox.Show($"{playerLabel} doesn't have a name.",
                                             "Please enter a name.",
                                             MessageBoxButtons.RetryCancel,
                                             MessageBoxIcon.Error);
                return result != DialogResult.Retry;
            }

            return true;
        }

        private void setBoardDimension()
        {
            if (radioButton6x6.Checked)
            {
                m_Dimension = 6;
            }
            else if (radioButton8x8.Checked)
            {
                m_Dimension = 8;
            }
            else if (radioButton10x10.Checked)
            {
                m_Dimension = 10;
            }
        }
    }
}
