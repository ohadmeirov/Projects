using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BasicFacebookFeatures
{
    public partial class ControlQuestion : UserControl
    {
        private Label m_LabelQuestion;
        private ListBox m_ListBoxAnswers;
        private PictureBox m_PictureBoxAnswer;
        private Question m_Question = null;

        public object SelectedAnswer { get; private set; }
        public event EventHandler CorrectAnswerSelected;
        public event EventHandler WrongAnswerSelected;
        public PictureBox PictureBoxAnswer
        {
            get
            {
                return m_PictureBoxAnswer;
            }
        }

        public Question Question
        {
            get { return m_Question; }
            set
            {
                m_Question = value;
                initFields();
            }
        } 

        public ControlQuestion()
        {
            InitializeComponent();
            m_ListBoxAnswers.SelectedIndexChanged += ListBoxAnswers_SelectedIndexChanged;
        }

        private void ListBoxAnswers_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedAnswer = m_ListBoxAnswers.SelectedItem;
        }

        public void CheckAnswer()
        {
            if (m_Question != null && SelectedAnswer != null)
            {
                bool isCorrect = m_Question.CorrectAnswer == SelectedAnswer;

                if (isCorrect)
                {
                    CorrectAnswerSelected?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    WrongAnswerSelected?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private void initFields()
        {
            if (m_Question != null)
            {
                m_LabelQuestion.Text = m_Question.Text;
                m_ListBoxAnswers.DataSource = m_Question.GetShuffledAnswers();
                m_PictureBoxAnswer.Image = null;
            }
        }

        private void InitializeComponent()
        {
            this.m_LabelQuestion = new System.Windows.Forms.Label();
            this.m_ListBoxAnswers = new System.Windows.Forms.ListBox();
            this.m_PictureBoxAnswer = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.m_PictureBoxAnswer)).BeginInit();
            this.SuspendLayout();
            // 
            // labelQuestion
            // 
            this.m_LabelQuestion.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_LabelQuestion.AutoSize = true;
            this.m_LabelQuestion.BackColor = System.Drawing.SystemColors.Control;
            this.m_LabelQuestion.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.m_LabelQuestion.Location = new System.Drawing.Point(12, 32);
            this.m_LabelQuestion.Name = "labelQuestion";
            this.m_LabelQuestion.Size = new System.Drawing.Size(77, 27);
            this.m_LabelQuestion.TabIndex = 0;
            this.m_LabelQuestion.Text = "label1";
            // 
            // listBoxAnswers
            // 
            this.m_ListBoxAnswers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ListBoxAnswers.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.m_ListBoxAnswers.FormattingEnabled = true;
            this.m_ListBoxAnswers.ItemHeight = 27;
            this.m_ListBoxAnswers.Location = new System.Drawing.Point(17, 62);
            this.m_ListBoxAnswers.Name = "listBoxAnswers";
            this.m_ListBoxAnswers.Size = new System.Drawing.Size(230, 274);
            this.m_ListBoxAnswers.TabIndex = 1;
            // 
            // pictureBoxAnswer
            // 
            this.m_PictureBoxAnswer.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.m_PictureBoxAnswer.Location = new System.Drawing.Point(67, 344);
            this.m_PictureBoxAnswer.Name = "pictureBoxAnswer";
            this.m_PictureBoxAnswer.Size = new System.Drawing.Size(111, 94);
            this.m_PictureBoxAnswer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.m_PictureBoxAnswer.TabIndex = 2;
            this.m_PictureBoxAnswer.TabStop = false;
            // 
            // ControlQuestion
            // 
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.m_PictureBoxAnswer);
            this.Controls.Add(this.m_ListBoxAnswers);
            this.Controls.Add(this.m_LabelQuestion);
            this.Name = "ControlQuestion";
            this.Size = new System.Drawing.Size(264, 441);
            ((System.ComponentModel.ISupportInitialize)(this.m_PictureBoxAnswer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}

