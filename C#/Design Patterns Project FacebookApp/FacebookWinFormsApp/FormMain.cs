using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FacebookWrapper.ObjectModel;
using FacebookWrapper;
using System.Reflection;
using static BasicFacebookFeatures.FormMain;
using System.Runtime.Remoting.Messaging;
using System.Windows.Forms.DataVisualization.Charting;

namespace BasicFacebookFeatures
{
    public partial class FormMain : Form
    {
        private User m_LoggedInUser;

        public FriendAnalyzer FriendAnalyzer { get; private set; }
        public PostAnalyzer PostAnalyzer { get; private set; }
        public LoginResult LoginResult { get; set; }

        public FormMain()
        {
            InitializeComponent();
            FacebookService.s_CollectionLimit = 25;
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            Clipboard.SetText("design.patterns");

            if (LoginResult == null || m_LoggedInUser == null)
            {
                login();
            }
        }

        private void login()
        {
            LoginResult = FacebookService.Login(
                /// (This is Desig Patter's App ID. replace it with your own)
                textBoxAppID.Text,
                /// requested permissions:
                "email",
                "public_profile",
                "user_friends",
                "user_hometown",
                "user_location",
                "user_gender",
                "user_age_range",
                "user_birthday",
                "user_likes",
                "user_photos",
                "user_posts",
                "user_videos",
                "user_link",
                "user_events"
            /// add any other relevant permissions
            );

            if (string.IsNullOrEmpty(LoginResult.ErrorMessage) && LoginResult.LoggedInUser != null)
            {
                m_LoggedInUser = LoginResult.LoggedInUser;
                buttonLogin.Text = $"Logged in as {m_LoggedInUser.Name}";
                buttonLogin.BackColor = Color.LightGreen;
                pictureBoxProfile.ImageLocation = m_LoggedInUser.PictureNormalURL;
                buttonLogin.Enabled = false;
                buttonLogout.Enabled = true;
            }
        }

        private void buttonLogout_Click(object sender, EventArgs e)
        {
            FacebookService.LogoutWithUI();
            buttonLogin.Text = "Login";
            buttonLogin.BackColor = buttonLogout.BackColor;
            LoginResult = null;
            buttonLogin.Enabled = true;
            buttonLogout.Enabled = false;
        }

        private void linkLabelFetchFriends_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            listBoxFriends.DisplayMember = "Name";
            listBoxFriends.DataSource = m_LoggedInUser.Friends;
        }

        private void listBoxFriends_SelectedIndexChanged(object sender, EventArgs e)
        {
            pictureBoxFriend.ImageLocation = (listBoxFriends.SelectedItem as User).PictureNormalURL;
        }

        private void buttonPlayGuessGame_Click(object sender, EventArgs e)
        {
            if (LoginResult != null)
            {
                FormFriendTrivia formGuessGame = new FormFriendTrivia(m_LoggedInUser);
                formGuessGame.ShowDialog();
            }
        }

        private void linkLabelFetchPosts_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ICollection<Post> posts = m_LoggedInUser.WallPosts;
            clearAndAddPosts(posts);

        }

        private void clearAndAddPosts(ICollection<Post> i_Posts)
        {
            panelPosts.Controls.Clear();
            int yOffset = 0;

            foreach (Post post in i_Posts)
            {
                FacebookPostControl postControl = new FacebookPostControl();
                postControl.Width = panelPosts.Width;
                postControl.GetDataFromPost(post);
                postControl.Location = new Point(10, yOffset);
                yOffset += postControl.Height + 15;
                panelPosts.Controls.Add(postControl);
            }
        }

        private void buttonProfile_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            FormProfile.Instance.Owner = this;
            FormProfile.LoginResult = LoginResult;
            FormProfile.Instance.ShowDialog();
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            search(textBoxSearch.Text);
        }

        private void search(string i_Query)
        {
            ICollection<Post> posts = new List<Post>();

            foreach (Post post in m_LoggedInUser.WallPosts)
            {
                if (post.Message != null && post.Message.Trim().IndexOf(i_Query, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    posts.Add(post);
                }
            }

            clearAndAddPosts(posts);
        }

        private void displayTopLikingFriendsChart()
        {
            var topFriends = FriendAnalyzer.TryGetTopLikeingFriendsOrMockData();
            Series series = new Series("Top Liking Friends")
            {
                ChartType = SeriesChartType.Bar
            };

            chartFriendsAnalyzer.Series.Clear();
            chartFriendsAnalyzer.ChartAreas[0].AxisX.Title = "Like Count";
            chartFriendsAnalyzer.ChartAreas[0].AxisY.Title = "Friend Name";
            foreach (var friend in topFriends)
            {
                series.Points.AddXY(friend.FriendName, friend.LikeCount);
            }

            chartFriendsAnalyzer.Series.Add(series);
        }

        private void displayPostAnalysisChart()
        {
            Series likesSeries = new Series("Total Likes")
            {
                ChartType = SeriesChartType.Bar
            };
            Series postCountSeries = new Series("Post Count")
            {
                ChartType = SeriesChartType.Bar
            };
            var timeOfDayStats = PostAnalyzer.TryGetBestTimeToPostOrMockData();

            chartPostsAnalyzer.Series.Clear();
            chartPostsAnalyzer.ChartAreas[0].AxisX.Title = "Time of Day";
            chartPostsAnalyzer.ChartAreas[0].AxisY.Title = "Counts";
            foreach (var entry in timeOfDayStats)
            {
                likesSeries.Points.AddXY(entry.Key, entry.Value.TotalLikes);
                postCountSeries.Points.AddXY(entry.Key, entry.Value.PostCount);
            }

            chartPostsAnalyzer.Series.Add(likesSeries);
            chartPostsAnalyzer.Series.Add(postCountSeries);
        }

        private void buttonShowStatistics_Click(object sender, EventArgs e)
        {
            LoadStatistics();
        }

        private void LoadStatistics()
        {
            if (FriendAnalyzer == null)
            {
                FriendAnalyzer = new FriendAnalyzer(m_LoggedInUser);
            }
            if (PostAnalyzer == null)
            {
                PostAnalyzer = new PostAnalyzer(m_LoggedInUser);
            }

            displayTopLikingFriendsChart();
            displayPostAnalysisChart();
        }
    }
}