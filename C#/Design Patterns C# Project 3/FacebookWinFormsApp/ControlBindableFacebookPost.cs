using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FacebookWinFormsApp.Observer;

namespace BasicFacebookFeatures
{
    public partial class ControlBindableFacebookPost : UserControl
    {
        private BindablePost m_BindablePost;
        private Button m_ButtonEdit;
        private BindingSource m_BindingSource;
        private FacebookPostSubject m_PostSubject;
        private Button m_ButtonSubscribe;
        private PostEditNotification m_PostEditNotification;

        public event EventHandler EditButtonClicked;

        public ControlBindableFacebookPost(BindablePost i_Post)
        {
            InitializeComponent();
            m_BindablePost = i_Post;
            setupDataBinding();
            setupObserver();
            GetDataFromPost(m_BindablePost.InnerPost);
        }

        private void setupObserver()
        {
            m_PostSubject = new FacebookPostSubject();
            m_PostEditNotification = new PostEditNotification(m_BindablePost.UserName);
        }

        private void setupDataBinding()
        {
            m_BindingSource = new BindingSource();
            m_BindingSource.DataSource = m_BindablePost;
            richTextBoxPostContent.DataBindings.Add("Text", m_BindingSource, "Message", true, DataSourceUpdateMode.OnPropertyChanged);
            labelUserName.DataBindings.Add("Text", m_BindingSource, "Username", true, DataSourceUpdateMode.OnPropertyChanged);
            labelCreatedAt.DataBindings.Add("Text", m_BindingSource, "CreatedTime", true, DataSourceUpdateMode.OnPropertyChanged);
            pictureBoxProfile.DataBindings.Add("ImageLocation", m_BindingSource, "PictureUrl", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        public void GetDataFromPost(Post i_Post)
        {
            labelUserName.Text = i_Post.From?.Name;
            pictureBoxProfile.ImageLocation = i_Post.From?.PictureNormalURL;
            richTextBoxPostContent.Text = m_BindablePost.Message;
            //labelCommentsCount.Text = i_Post.Comments?.Count.ToString();
            if(i_Post.PictureURL != null)
            {
                pictureBoxPostPicture.ImageLocation = i_Post.PictureURL;
                pictureBoxPostPicture.Visible = true;
            }

            labelCreatedAt.Text = i_Post.CreatedTime.ToString();
            OnButtonLikeClick((sender, e) => i_Post.Like());
            OnButtonPostClick((sender, e) => i_Post.Comment(textBoxComment.Text));
        }

        protected override void Dispose(bool m_Disposing)
        {
            if (m_Disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(m_Disposing);
        }
        private void buttonEdit_Click(object sender, EventArgs e)
        {
            EditButtonClicked?.Invoke(this, e);
            m_PostSubject.PostMessage = richTextBoxPostContent.Text;
        }

        public void OnLikeButtonClick()
        {
        }

        public void OnButtonLikeClick(EventHandler i_Event)
        {
            buttonLike.Click += i_Event;
        }
        public void OnButtonPostClick(EventHandler i_Event)
        {
            buttonPost.Click += i_Event;
        }

        public void OnButtonShareClick(EventHandler i_Event)
        {
            buttonShare.Click += i_Event;
        }

        private void buttonComment_Click(object sender, EventArgs e)
        {
            textBoxComment.Visible = !textBoxComment.Visible;
            buttonPost.Visible = !buttonPost.Visible;
            this.Height = this.Height + (textBoxComment.Visible ? 40 : -40);
        }
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ControlBindableFacebookPost));
            this.labelUserName = new System.Windows.Forms.Label();
            this.richTextBoxPostContent = new System.Windows.Forms.RichTextBox();
            this.buttonComment = new System.Windows.Forms.Button();
            this.buttonShare = new System.Windows.Forms.Button();
            this.labelLikesCount = new System.Windows.Forms.Label();
            this.labelCommentsCount = new System.Windows.Forms.Label();
            this.labelSharesCount = new System.Windows.Forms.Label();
            this.labelCreatedAt = new System.Windows.Forms.Label();
            this.textBoxComment = new System.Windows.Forms.TextBox();
            this.buttonPost = new System.Windows.Forms.Button();
            this.buttonLike = new System.Windows.Forms.Button();
            this.m_ButtonEdit = new System.Windows.Forms.Button();
            this.pictureBoxPostPicture = new System.Windows.Forms.PictureBox();
            this.pictureBoxProfile = new System.Windows.Forms.PictureBox();
            this.m_ButtonSubscribe = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPostPicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProfile)).BeginInit();
            this.SuspendLayout();
            // 
            // labelUserName
            // 
            this.labelUserName.AutoSize = true;
            this.labelUserName.Location = new System.Drawing.Point(70, 15);
            this.labelUserName.Name = "labelUserName";
            this.labelUserName.Size = new System.Drawing.Size(60, 14);
            this.labelUserName.TabIndex = 1;
            this.labelUserName.Text = "User Name";
            // 
            // richTextBoxPostContent
            // 
            this.richTextBoxPostContent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxPostContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(227)))), ((int)(((byte)(238)))));
            this.richTextBoxPostContent.Location = new System.Drawing.Point(10, 56);
            this.richTextBoxPostContent.Name = "richTextBoxPostContent";
            this.richTextBoxPostContent.Size = new System.Drawing.Size(472, 150);
            this.richTextBoxPostContent.TabIndex = 2;
            this.richTextBoxPostContent.Text = "";
            // 
            // buttonComment
            // 
            this.buttonComment.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.buttonComment.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(227)))), ((int)(((byte)(238)))));
            this.buttonComment.Location = new System.Drawing.Point(196, 194);
            this.buttonComment.Name = "buttonComment";
            this.buttonComment.Size = new System.Drawing.Size(111, 30);
            this.buttonComment.TabIndex = 4;
            this.buttonComment.Text = "Comment";
            this.buttonComment.UseVisualStyleBackColor = false;
            this.buttonComment.Click += new System.EventHandler(this.buttonComment_Click);
            // 
            // buttonShare
            // 
            this.buttonShare.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonShare.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(227)))), ((int)(((byte)(238)))));
            this.buttonShare.Location = new System.Drawing.Point(369, 194);
            this.buttonShare.Name = "buttonShare";
            this.buttonShare.Size = new System.Drawing.Size(90, 30);
            this.buttonShare.TabIndex = 5;
            this.buttonShare.Text = "Share";
            this.buttonShare.UseVisualStyleBackColor = false;
            // 
            // labelLikesCount
            // 
            this.labelLikesCount.AutoSize = true;
            this.labelLikesCount.BackColor = System.Drawing.Color.Transparent;
            this.labelLikesCount.Location = new System.Drawing.Point(30, 201);
            this.labelLikesCount.Name = "labelLikesCount";
            this.labelLikesCount.Size = new System.Drawing.Size(19, 14);
            this.labelLikesCount.TabIndex = 6;
            this.labelLikesCount.Text = "87";
            // 
            // labelCommentsCount
            // 
            this.labelCommentsCount.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.labelCommentsCount.AutoSize = true;
            this.labelCommentsCount.BackColor = System.Drawing.Color.Transparent;
            this.labelCommentsCount.Location = new System.Drawing.Point(156, 200);
            this.labelCommentsCount.Name = "labelCommentsCount";
            this.labelCommentsCount.Size = new System.Drawing.Size(19, 14);
            this.labelCommentsCount.TabIndex = 7;
            this.labelCommentsCount.Text = "87";
            // 
            // labelSharesCount
            // 
            this.labelSharesCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelSharesCount.AutoSize = true;
            this.labelSharesCount.BackColor = System.Drawing.Color.Transparent;
            this.labelSharesCount.Location = new System.Drawing.Point(327, 201);
            this.labelSharesCount.Name = "labelSharesCount";
            this.labelSharesCount.Size = new System.Drawing.Size(19, 14);
            this.labelSharesCount.TabIndex = 8;
            this.labelSharesCount.Text = "87";
            // 
            // labelCreatedAt
            // 
            this.labelCreatedAt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelCreatedAt.AutoSize = true;
            this.labelCreatedAt.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelCreatedAt.Location = new System.Drawing.Point(136, 35);
            this.labelCreatedAt.Name = "labelCreatedAt";
            this.labelCreatedAt.Size = new System.Drawing.Size(106, 14);
            this.labelCreatedAt.TabIndex = 10;
            this.labelCreatedAt.Text = "01/01/2000 08:00:00";
            // 
            // textBoxComment
            // 
            this.textBoxComment.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxComment.Location = new System.Drawing.Point(10, 237);
            this.textBoxComment.Multiline = true;
            this.textBoxComment.Name = "textBoxComment";
            this.textBoxComment.Size = new System.Drawing.Size(353, 0);
            this.textBoxComment.TabIndex = 11;
            this.textBoxComment.Visible = false;
            // 
            // buttonPost
            // 
            this.buttonPost.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPost.Location = new System.Drawing.Point(369, 237);
            this.buttonPost.Name = "buttonPost";
            this.buttonPost.Size = new System.Drawing.Size(104, 0);
            this.buttonPost.TabIndex = 12;
            this.buttonPost.Text = "Post";
            this.buttonPost.UseVisualStyleBackColor = true;
            this.buttonPost.Visible = false;
            // 
            // buttonLike
            // 
            this.buttonLike.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(227)))), ((int)(((byte)(238)))));
            this.buttonLike.Location = new System.Drawing.Point(61, 194);
            this.buttonLike.Name = "buttonLike";
            this.buttonLike.Size = new System.Drawing.Size(75, 30);
            this.buttonLike.TabIndex = 3;
            this.buttonLike.Text = "Like";
            this.buttonLike.UseVisualStyleBackColor = false;
            // 
            // m_ButtonEdit
            // 
            this.m_ButtonEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ButtonEdit.BackColor = System.Drawing.Color.Transparent;
            this.m_ButtonEdit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("m_ButtonEdit.BackgroundImage")));
            this.m_ButtonEdit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.m_ButtonEdit.Location = new System.Drawing.Point(428, 3);
            this.m_ButtonEdit.Name = "m_ButtonEdit";
            this.m_ButtonEdit.Size = new System.Drawing.Size(43, 53);
            this.m_ButtonEdit.TabIndex = 13;
            this.m_ButtonEdit.UseVisualStyleBackColor = false;
            this.m_ButtonEdit.Click += new System.EventHandler(this.buttonEdit_Click);
            // 
            // pictureBoxPostPicture
            // 
            this.pictureBoxPostPicture.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxPostPicture.Location = new System.Drawing.Point(349, 67);
            this.pictureBoxPostPicture.Name = "pictureBoxPostPicture";
            this.pictureBoxPostPicture.Size = new System.Drawing.Size(131, 120);
            this.pictureBoxPostPicture.TabIndex = 9;
            this.pictureBoxPostPicture.TabStop = false;
            this.pictureBoxPostPicture.Visible = false;
            // 
            // pictureBoxProfile
            // 
            this.pictureBoxProfile.Location = new System.Drawing.Point(10, 10);
            this.pictureBoxProfile.Name = "pictureBoxProfile";
            this.pictureBoxProfile.Size = new System.Drawing.Size(50, 50);
            this.pictureBoxProfile.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxProfile.TabIndex = 0;
            this.pictureBoxProfile.TabStop = false;
            // 
            // m_ButtonSubscribe
            // 
            this.m_ButtonSubscribe.AutoSize = true;
            this.m_ButtonSubscribe.Location = new System.Drawing.Point(321, 10);
            this.m_ButtonSubscribe.Name = "m_ButtonSubscribe";
            this.m_ButtonSubscribe.Size = new System.Drawing.Size(90, 39);
            this.m_ButtonSubscribe.TabIndex = 14;
            this.m_ButtonSubscribe.Text = "Subscribe";
            this.m_ButtonSubscribe.UseVisualStyleBackColor = true;
            this.m_ButtonSubscribe.Click += new System.EventHandler(this.buttonSubscribe_Click);
            // 
            // ControlBindableFacebookPost
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(157)))), ((int)(((byte)(195)))));
            this.Controls.Add(this.m_ButtonSubscribe);
            this.Controls.Add(this.m_ButtonEdit);
            this.Controls.Add(this.buttonPost);
            this.Controls.Add(this.textBoxComment);
            this.Controls.Add(this.buttonShare);
            this.Controls.Add(this.labelCreatedAt);
            this.Controls.Add(this.pictureBoxPostPicture);
            this.Controls.Add(this.buttonComment);
            this.Controls.Add(this.buttonLike);
            this.Controls.Add(this.labelUserName);
            this.Controls.Add(this.pictureBoxProfile);
            this.Controls.Add(this.labelSharesCount);
            this.Controls.Add(this.labelCommentsCount);
            this.Controls.Add(this.labelLikesCount);
            this.Controls.Add(this.richTextBoxPostContent);
            this.Name = "ControlBindableFacebookPost";
            this.Size = new System.Drawing.Size(492, 236);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPostPicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProfile)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.PictureBox pictureBoxProfile;
        private System.Windows.Forms.Label labelUserName;
        private System.Windows.Forms.RichTextBox richTextBoxPostContent;
        private System.Windows.Forms.Button buttonLike;
        private System.Windows.Forms.Button buttonComment;
        private System.Windows.Forms.Button buttonShare;
        private int m_LikesCount;
        private Label labelLikesCount;
        private Label labelCommentsCount;
        private Label labelSharesCount;
        private PictureBox pictureBoxPostPicture;
        private Label labelCreatedAt;
        private TextBox textBoxComment;
        private Button buttonPost;
        private int m_commentsCount;

        private void buttonSubscribe_Click(object sender, EventArgs e)
        {
            if (m_ButtonSubscribe.Text == "UnSubscribe")
            {
                m_PostSubject.Detach(m_PostEditNotification);
                m_ButtonSubscribe.Text = "Subscribe";
            }
            else
            {
                m_PostSubject.Attach(m_PostEditNotification);
                m_ButtonSubscribe.Text = "UnSubscribe";
            }
        }
    }
}