using FacebookWrapper;
using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BasicFacebookFeatures
{
    public partial class FormProfile : Form
    {
        private PictureBox pictureBoxBanner;
        private PictureBox pictureBoxProfile;
        private Label labelName;
        private Panel panelIntro;
        private Panel panelPosts;
        private TextBox textBoxMessage;
        private Panel panelAlbums;
        private Panel panelFriends;
        private Button buttonUploadFiles;
        private Button buttonPost;
        private Panel panelPost;
        private static FormProfile s_Instance;
        private static readonly object sr_CreationalLockContext = new object();
        private Label label1;
        private Label label2;
        private Label label3;
        private Label labelFileDetails;
        private static LoginResult s_LoginResult;

        private FormProfile()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            initFields();
        }

        private void initFields()
        {
            User loggedUser = s_LoginResult?.LoggedInUser;

            if (s_LoginResult == null || loggedUser == null)
            {
                return;
            }

            pictureBoxProfile.LoadAsync(loggedUser.PictureNormalURL);
            labelName.Text = loggedUser.Name;
            fetchCoverImage();
            fetchAlbums();
            fetchFriends();
            fetchIntro();
            Task.Run(() => fetchPosts());
        }

        private void fetchCoverImage()
        {
            User loggedUser = s_LoginResult?.LoggedInUser;

            try
            {
                pictureBoxBanner.LoadAsync(loggedUser.Cover.SourceURL);
            }
            catch (Exception)
            {
                string SourceURL = "http://www.trendycovers.com/covers/Listen_to_your_heart_facebook_cover_1330517429.jpg";
                pictureBoxBanner.LoadAsync(SourceURL);
            }
        }
        private void fetchFriends()
        {
            User loggedUser = s_LoginResult?.LoggedInUser;

            if (loggedUser.Friends != null)
            {
                List<string> friendImageUrls = loggedUser.Friends.Select(friend => friend.PictureNormalURL).ToList();
                addPictureBoxesToPanel(panelFriends, friendImageUrls, 80, 3);
            }
        }

        private void fetchAlbums()
        {
            User loggedUser = s_LoginResult?.LoggedInUser;

            if (loggedUser.Albums != null)
            {
                List<string> albumImageUrls = loggedUser.Albums.Select(album => album.PictureAlbumURL).ToList();
                addPictureBoxesToPanel(panelAlbums, albumImageUrls, 80, 3);
            }
        }

        private void fetchIntro()
        {
            User loggedUser = s_LoginResult?.LoggedInUser;
            int yOffset = 10;

            panelIntro.Controls.Clear();
            addLabelToPanel(panelIntro, "Birthday: " + loggedUser.Birthday, ref yOffset);
            addLabelToPanel(panelIntro, "Relationship Status: " + loggedUser.RelationshipStatus, ref yOffset);
            addLabelToPanel(panelIntro, "Gender: " + loggedUser.Gender, ref yOffset);
            addLabelToPanel(panelIntro, "Hometown: " + loggedUser.Hometown?.Name, ref yOffset);
            addLabelToPanel(panelIntro, "Location: " + loggedUser.Location?.Name, ref yOffset);

            if (loggedUser.Educations != null)
            {
                foreach (var education in loggedUser.Educations)
                {
                    addLabelToPanel(panelIntro, "Education: " + education.School?.Name, ref yOffset);
                }
            }

            if (loggedUser.Languages != null)
            {
                foreach (var language in loggedUser.Languages)
                {
                    addLabelToPanel(panelIntro, "Language: " + language.Name, ref yOffset);
                }
            }
        }

        private void addLabelToPanel(Panel i_Panel, string i_Text, ref int io_YOffset)
        {
            if (!string.IsNullOrEmpty(i_Text))
            {
                Label label = new Label();
                label.Text = i_Text;
                label.Location = new Point(10, io_YOffset);
                label.AutoSize = true;
                i_Panel.Controls.Add(label);
                io_YOffset += label.Height + 5;
            }
        }

        public static FormProfile Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    lock (sr_CreationalLockContext)
                    {
                        if (s_Instance == null)
                        {
                            s_Instance = new FormProfile();
                        }
                    }
                }

                return s_Instance;
            }
        }

        public static LoginResult LoginResult
        {
            get
            {
                return s_LoginResult;
            }
            set
            {
                s_LoginResult = value;
            }
        }

        private void addPictureBoxesToPanel(Panel i_Panel, List<string> i_ImageUrls, int i_PictureBoxSize, int i_Columns)
        {
            int xOffset = 10, yOffset = 10, xSpacing = 10, ySpacing = 10;

            i_Panel.Controls.Clear();
            for (int i = 0; i < i_ImageUrls.Count; i++)
            {
                int column = i % i_Columns;
                int row = i / i_Columns;
                int xPosition = xOffset + (column * (i_PictureBoxSize + xSpacing));
                int yPosition = yOffset + (row * (i_PictureBoxSize + ySpacing));
                PictureBox pictureBox = new PictureBox()
                {
                    Size = new Size(i_PictureBoxSize, i_PictureBoxSize),
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Location = new Point(xPosition, yPosition)
                };

                pictureBox.LoadAsync(i_ImageUrls[i]);
                i_Panel.Controls.Add(pictureBox);
            }
        }

        private void fetchPosts()
        {
            int yOffset = 0;
            ICollection<Post> posts;
            posts = LoginResult?.LoggedInUser?.Posts;

            panelPosts.Controls.Clear();
            if (panelPosts.InvokeRequired)
            {
                panelPosts.Invoke(new Action(fetchPosts));
                return;
            }

            foreach (var post in posts)
            {
                Point location = new Point(10, yOffset);
                FacebookPostControl postControl = createPostControl(post, location);
                yOffset += postControl.Height + 15;
            }
        }

        private FacebookPostControl createPostControl(Post i_Post, Point i_Location)
        {
            FacebookPostControl postControl = new FacebookPostControl()
            {
                Width = panelPosts.Width,
                Location = i_Location
            };

            postControl.GetDataFromPost(i_Post);
            panelPosts.Controls.Add(postControl);

            return postControl;
        }

        private void buttonPost_Click(object sender, EventArgs e)
        {
            User loggedUser = s_LoginResult?.LoggedInUser;
            string filePath = textBoxMessage.Tag as string;

            if (loggedUser != null)
            {
                loggedUser.PostStatus(textBoxMessage.Text, i_PictureURL: filePath);
            }
        }
        private void buttonUploadFiles_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "All Files|*.*";
                openFileDialog.Title = "Select a file";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    textBoxMessage.Tag = filePath;
                    FileInfo fileInfo = new FileInfo(filePath);
                    labelFileDetails.Text = $"File: {fileInfo.Name}\nSize: {fileInfo.Length / 1024} KB";
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            Owner.Visible = true;
        }
        private void InitializeComponent()
        {
            this.pictureBoxBanner = new System.Windows.Forms.PictureBox();
            this.pictureBoxProfile = new System.Windows.Forms.PictureBox();
            this.labelName = new System.Windows.Forms.Label();
            this.panelIntro = new System.Windows.Forms.Panel();
            this.panelPosts = new System.Windows.Forms.Panel();
            this.panelPost = new System.Windows.Forms.Panel();
            this.labelFileDetails = new System.Windows.Forms.Label();
            this.buttonUploadFiles = new System.Windows.Forms.Button();
            this.buttonPost = new System.Windows.Forms.Button();
            this.textBoxMessage = new System.Windows.Forms.TextBox();
            this.panelAlbums = new System.Windows.Forms.Panel();
            this.panelFriends = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBanner)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProfile)).BeginInit();
            this.panelPost.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBoxBanner
            // 
            this.pictureBoxBanner.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBoxBanner.Location = new System.Drawing.Point(118, 12);
            this.pictureBoxBanner.Name = "pictureBoxBanner";
            this.pictureBoxBanner.Size = new System.Drawing.Size(793, 200);
            this.pictureBoxBanner.TabIndex = 0;
            this.pictureBoxBanner.TabStop = false;
            // 
            // pictureBoxProfile
            // 
            this.pictureBoxProfile.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBoxProfile.Location = new System.Drawing.Point(118, 169);
            this.pictureBoxProfile.Name = "pictureBoxProfile";
            this.pictureBoxProfile.Size = new System.Drawing.Size(94, 81);
            this.pictureBoxProfile.TabIndex = 1;
            this.pictureBoxProfile.TabStop = false;
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelName.Location = new System.Drawing.Point(218, 226);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(224, 44);
            this.labelName.TabIndex = 2;
            this.labelName.Text = "Israel Israeli";
            // 
            // panelIntro
            // 
            this.panelIntro.AutoScroll = true;
            this.panelIntro.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(227)))), ((int)(((byte)(238)))));
            this.panelIntro.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelIntro.Location = new System.Drawing.Point(118, 282);
            this.panelIntro.Name = "panelIntro";
            this.panelIntro.Size = new System.Drawing.Size(307, 237);
            this.panelIntro.TabIndex = 3;
            // 
            // panelPosts
            // 
            this.panelPosts.AutoScroll = true;
            this.panelPosts.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(227)))), ((int)(((byte)(238)))));
            this.panelPosts.Location = new System.Drawing.Point(444, 368);
            this.panelPosts.Name = "panelPosts";
            this.panelPosts.Size = new System.Drawing.Size(467, 641);
            this.panelPosts.TabIndex = 4;
            // 
            // panelPost
            // 
            this.panelPost.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(227)))), ((int)(((byte)(238)))));
            this.panelPost.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelPost.Controls.Add(this.labelFileDetails);
            this.panelPost.Controls.Add(this.buttonUploadFiles);
            this.panelPost.Controls.Add(this.buttonPost);
            this.panelPost.Controls.Add(this.textBoxMessage);
            this.panelPost.Location = new System.Drawing.Point(444, 282);
            this.panelPost.Name = "panelPost";
            this.panelPost.Size = new System.Drawing.Size(467, 80);
            this.panelPost.TabIndex = 5;
            // 
            // labelFileDetails
            // 
            this.labelFileDetails.AutoSize = true;
            this.labelFileDetails.Location = new System.Drawing.Point(93, 50);
            this.labelFileDetails.Name = "labelFileDetails";
            this.labelFileDetails.Size = new System.Drawing.Size(0, 25);
            this.labelFileDetails.TabIndex = 3;
            // 
            // buttonUploadFiles
            // 
            this.buttonUploadFiles.Location = new System.Drawing.Point(12, 54);
            this.buttonUploadFiles.Name = "buttonUploadFiles";
            this.buttonUploadFiles.Size = new System.Drawing.Size(75, 23);
            this.buttonUploadFiles.TabIndex = 2;
            this.buttonUploadFiles.Text = "Photo/Video";
            this.buttonUploadFiles.UseVisualStyleBackColor = true;
            this.buttonUploadFiles.Click += new System.EventHandler(this.buttonUploadFiles_Click);
            // 
            // buttonPost
            // 
            this.buttonPost.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.buttonPost.Location = new System.Drawing.Point(406, 14);
            this.buttonPost.Name = "buttonPost";
            this.buttonPost.Size = new System.Drawing.Size(38, 31);
            this.buttonPost.TabIndex = 1;
            this.buttonPost.Text = "Post";
            this.buttonPost.UseVisualStyleBackColor = false;
            this.buttonPost.Click += new System.EventHandler(this.buttonPost_Click);
            // 
            // textBoxMessage
            // 
            this.textBoxMessage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.textBoxMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxMessage.Location = new System.Drawing.Point(6, 14);
            this.textBoxMessage.Multiline = true;
            this.textBoxMessage.Name = "textBoxMessage";
            this.textBoxMessage.Size = new System.Drawing.Size(382, 33);
            this.textBoxMessage.TabIndex = 0;
            this.textBoxMessage.Text = "Wha\'s on your mind?";
            // 
            // panelAlbums
            // 
            this.panelAlbums.AutoScroll = true;
            this.panelAlbums.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(227)))), ((int)(((byte)(238)))));
            this.panelAlbums.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelAlbums.Location = new System.Drawing.Point(118, 540);
            this.panelAlbums.Name = "panelAlbums";
            this.panelAlbums.Size = new System.Drawing.Size(307, 215);
            this.panelAlbums.TabIndex = 4;
            // 
            // panelFriends
            // 
            this.panelFriends.AutoScroll = true;
            this.panelFriends.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(227)))), ((int)(((byte)(238)))));
            this.panelFriends.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelFriends.Location = new System.Drawing.Point(118, 775);
            this.panelFriends.Name = "panelFriends";
            this.panelFriends.Size = new System.Drawing.Size(307, 234);
            this.panelFriends.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(122, 266);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 25);
            this.label1.TabIndex = 6;
            this.label1.Text = "Information";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(122, 524);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 25);
            this.label2.TabIndex = 7;
            this.label2.Text = "Albums";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(122, 759);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 25);
            this.label3.TabIndex = 8;
            this.label3.Text = "Friends";
            // 
            // FormProfile
            // 
            this.AutoScroll = true;
            this.BackgroundImage = global::BasicFacebookFeatures.Properties.Resources.FormProfileBackground;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(997, 1021);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panelFriends);
            this.Controls.Add(this.panelAlbums);
            this.Controls.Add(this.panelPost);
            this.Controls.Add(this.panelPosts);
            this.Controls.Add(this.panelIntro);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.pictureBoxProfile);
            this.Controls.Add(this.pictureBoxBanner);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "FormProfile";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBanner)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProfile)).EndInit();
            this.panelPost.ResumeLayout(false);
            this.panelPost.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
