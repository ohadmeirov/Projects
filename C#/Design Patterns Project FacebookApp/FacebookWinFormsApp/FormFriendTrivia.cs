using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BasicFacebookFeatures
{
    public partial class FormFriendTrivia : Form
    {
        private ListBox listBoxDates;
        private ListBox listBoxAges;
        private ListBox listBoxCities;
        private ListBox listBoxGenders;
        private ListBox listBoxRelationships;
        private PictureBox pictureBoxFriend;
        private Label labelFriendName;
        private Button buttonGuess;
        private User m_LoggedInUser;
        private PictureBox pictureBoxDateAnswer;
        private PictureBox pictureBoxAgeAnswer;
        private PictureBox pictureBoxGenderAnswer;
        private PictureBox pictureBoxCityAnswer;
        private PictureBox pictureBoxRelationshipAnswer;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Button buttonNewGame;
        private FriendTrivia m_Game;
        public FormFriendTrivia(User i_LoggedInUser)
        {
            InitializeComponent();
            m_LoggedInUser = i_LoggedInUser;
            m_Game = new FriendTrivia(m_LoggedInUser);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            m_Game.StartGame();
            initOptions();
        }

        private void initOptions()
        {
            listBoxDates.DataSource = m_Game.DateOptions;
            listBoxAges.DataSource = m_Game.AgeOptions;
            listBoxCities.DataSource = m_Game.CitiesOptions;
            listBoxGenders.DataSource = m_Game.GenderOptions;
            listBoxRelationships.DataSource = m_Game.RelationStatusOptions;
            pictureBoxFriend.LoadAsync(m_Game.ChosenFriend.PictureNormalURL);
            labelFriendName.Text = m_Game.ChosenFriend.Name;
        }

        private void buttonGuess_Click(object sender, EventArgs e)
        {
            checkAnswers();
        }

        private void checkAnswers()
        {
            if (listBoxDates.SelectedItem is DateTime selectedDate)
            {
                setAnswerImage(pictureBoxDateAnswer, m_Game.GuessDate(selectedDate));
            }

            if (listBoxAges.SelectedItem is int selectedAge)
            {
                setAnswerImage(pictureBoxAgeAnswer, m_Game.GuessAge(selectedAge));
            }

            if (listBoxCities.SelectedItem is City selectedCity)
            {
                setAnswerImage(pictureBoxCityAnswer, m_Game.GuessCity(selectedCity));
            }

            if (listBoxGenders.SelectedItem is User.eGender selectedGender)
            {
                setAnswerImage(pictureBoxGenderAnswer, m_Game.GuessGender(selectedGender));
            }

            if (listBoxRelationships.SelectedItem is User.eRelationshipStatus selectedRelation)
            {
                setAnswerImage(pictureBoxRelationshipAnswer, m_Game.GuessRelationStatus(selectedRelation));
            }
        }

        private void startNewGame()
        {
            m_Game = new FriendTrivia(m_LoggedInUser);

            m_Game.StartGame();
            initOptions();
            resetUI();
        }

        private void resetUI()
        { 
            foreach (Control control in this.Controls)
            {
                if (control is ListBox listBox)
                {
                    listBox.ClearSelected();
                }

                if (control is PictureBox pictureBox && pictureBox.Name.Contains("Answer"))
                {
                    pictureBox.Visible = false;
                }
            }
        }

        private void setAnswerImage(PictureBox i_PictureBox, bool i_IsCorrect)
        {
            Bitmap image = i_IsCorrect ? Properties.Resources.CorrectIcon : Properties.Resources.WrongIcon;

            i_PictureBox.Image = image;
            i_PictureBox.Visible = true;
        }

        private void buttonNewGame_Click(object sender, EventArgs e)
        {
            startNewGame();
        }

        private void InitializeComponent()
        {
            this.listBoxDates = new System.Windows.Forms.ListBox();
            this.listBoxAges = new System.Windows.Forms.ListBox();
            this.listBoxCities = new System.Windows.Forms.ListBox();
            this.listBoxGenders = new System.Windows.Forms.ListBox();
            this.listBoxRelationships = new System.Windows.Forms.ListBox();
            this.pictureBoxFriend = new System.Windows.Forms.PictureBox();
            this.labelFriendName = new System.Windows.Forms.Label();
            this.buttonGuess = new System.Windows.Forms.Button();
            this.pictureBoxDateAnswer = new System.Windows.Forms.PictureBox();
            this.pictureBoxAgeAnswer = new System.Windows.Forms.PictureBox();
            this.pictureBoxGenderAnswer = new System.Windows.Forms.PictureBox();
            this.pictureBoxCityAnswer = new System.Windows.Forms.PictureBox();
            this.pictureBoxRelationshipAnswer = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonNewGame = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFriend)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDateAnswer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAgeAnswer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGenderAnswer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCityAnswer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRelationshipAnswer)).BeginInit();
            this.SuspendLayout();
            // 
            // listBoxDates
            // 
            this.listBoxDates.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(227)))), ((int)(((byte)(238)))));
            this.listBoxDates.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxDates.FormattingEnabled = true;
            this.listBoxDates.ItemHeight = 31;
            this.listBoxDates.Location = new System.Drawing.Point(203, 360);
            this.listBoxDates.Name = "listBoxDates";
            this.listBoxDates.Size = new System.Drawing.Size(161, 159);
            this.listBoxDates.TabIndex = 0;
            // 
            // listBoxAges
            // 
            this.listBoxAges.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(227)))), ((int)(((byte)(238)))));
            this.listBoxAges.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxAges.FormattingEnabled = true;
            this.listBoxAges.ItemHeight = 31;
            this.listBoxAges.Location = new System.Drawing.Point(437, 360);
            this.listBoxAges.Name = "listBoxAges";
            this.listBoxAges.Size = new System.Drawing.Size(161, 159);
            this.listBoxAges.TabIndex = 1;
            // 
            // listBoxCities
            // 
            this.listBoxCities.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(227)))), ((int)(((byte)(238)))));
            this.listBoxCities.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxCities.FormattingEnabled = true;
            this.listBoxCities.ItemHeight = 31;
            this.listBoxCities.Location = new System.Drawing.Point(661, 360);
            this.listBoxCities.Name = "listBoxCities";
            this.listBoxCities.Size = new System.Drawing.Size(161, 159);
            this.listBoxCities.TabIndex = 2;
            // 
            // listBoxGenders
            // 
            this.listBoxGenders.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(227)))), ((int)(((byte)(238)))));
            this.listBoxGenders.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxGenders.FormattingEnabled = true;
            this.listBoxGenders.ItemHeight = 31;
            this.listBoxGenders.Location = new System.Drawing.Point(874, 360);
            this.listBoxGenders.Name = "listBoxGenders";
            this.listBoxGenders.Size = new System.Drawing.Size(161, 159);
            this.listBoxGenders.TabIndex = 3;
            // 
            // listBoxRelationships
            // 
            this.listBoxRelationships.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(227)))), ((int)(((byte)(238)))));
            this.listBoxRelationships.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxRelationships.FormattingEnabled = true;
            this.listBoxRelationships.ItemHeight = 31;
            this.listBoxRelationships.Location = new System.Drawing.Point(1107, 360);
            this.listBoxRelationships.Name = "listBoxRelationships";
            this.listBoxRelationships.Size = new System.Drawing.Size(161, 159);
            this.listBoxRelationships.TabIndex = 4;
            // 
            // pictureBoxFriend
            // 
            this.pictureBoxFriend.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(227)))), ((int)(((byte)(238)))));
            this.pictureBoxFriend.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBoxFriend.Location = new System.Drawing.Point(592, 81);
            this.pictureBoxFriend.Name = "pictureBoxFriend";
            this.pictureBoxFriend.Size = new System.Drawing.Size(285, 220);
            this.pictureBoxFriend.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxFriend.TabIndex = 5;
            this.pictureBoxFriend.TabStop = false;
            // 
            // labelFriendName
            // 
            this.labelFriendName.AutoSize = true;
            this.labelFriendName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(227)))), ((int)(((byte)(238)))));
            this.labelFriendName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFriendName.Location = new System.Drawing.Point(623, 34);
            this.labelFriendName.Name = "labelFriendName";
            this.labelFriendName.Size = new System.Drawing.Size(224, 44);
            this.labelFriendName.TabIndex = 6;
            this.labelFriendName.Text = "Israel Israeli";
            // 
            // buttonGuess
            // 
            this.buttonGuess.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(227)))), ((int)(((byte)(238)))));
            this.buttonGuess.Location = new System.Drawing.Point(592, 655);
            this.buttonGuess.Name = "buttonGuess";
            this.buttonGuess.Size = new System.Drawing.Size(271, 87);
            this.buttonGuess.TabIndex = 7;
            this.buttonGuess.Text = "Check answer!";
            this.buttonGuess.UseVisualStyleBackColor = false;
            this.buttonGuess.Click += new System.EventHandler(this.buttonGuess_Click);
            // 
            // pictureBoxDateAnswer
            // 
            this.pictureBoxDateAnswer.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxDateAnswer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBoxDateAnswer.Location = new System.Drawing.Point(252, 570);
            this.pictureBoxDateAnswer.Name = "pictureBoxDateAnswer";
            this.pictureBoxDateAnswer.Size = new System.Drawing.Size(62, 52);
            this.pictureBoxDateAnswer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxDateAnswer.TabIndex = 9;
            this.pictureBoxDateAnswer.TabStop = false;
            this.pictureBoxDateAnswer.Visible = false;
            // 
            // pictureBoxAgeAnswer
            // 
            this.pictureBoxAgeAnswer.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxAgeAnswer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBoxAgeAnswer.Location = new System.Drawing.Point(482, 570);
            this.pictureBoxAgeAnswer.Name = "pictureBoxAgeAnswer";
            this.pictureBoxAgeAnswer.Size = new System.Drawing.Size(62, 52);
            this.pictureBoxAgeAnswer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxAgeAnswer.TabIndex = 10;
            this.pictureBoxAgeAnswer.TabStop = false;
            this.pictureBoxAgeAnswer.Visible = false;
            // 
            // pictureBoxGenderAnswer
            // 
            this.pictureBoxGenderAnswer.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxGenderAnswer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBoxGenderAnswer.Location = new System.Drawing.Point(922, 570);
            this.pictureBoxGenderAnswer.Name = "pictureBoxGenderAnswer";
            this.pictureBoxGenderAnswer.Size = new System.Drawing.Size(62, 52);
            this.pictureBoxGenderAnswer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxGenderAnswer.TabIndex = 11;
            this.pictureBoxGenderAnswer.TabStop = false;
            this.pictureBoxGenderAnswer.Visible = false;
            // 
            // pictureBoxCityAnswer
            // 
            this.pictureBoxCityAnswer.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxCityAnswer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBoxCityAnswer.Location = new System.Drawing.Point(701, 570);
            this.pictureBoxCityAnswer.Name = "pictureBoxCityAnswer";
            this.pictureBoxCityAnswer.Size = new System.Drawing.Size(62, 52);
            this.pictureBoxCityAnswer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxCityAnswer.TabIndex = 11;
            this.pictureBoxCityAnswer.TabStop = false;
            this.pictureBoxCityAnswer.Visible = false;
            // 
            // pictureBoxRelationshipAnswer
            // 
            this.pictureBoxRelationshipAnswer.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxRelationshipAnswer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBoxRelationshipAnswer.Location = new System.Drawing.Point(1158, 570);
            this.pictureBoxRelationshipAnswer.Name = "pictureBoxRelationshipAnswer";
            this.pictureBoxRelationshipAnswer.Size = new System.Drawing.Size(62, 52);
            this.pictureBoxRelationshipAnswer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxRelationshipAnswer.TabIndex = 12;
            this.pictureBoxRelationshipAnswer.TabStop = false;
            this.pictureBoxRelationshipAnswer.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(227)))), ((int)(((byte)(238)))));
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(198, 332);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 31);
            this.label1.TabIndex = 13;
            this.label1.Text = "Birthday";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(227)))), ((int)(((byte)(238)))));
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(430, 332);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 31);
            this.label2.TabIndex = 14;
            this.label2.Text = "Age";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(227)))), ((int)(((byte)(238)))));
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(656, 332);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 31);
            this.label3.TabIndex = 15;
            this.label3.Text = "City";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(227)))), ((int)(((byte)(238)))));
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(867, 332);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 31);
            this.label4.TabIndex = 15;
            this.label4.Text = "Gender";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(227)))), ((int)(((byte)(238)))));
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(1091, 332);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(245, 31);
            this.label5.TabIndex = 16;
            this.label5.Text = "Relationship status";
            // 
            // buttonNewGame
            // 
            this.buttonNewGame.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(227)))), ((int)(((byte)(238)))));
            this.buttonNewGame.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonNewGame.Location = new System.Drawing.Point(1299, 12);
            this.buttonNewGame.Name = "buttonNewGame";
            this.buttonNewGame.Size = new System.Drawing.Size(129, 83);
            this.buttonNewGame.TabIndex = 17;
            this.buttonNewGame.Text = "New Game";
            this.buttonNewGame.UseVisualStyleBackColor = false;
            this.buttonNewGame.Click += new System.EventHandler(this.buttonNewGame_Click);
            // 
            // FormFriendTrivia
            // 
            this.BackgroundImage = global::BasicFacebookFeatures.Properties.Resources.FormMainBackground;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(1440, 805);
            this.Controls.Add(this.buttonNewGame);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBoxRelationshipAnswer);
            this.Controls.Add(this.pictureBoxCityAnswer);
            this.Controls.Add(this.pictureBoxGenderAnswer);
            this.Controls.Add(this.pictureBoxAgeAnswer);
            this.Controls.Add(this.pictureBoxDateAnswer);
            this.Controls.Add(this.buttonGuess);
            this.Controls.Add(this.labelFriendName);
            this.Controls.Add(this.pictureBoxFriend);
            this.Controls.Add(this.listBoxRelationships);
            this.Controls.Add(this.listBoxGenders);
            this.Controls.Add(this.listBoxCities);
            this.Controls.Add(this.listBoxAges);
            this.Controls.Add(this.listBoxDates);
            this.Name = "FormFriendTrivia";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFriend)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDateAnswer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAgeAnswer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGenderAnswer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCityAnswer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRelationshipAnswer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
