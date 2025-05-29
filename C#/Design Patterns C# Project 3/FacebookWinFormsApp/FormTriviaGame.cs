using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BasicFacebookFeatures
{
    public partial class FormTriviaGame : Form
    {
        private PictureBox m_PictureBoxFriend;
        private Label m_LabelFriendName;
        private Button m_ButtonGuess;
        private User m_LoggedInUser;
        private Button m_ButtonNewGame;
        private ICollection<Question> m_Questions;
        private ControlQuestion m_ControlQuestion1;
        private ControlQuestion m_ControlQuestion2;
        private ControlQuestion m_ControlQuestion3;
        private ControlQuestion m_ControlQuestion4;
        private ControlQuestion m_ControlQuestion5;
        private ITriviaGame m_Game;
        private Bitmap m_WrongImg = Properties.Resources.WrongIcon;
        private Label m_Label1;
        private Label m_LabelScore;
        private Bitmap m_CorrectImg = Properties.Resources.CorrectIcon;

        public IFriendTriviaGameBuilder Builder { get; set; }
        public ITriviaGameDirector Director { get; set; }

        public FormTriviaGame(User i_LoggedInUser)
        {
            InitializeComponent();
            m_LoggedInUser = i_LoggedInUser;
            Builder = new FriendTriviaGameBuilder(m_LoggedInUser);
            Director = new FriendTriviaGameDirector(Builder);
            startNewGame();
            initControlsQuestions();
        }

        private void initControlsQuestions()
        {
            int i = 0;

            foreach (Control control in Controls)
            {
                if (control is ControlQuestion controlQuestion)
                {
                    controlQuestion.Question = m_Questions.ElementAt(i++);
                    controlQuestion.CorrectAnswerSelected += (sender, e) => setAnswerImage(controlQuestion.PictureBoxAnswer, true);
                    controlQuestion.WrongAnswerSelected += (sender, e) => setAnswerImage(controlQuestion.PictureBoxAnswer, false);
                }
            }
        }

        private void loadSelectedFriendInfo()
        {
            User selectedUser = Builder.GetSelectedFriend();

            if (m_LoggedInUser.PictureNormalURL != null)
            {
                m_PictureBoxFriend.LoadAsync(selectedUser.PictureNormalURL);
                m_LabelFriendName.Text = selectedUser.Name;
            }
        }

        private void updateScore()
        {
            m_LabelScore.Text = m_Game.GetScore().ToString();
        }

        private void buttonGuess_Click(object sender, EventArgs e)
        {
            foreach (Control control in Controls)
            {
                if (control is ControlQuestion controlQuestion)
                {
                    if (controlQuestion.SelectedAnswer != null)
                    {
                        controlQuestion.CheckAnswer();
                        CheckAnswer(controlQuestion.Question, controlQuestion.SelectedAnswer);
                    }
                }
            }
        }

        private void CheckAnswer(Question i_Question, object i_Answer)
        {
            m_Game.CheckAnswer(i_Question, i_Answer);
            updateScore();
        }

        private void setAnswerImage(PictureBox i_PictureBox, bool i_IsCorrect)
        {
            Bitmap image = i_IsCorrect ? m_CorrectImg : m_WrongImg;

            i_PictureBox.Image = image;
            i_PictureBox.Visible = true;
        }

        private void buttonNewGame_Click(object sender, EventArgs e)
        {
            startNewGame();
            updateControlsQuestions();
        }

        private void startNewGame()
        {
            m_Game = Director.Construct();
            m_Questions = m_Game.GetQuestions();
            m_LabelScore.Text = m_Game.GetScore().ToString();
            loadSelectedFriendInfo();
        }

        private void updateControlsQuestions()
        {
            int i = 0;

            foreach (Control control in Controls)
            {
                if (control is ControlQuestion controlQuestion)
                {
                    controlQuestion.Question = m_Questions.ElementAt(i++);
                }
            }
        }

        private void InitializeComponent()
        {
            this.m_PictureBoxFriend = new System.Windows.Forms.PictureBox();
            this.m_LabelFriendName = new System.Windows.Forms.Label();
            this.m_ButtonGuess = new System.Windows.Forms.Button();
            this.m_ButtonNewGame = new System.Windows.Forms.Button();
            this.m_ControlQuestion1 = new ControlQuestion();
            this.m_ControlQuestion2 = new ControlQuestion();
            this.m_ControlQuestion3 = new ControlQuestion();
            this.m_ControlQuestion4 = new ControlQuestion();
            this.m_ControlQuestion5 = new ControlQuestion();
            this.m_Label1 = new System.Windows.Forms.Label();
            this.m_LabelScore = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.m_PictureBoxFriend)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxFriend
            // 
            this.m_PictureBoxFriend.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(227)))), ((int)(((byte)(238)))));
            this.m_PictureBoxFriend.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.m_PictureBoxFriend.Location = new System.Drawing.Point(592, 81);
            this.m_PictureBoxFriend.Name = "pictureBoxFriend";
            this.m_PictureBoxFriend.Size = new System.Drawing.Size(285, 220);
            this.m_PictureBoxFriend.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.m_PictureBoxFriend.TabIndex = 5;
            this.m_PictureBoxFriend.TabStop = false;
            // 
            // labelFriendName
            // 
            this.m_LabelFriendName.AutoSize = true;
            this.m_LabelFriendName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(227)))), ((int)(((byte)(238)))));
            this.m_LabelFriendName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_LabelFriendName.Location = new System.Drawing.Point(623, 34);
            this.m_LabelFriendName.Name = "labelFriendName";
            this.m_LabelFriendName.Size = new System.Drawing.Size(166, 32);
            this.m_LabelFriendName.TabIndex = 6;
            this.m_LabelFriendName.Text = "Israel Israeli";
            // 
            // buttonGuess
            // 
            this.m_ButtonGuess.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(227)))), ((int)(((byte)(238)))));
            this.m_ButtonGuess.Location = new System.Drawing.Point(592, 655);
            this.m_ButtonGuess.Name = "buttonGuess";
            this.m_ButtonGuess.Size = new System.Drawing.Size(271, 87);
            this.m_ButtonGuess.TabIndex = 7;
            this.m_ButtonGuess.Text = "Check answer!";
            this.m_ButtonGuess.UseVisualStyleBackColor = false;
            this.m_ButtonGuess.Click += new System.EventHandler(this.buttonGuess_Click);
            // 
            // buttonNewGame
            // 
            this.m_ButtonNewGame.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(227)))), ((int)(((byte)(238)))));
            this.m_ButtonNewGame.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_ButtonNewGame.Location = new System.Drawing.Point(1299, 12);
            this.m_ButtonNewGame.Name = "buttonNewGame";
            this.m_ButtonNewGame.Size = new System.Drawing.Size(129, 83);
            this.m_ButtonNewGame.TabIndex = 17;
            this.m_ButtonNewGame.Text = "New Game";
            this.m_ButtonNewGame.UseVisualStyleBackColor = false;
            this.m_ButtonNewGame.Click += new System.EventHandler(this.buttonNewGame_Click);
            // 
            // controlQuestion1
            // 
            this.m_ControlQuestion1.BackColor = System.Drawing.Color.Transparent;
            this.m_ControlQuestion1.Location = new System.Drawing.Point(82, 307);
            this.m_ControlQuestion1.Name = "controlQuestion1";
            this.m_ControlQuestion1.Question = null;
            this.m_ControlQuestion1.Size = new System.Drawing.Size(243, 340);
            this.m_ControlQuestion1.TabIndex = 18;
            // 
            // controlQuestion2
            // 
            this.m_ControlQuestion2.BackColor = System.Drawing.Color.Transparent;
            this.m_ControlQuestion2.Location = new System.Drawing.Point(343, 316);
            this.m_ControlQuestion2.Name = "controlQuestion2";
            this.m_ControlQuestion2.Question = null;
            this.m_ControlQuestion2.Size = new System.Drawing.Size(243, 340);
            this.m_ControlQuestion2.TabIndex = 19;
            // 
            // controlQuestion3
            // 
            this.m_ControlQuestion3.BackColor = System.Drawing.Color.Transparent;
            this.m_ControlQuestion3.Location = new System.Drawing.Point(606, 316);
            this.m_ControlQuestion3.Name = "controlQuestion3";
            this.m_ControlQuestion3.Question = null;
            this.m_ControlQuestion3.Size = new System.Drawing.Size(243, 340);
            this.m_ControlQuestion3.TabIndex = 20;
            // 
            // controlQuestion4
            // 
            this.m_ControlQuestion4.BackColor = System.Drawing.Color.Transparent;
            this.m_ControlQuestion4.Location = new System.Drawing.Point(869, 316);
            this.m_ControlQuestion4.Name = "controlQuestion4";
            this.m_ControlQuestion4.Question = null;
            this.m_ControlQuestion4.Size = new System.Drawing.Size(243, 340);
            this.m_ControlQuestion4.TabIndex = 21;
            // 
            // controlQuestion5
            // 
            this.m_ControlQuestion5.BackColor = System.Drawing.Color.Transparent;
            this.m_ControlQuestion5.Location = new System.Drawing.Point(1118, 316);
            this.m_ControlQuestion5.Name = "controlQuestion5";
            this.m_ControlQuestion5.Question = null;
            this.m_ControlQuestion5.Size = new System.Drawing.Size(243, 340);
            this.m_ControlQuestion5.TabIndex = 22;
            // 
            // label1
            // 
            this.m_Label1.AutoSize = true;
            this.m_Label1.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.m_Label1.Location = new System.Drawing.Point(43, 35);
            this.m_Label1.Name = "label1";
            this.m_Label1.Size = new System.Drawing.Size(94, 32);
            this.m_Label1.TabIndex = 23;
            this.m_Label1.Text = "Score:";
            // 
            // labelScore
            // 
            this.m_LabelScore.AutoSize = true;
            this.m_LabelScore.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.m_LabelScore.Location = new System.Drawing.Point(143, 34);
            this.m_LabelScore.Name = "labelScore";
            this.m_LabelScore.Size = new System.Drawing.Size(30, 32);
            this.m_LabelScore.TabIndex = 24;
            this.m_LabelScore.Text = "0";
            // 
            // FormTriviaGame
            // 
            this.BackgroundImage = global::BasicFacebookFeatures.Properties.Resources.FormMainBackground;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(1440, 805);
            this.Controls.Add(this.m_LabelScore);
            this.Controls.Add(this.m_Label1);
            this.Controls.Add(this.m_ControlQuestion5);
            this.Controls.Add(this.m_ControlQuestion4);
            this.Controls.Add(this.m_ControlQuestion3);
            this.Controls.Add(this.m_ControlQuestion2);
            this.Controls.Add(this.m_ControlQuestion1);
            this.Controls.Add(this.m_ButtonNewGame);
            this.Controls.Add(this.m_ButtonGuess);
            this.Controls.Add(this.m_LabelFriendName);
            this.Controls.Add(this.m_PictureBoxFriend);
            this.Name = "FormTriviaGame";
            ((System.ComponentModel.ISupportInitialize)(this.m_PictureBoxFriend)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}