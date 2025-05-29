namespace BasicFacebookFeatures
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="i_Disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool i_Disposing)
        {
            if (i_Disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(i_Disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label createdTimeLabel;
            System.Windows.Forms.Label messageLabel;
            System.Windows.Forms.Label userNameLabel;
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panelPostDetails = new System.Windows.Forms.Panel();
            this.buttonHidePostDetails = new System.Windows.Forms.Button();
            this.createdTimeDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.messageTextBox = new System.Windows.Forms.TextBox();
            this.userNameTextBox = new System.Windows.Forms.TextBox();
            this.buttonProfile = new System.Windows.Forms.Button();
            this.textBoxAppID = new System.Windows.Forms.TextBox();
            this.linkLabelFetchPosts = new System.Windows.Forms.LinkLabel();
            this.panelPosts = new System.Windows.Forms.Panel();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.textBoxSearch = new System.Windows.Forms.TextBox();
            this.buttonPlayGuessGame = new System.Windows.Forms.Button();
            this.linkLabelFetchFriends = new System.Windows.Forms.LinkLabel();
            this.pictureBoxFriend = new System.Windows.Forms.PictureBox();
            this.listBoxFriends = new System.Windows.Forms.ListBox();
            this.pictureBoxProfile = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonLogout = new System.Windows.Forms.Button();
            this.buttonLogin = new System.Windows.Forms.Button();
            this.tabPageStatistics = new System.Windows.Forms.TabPage();
            this.buttonShowStatistics = new System.Windows.Forms.Button();
            this.chartPostsAnalyzer = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartFriendsAnalyzer = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.bindablePostBindingSource = new System.Windows.Forms.BindingSource(this.components);
            createdTimeLabel = new System.Windows.Forms.Label();
            messageLabel = new System.Windows.Forms.Label();
            userNameLabel = new System.Windows.Forms.Label();
            this.tabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panelPostDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFriend)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProfile)).BeginInit();
            this.tabPageStatistics.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartPostsAnalyzer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartFriendsAnalyzer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindablePostBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // createdTimeLabel
            // 
            createdTimeLabel.AutoSize = true;
            createdTimeLabel.Location = new System.Drawing.Point(12, 78);
            createdTimeLabel.Name = "createdTimeLabel";
            createdTimeLabel.Size = new System.Drawing.Size(199, 36);
            createdTimeLabel.TabIndex = 0;
            createdTimeLabel.Text = "Created Time:";
            // 
            // messageLabel
            // 
            messageLabel.AutoSize = true;
            messageLabel.Location = new System.Drawing.Point(12, 124);
            messageLabel.Name = "messageLabel";
            messageLabel.Size = new System.Drawing.Size(143, 36);
            messageLabel.TabIndex = 2;
            messageLabel.Text = "Message:";
            // 
            // userNameLabel
            // 
            userNameLabel.AutoSize = true;
            userNameLabel.Location = new System.Drawing.Point(12, 171);
            userNameLabel.Name = "userNameLabel";
            userNameLabel.Size = new System.Drawing.Size(171, 36);
            userNameLabel.TabIndex = 4;
            userNameLabel.Text = "User Name:";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.tabPageStatistics);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1375, 697);
            this.tabControl.TabIndex = 54;
            // 
            // tabPage1
            // 
            this.tabPage1.AutoScroll = true;
            this.tabPage1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(227)))), ((int)(((byte)(238)))));
            this.tabPage1.BackgroundImage = global::BasicFacebookFeatures.Properties.Resources.FormMainBackground;
            this.tabPage1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tabPage1.Controls.Add(this.panelPostDetails);
            this.tabPage1.Controls.Add(this.buttonProfile);
            this.tabPage1.Controls.Add(this.textBoxAppID);
            this.tabPage1.Controls.Add(this.linkLabelFetchPosts);
            this.tabPage1.Controls.Add(this.panelPosts);
            this.tabPage1.Controls.Add(this.buttonSearch);
            this.tabPage1.Controls.Add(this.textBoxSearch);
            this.tabPage1.Controls.Add(this.buttonPlayGuessGame);
            this.tabPage1.Controls.Add(this.linkLabelFetchFriends);
            this.tabPage1.Controls.Add(this.pictureBoxFriend);
            this.tabPage1.Controls.Add(this.listBoxFriends);
            this.tabPage1.Controls.Add(this.pictureBoxProfile);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.buttonLogout);
            this.tabPage1.Controls.Add(this.buttonLogin);
            this.tabPage1.Location = new System.Drawing.Point(8, 47);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1359, 642);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Feed";
            // 
            // panelPostDetails
            // 
            this.panelPostDetails.Controls.Add(this.buttonHidePostDetails);
            this.panelPostDetails.Controls.Add(createdTimeLabel);
            this.panelPostDetails.Controls.Add(this.createdTimeDateTimePicker);
            this.panelPostDetails.Controls.Add(messageLabel);
            this.panelPostDetails.Controls.Add(this.messageTextBox);
            this.panelPostDetails.Controls.Add(userNameLabel);
            this.panelPostDetails.Controls.Add(this.userNameTextBox);
            this.panelPostDetails.Location = new System.Drawing.Point(857, 152);
            this.panelPostDetails.Name = "panelPostDetails";
            this.panelPostDetails.Size = new System.Drawing.Size(455, 366);
            this.panelPostDetails.TabIndex = 69;
            this.panelPostDetails.Visible = false;
            // 
            // buttonHidePostDetails
            // 
            this.buttonHidePostDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonHidePostDetails.Location = new System.Drawing.Point(414, 0);
            this.buttonHidePostDetails.Name = "buttonHidePostDetails";
            this.buttonHidePostDetails.Size = new System.Drawing.Size(41, 36);
            this.buttonHidePostDetails.TabIndex = 6;
            this.buttonHidePostDetails.Text = "X";
            this.buttonHidePostDetails.UseVisualStyleBackColor = true;
            this.buttonHidePostDetails.Click += new System.EventHandler(this.buttonHidePostDetails_Click);
            // 
            // createdTimeDateTimePicker
            // 
            this.createdTimeDateTimePicker.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.bindablePostBindingSource, "CreatedTime", true));
            this.createdTimeDateTimePicker.Location = new System.Drawing.Point(217, 74);
            this.createdTimeDateTimePicker.Name = "createdTimeDateTimePicker";
            this.createdTimeDateTimePicker.Size = new System.Drawing.Size(214, 41);
            this.createdTimeDateTimePicker.TabIndex = 1;
            // 
            // messageTextBox
            // 
            this.messageTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindablePostBindingSource, "Message", true));
            this.messageTextBox.Location = new System.Drawing.Point(217, 121);
            this.messageTextBox.Name = "messageTextBox";
            this.messageTextBox.Size = new System.Drawing.Size(214, 41);
            this.messageTextBox.TabIndex = 3;
            // 
            // userNameTextBox
            // 
            this.userNameTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindablePostBindingSource, "UserName", true));
            this.userNameTextBox.Location = new System.Drawing.Point(217, 168);
            this.userNameTextBox.Name = "userNameTextBox";
            this.userNameTextBox.Size = new System.Drawing.Size(214, 41);
            this.userNameTextBox.TabIndex = 5;
            // 
            // buttonProfile
            // 
            this.buttonProfile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(227)))), ((int)(((byte)(238)))));
            this.buttonProfile.Location = new System.Drawing.Point(367, 7);
            this.buttonProfile.Name = "buttonProfile";
            this.buttonProfile.Size = new System.Drawing.Size(65, 55);
            this.buttonProfile.TabIndex = 68;
            this.buttonProfile.Text = "Profile";
            this.buttonProfile.UseVisualStyleBackColor = false;
            this.buttonProfile.Click += new System.EventHandler(this.buttonProfile_Click);
            // 
            // textBoxAppID
            // 
            this.textBoxAppID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(227)))), ((int)(((byte)(238)))));
            this.textBoxAppID.Location = new System.Drawing.Point(9, 14);
            this.textBoxAppID.Name = "textBoxAppID";
            this.textBoxAppID.Size = new System.Drawing.Size(268, 41);
            this.textBoxAppID.TabIndex = 67;
            this.textBoxAppID.Text = "1080272300522389";
            // 
            // linkLabelFetchPosts
            // 
            this.linkLabelFetchPosts.AutoSize = true;
            this.linkLabelFetchPosts.BackColor = System.Drawing.Color.Transparent;
            this.linkLabelFetchPosts.LinkColor = System.Drawing.Color.White;
            this.linkLabelFetchPosts.Location = new System.Drawing.Point(364, 75);
            this.linkLabelFetchPosts.Name = "linkLabelFetchPosts";
            this.linkLabelFetchPosts.Size = new System.Drawing.Size(169, 36);
            this.linkLabelFetchPosts.TabIndex = 66;
            this.linkLabelFetchPosts.TabStop = true;
            this.linkLabelFetchPosts.Text = "Fetch posts";
            this.linkLabelFetchPosts.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelFetchPosts_LinkClicked);
            // 
            // panelPosts
            // 
            this.panelPosts.AutoScroll = true;
            this.panelPosts.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(227)))), ((int)(((byte)(238)))));
            this.panelPosts.Location = new System.Drawing.Point(364, 97);
            this.panelPosts.Name = "panelPosts";
            this.panelPosts.Size = new System.Drawing.Size(487, 561);
            this.panelPosts.TabIndex = 65;
            // 
            // buttonSearch
            // 
            this.buttonSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(227)))), ((int)(((byte)(238)))));
            this.buttonSearch.Location = new System.Drawing.Point(751, 19);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(100, 39);
            this.buttonSearch.TabIndex = 63;
            this.buttonSearch.Text = "Search";
            this.buttonSearch.UseVisualStyleBackColor = false;
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // textBoxSearch
            // 
            this.textBoxSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(227)))), ((int)(((byte)(238)))));
            this.textBoxSearch.Location = new System.Drawing.Point(477, 22);
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.Size = new System.Drawing.Size(268, 41);
            this.textBoxSearch.TabIndex = 62;
            // 
            // buttonPlayGuessGame
            // 
            this.buttonPlayGuessGame.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(227)))), ((int)(((byte)(238)))));
            this.buttonPlayGuessGame.Location = new System.Drawing.Point(1150, 11);
            this.buttonPlayGuessGame.Name = "buttonPlayGuessGame";
            this.buttonPlayGuessGame.Size = new System.Drawing.Size(162, 103);
            this.buttonPlayGuessGame.TabIndex = 60;
            this.buttonPlayGuessGame.Text = "Play Trivia Game";
            this.buttonPlayGuessGame.UseVisualStyleBackColor = false;
            this.buttonPlayGuessGame.Click += new System.EventHandler(this.buttonPlayGuessGame_Click);
            // 
            // linkLabelFetchFriends
            // 
            this.linkLabelFetchFriends.AutoSize = true;
            this.linkLabelFetchFriends.BackColor = System.Drawing.Color.Transparent;
            this.linkLabelFetchFriends.LinkColor = System.Drawing.Color.White;
            this.linkLabelFetchFriends.Location = new System.Drawing.Point(16, 388);
            this.linkLabelFetchFriends.Name = "linkLabelFetchFriends";
            this.linkLabelFetchFriends.Size = new System.Drawing.Size(187, 36);
            this.linkLabelFetchFriends.TabIndex = 58;
            this.linkLabelFetchFriends.TabStop = true;
            this.linkLabelFetchFriends.Text = "Fetch friends";
            this.linkLabelFetchFriends.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelFetchFriends_LinkClicked);
            // 
            // pictureBoxFriend
            // 
            this.pictureBoxFriend.Location = new System.Drawing.Point(129, 461);
            this.pictureBoxFriend.Name = "pictureBoxFriend";
            this.pictureBoxFriend.Size = new System.Drawing.Size(124, 112);
            this.pictureBoxFriend.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxFriend.TabIndex = 57;
            this.pictureBoxFriend.TabStop = false;
            // 
            // listBoxFriends
            // 
            this.listBoxFriends.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(227)))), ((int)(((byte)(238)))));
            this.listBoxFriends.FormattingEnabled = true;
            this.listBoxFriends.ItemHeight = 33;
            this.listBoxFriends.Location = new System.Drawing.Point(9, 427);
            this.listBoxFriends.Name = "listBoxFriends";
            this.listBoxFriends.Size = new System.Drawing.Size(244, 103);
            this.listBoxFriends.TabIndex = 56;
            this.listBoxFriends.SelectedIndexChanged += new System.EventHandler(this.listBoxFriends_SelectedIndexChanged);
            // 
            // pictureBoxProfile
            // 
            this.pictureBoxProfile.Location = new System.Drawing.Point(9, 122);
            this.pictureBoxProfile.Name = "pictureBoxProfile";
            this.pictureBoxProfile.Size = new System.Drawing.Size(100, 90);
            this.pictureBoxProfile.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxProfile.TabIndex = 55;
            this.pictureBoxProfile.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(314, 17);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 36);
            this.label1.TabIndex = 53;
            // 
            // buttonLogout
            // 
            this.buttonLogout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(227)))), ((int)(((byte)(238)))));
            this.buttonLogout.Enabled = false;
            this.buttonLogout.Location = new System.Drawing.Point(9, 82);
            this.buttonLogout.Margin = new System.Windows.Forms.Padding(4);
            this.buttonLogout.Name = "buttonLogout";
            this.buttonLogout.Size = new System.Drawing.Size(268, 32);
            this.buttonLogout.TabIndex = 52;
            this.buttonLogout.Text = "Logout";
            this.buttonLogout.UseVisualStyleBackColor = false;
            this.buttonLogout.Click += new System.EventHandler(this.buttonLogout_Click);
            // 
            // buttonLogin
            // 
            this.buttonLogin.Location = new System.Drawing.Point(9, 42);
            this.buttonLogin.Margin = new System.Windows.Forms.Padding(4);
            this.buttonLogin.Name = "buttonLogin";
            this.buttonLogin.Size = new System.Drawing.Size(268, 32);
            this.buttonLogin.TabIndex = 36;
            this.buttonLogin.Text = "Login";
            this.buttonLogin.UseVisualStyleBackColor = true;
            this.buttonLogin.Click += new System.EventHandler(this.buttonLogin_Click);
            // 
            // tabPageStatistics
            // 
            this.tabPageStatistics.BackgroundImage = global::BasicFacebookFeatures.Properties.Resources.FormMainBackground;
            this.tabPageStatistics.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tabPageStatistics.Controls.Add(this.buttonShowStatistics);
            this.tabPageStatistics.Controls.Add(this.chartPostsAnalyzer);
            this.tabPageStatistics.Controls.Add(this.chartFriendsAnalyzer);
            this.tabPageStatistics.Location = new System.Drawing.Point(8, 47);
            this.tabPageStatistics.Name = "tabPageStatistics";
            this.tabPageStatistics.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageStatistics.Size = new System.Drawing.Size(1359, 642);
            this.tabPageStatistics.TabIndex = 1;
            this.tabPageStatistics.Text = "Statistics";
            this.tabPageStatistics.UseVisualStyleBackColor = true;
            // 
            // buttonShowStatistics
            // 
            this.buttonShowStatistics.Location = new System.Drawing.Point(740, 28);
            this.buttonShowStatistics.Name = "buttonShowStatistics";
            this.buttonShowStatistics.Size = new System.Drawing.Size(143, 89);
            this.buttonShowStatistics.TabIndex = 2;
            this.buttonShowStatistics.Text = "Show Statistics";
            this.buttonShowStatistics.UseVisualStyleBackColor = true;
            this.buttonShowStatistics.Click += new System.EventHandler(this.buttonShowStatistics_Click);
            // 
            // chartPostsAnalyzer
            // 
            chartArea1.Name = "ChartArea1";
            this.chartPostsAnalyzer.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartPostsAnalyzer.Legends.Add(legend1);
            this.chartPostsAnalyzer.Location = new System.Drawing.Point(8, 347);
            this.chartPostsAnalyzer.Name = "chartPostsAnalyzer";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartPostsAnalyzer.Series.Add(series1);
            this.chartPostsAnalyzer.Size = new System.Drawing.Size(703, 280);
            this.chartPostsAnalyzer.TabIndex = 1;
            this.chartPostsAnalyzer.Text = "Post Analyzer";
            // 
            // chartFriendsAnalyzer
            // 
            chartArea2.Name = "ChartArea1";
            this.chartFriendsAnalyzer.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chartFriendsAnalyzer.Legends.Add(legend2);
            this.chartFriendsAnalyzer.Location = new System.Drawing.Point(8, 28);
            this.chartFriendsAnalyzer.Name = "chartFriendsAnalyzer";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chartFriendsAnalyzer.Series.Add(series2);
            this.chartFriendsAnalyzer.Size = new System.Drawing.Size(703, 280);
            this.chartFriendsAnalyzer.TabIndex = 0;
            this.chartFriendsAnalyzer.Text = "Friend Analyzer";
            // 
            // bindablePostBindingSource
            // 
            this.bindablePostBindingSource.DataSource = typeof(BindablePost);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(17F, 33F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1375, 697);
            this.Controls.Add(this.tabControl);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Facebook Analytics";
            this.tabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.panelPostDetails.ResumeLayout(false);
            this.panelPostDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFriend)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProfile)).EndInit();
            this.tabPageStatistics.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartPostsAnalyzer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartFriendsAnalyzer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindablePostBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonLogin;
        private System.Windows.Forms.Button buttonLogout;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPageStatistics;
        private System.Windows.Forms.PictureBox pictureBoxProfile;
        private System.Windows.Forms.ListBox listBoxFriends;
        private System.Windows.Forms.LinkLabel linkLabelFetchFriends;
        private System.Windows.Forms.PictureBox pictureBoxFriend;
        private System.Windows.Forms.Button buttonPlayGuessGame;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.TextBox textBoxSearch;
        private System.Windows.Forms.Panel panelPosts;
        private System.Windows.Forms.LinkLabel linkLabelFetchPosts;
        private System.Windows.Forms.TextBox textBoxAppID;
        private System.Windows.Forms.Button buttonProfile;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartFriendsAnalyzer;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartPostsAnalyzer;
        private System.Windows.Forms.Button buttonShowStatistics;
        private System.Windows.Forms.Panel panelPostDetails;
        private System.Windows.Forms.BindingSource bindablePostBindingSource;
        private System.Windows.Forms.DateTimePicker createdTimeDateTimePicker;
        private System.Windows.Forms.TextBox messageTextBox;
        private System.Windows.Forms.TextBox userNameTextBox;
        private System.Windows.Forms.Button buttonHidePostDetails;
    }
}