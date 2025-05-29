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
using BasicFacebookFeatures.BasicFacebookFeatures;
using FacebookWinFormsApp.Iterator;

namespace BasicFacebookFeatures
{
    public partial class FormMain : Form
    {
        private User m_LoggedInUser;
        public FacebookAnalyticsFacade AnalyticsFacade { get; private set; }
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
                initializeAnalyticsFacade();
            }
            else //testing ////////////////////////!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            {
                LoginResult = FacebookService.Connect("EAAPWgH67u5UBO4FwkKoc7eVvvi3TYOBaHBUqdXlTNiAKDlpoZBFSTMZA7LwmKyZAm66USg1PzvPXNy0QOybgM5qQTlG2r4eMOrbweR9QxNCiCqoinf5oBf8fj3Jl7pTheAOlLgrdSleUZC9wHJ2lc3jp9jyvPIBCmfVSZAF4wRd29pcSpHGS3tgZDZD");
                m_LoggedInUser = LoginResult.LoggedInUser;
                buttonLogin.Text = $"Logged in as {m_LoggedInUser.Name}";
                buttonLogin.BackColor = Color.LightGreen;
                pictureBoxProfile.ImageLocation = m_LoggedInUser.PictureNormalURL;
                buttonLogin.Enabled = false;
                buttonLogout.Enabled = true;
                initializeAnalyticsFacade();
            }
        }
        
        private void initializeAnalyticsFacade()
        {
            AnalyticsFacade = new FacebookAnalyticsFacade(m_LoggedInUser);
        }

        private void buttonLogout_Click(object sender, EventArgs e)
        {
            FacebookService.LogoutWithUI();
            buttonLogin.Text = "Login";
            buttonLogin.BackColor = buttonLogout.BackColor;
            pictureBoxProfile.ImageLocation = null;
            LoginResult = null;
            m_LoggedInUser = null;
            AnalyticsFacade = null;
            buttonLogin.Enabled = true;
            buttonLogout.Enabled = false;
            clearUIData();
        }
        
        private void clearUIData()
        {
            chartFriendsAnalyzer.Series.Clear();
            chartPostsAnalyzer.Series.Clear();
            panelPosts.Controls.Clear();
            listBoxFriends.DataSource = null;
            panelPostDetails.Visible = false;
        }

        private void linkLabelFetchFriends_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (m_LoggedInUser != null)
            {
                listBoxFriends.DisplayMember = "Name";
                listBoxFriends.DataSource = m_LoggedInUser.Friends;
            }
        }

        private void listBoxFriends_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxFriends.SelectedItem != null)
            {
                pictureBoxFriend.ImageLocation = (listBoxFriends.SelectedItem as User).PictureNormalURL;
            }
        }

        private void buttonPlayGuessGame_Click(object sender, EventArgs e)
        {
            if (m_LoggedInUser != null)
            {
                FormTriviaGame form = new FormTriviaGame(m_LoggedInUser);
                form.ShowDialog();
            }
        }

        private void linkLabelFetchPosts_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (m_LoggedInUser != null)
            {
                ICollection<Post> posts = m_LoggedInUser.WallPosts;
                clearAndAddPosts(posts);
            }
        }

        private void clearAndAddPosts(ICollection<Post> i_Posts)
        {
            int yOffset = 0;
            FacebookPostCollection postCollection = new FacebookPostCollection(i_Posts);
            IFacebookIterator iterator = postCollection.CreateIterator();

            panelPosts.Controls.Clear();
            while (iterator.HasNext())
            {
                Post post = iterator.Next();
                BindablePost bindablePost = new BindablePost(post);
                ControlBindableFacebookPost postControl = new ControlBindableFacebookPost(bindablePost);

                postControl.EditButtonClicked += (s, e) =>
                {
                    panelPostDetails.Visible = true;
                    bindablePostBindingSource.DataSource = bindablePost;
                };
                
                postControl.Width = panelPosts.Width;
                postControl.Location = new Point(10, yOffset);
                yOffset += postControl.Height + 15;
                panelPosts.Controls.Add(postControl);
            }
        }

        private void buttonProfile_Click(object sender, EventArgs e)
        {
            if (m_LoggedInUser != null)
            {
                this.Visible = false;
                FormProfile form = FormProfile.Instance;
                form.Owner = this;
                form.LoggedInUser = m_LoggedInUser;
                form.ShowDialog();
            }
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            if (m_LoggedInUser != null && !string.IsNullOrEmpty(textBoxSearch.Text))
            {
                search(textBoxSearch.Text);
            }
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
            List<(string friendName, int likeCount)> topFriends;

            if (AnalyticsFacade == null)
            {
                MessageBox.Show("Please log in first to view analytics.", "Not Logged In", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            topFriends = AnalyticsFacade.GetTopFriendsAnalysis();
            chartFriendsAnalyzer.Series.Clear();
            chartFriendsAnalyzer.ChartAreas[0].AxisX.Title = "Like Count";
            chartFriendsAnalyzer.ChartAreas[0].AxisY.Title = "Friend Name";
            Series series = new Series("Top Liking Friends")
            {
                ChartType = SeriesChartType.Bar
            };

            foreach (var friend in topFriends)
            {
                series.Points.AddXY(friend.likeCount, friend.friendName);
            }

            chartFriendsAnalyzer.Series.Add(series);
        }

        private void displayPostAnalysisChart()
        {
            if (AnalyticsFacade != null)
            {
                Dictionary<string, (int totalLikes, int postCount)> timeOfDayStats;

                timeOfDayStats = AnalyticsFacade.GetPostingTimeAnalysis();
                chartPostsAnalyzer.Series.Clear();
                chartPostsAnalyzer.ChartAreas[0].AxisX.Title = "Time of Day";
                chartPostsAnalyzer.ChartAreas[0].AxisY.Title = "Counts";

                Series likesSeries = new Series("Total Likes")
                {
                    ChartType = SeriesChartType.Bar
                };

                Series postCountSeries = new Series("Post Count") 
                {
                    ChartType = SeriesChartType.Bar
                };

                foreach (var entry in timeOfDayStats)
                {
                    likesSeries.Points.AddXY(entry.Key, entry.Value.totalLikes);
                    postCountSeries.Points.AddXY(entry.Key, entry.Value.postCount);
                }

                chartPostsAnalyzer.Series.Add(likesSeries);
                chartPostsAnalyzer.Series.Add(postCountSeries);
            }
        }

        private void buttonShowStatistics_Click(object sender, EventArgs e)
        {
            loadStatistics();
        }

        private void loadStatistics()
        {
            if (m_LoggedInUser == null)
            {
                MessageBox.Show("Please log in first to view analytics.", "Not Logged In", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (AnalyticsFacade == null)
            {
                initializeAnalyticsFacade();
            }

            displayTopLikingFriendsChart();
            displayPostAnalysisChart();
            displayEngagementAnalysis();
            displayContentRecommendations();
            displayFriendshipRecommendations();
            displayBestTimeRecommendation();
        }

        private void displayEngagementAnalysis()
        {
            if (AnalyticsFacade != null)
            {
                var engagementData = AnalyticsFacade.GetEngagementAnalysis();
            }
        }

        private void displayContentRecommendations()
        {
            if (AnalyticsFacade != null)
            {
                var recommendations = AnalyticsFacade.GetContentRecommendations();
            }
        }

        private void displayFriendshipRecommendations()
        {
            if (AnalyticsFacade != null)
            {
                var recommendations = AnalyticsFacade.GetFriendshipRecommendations();
            }
        }

        private void displayBestTimeRecommendation()
        {
            var recommendation = AnalyticsFacade.GetBestPostingRecommendation();

            MessageBox.Show(
                $"The best time to post is: {recommendation.BestTimeToPost}\n" +
                $"Average likes per post: {recommendation.AverageLikesPerPost:F1}",
                "Posting Time Recommendation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void buttonHidePostDetails_Click(object sender, EventArgs e)
        {
            panelPostDetails.Visible = false;
        }

        private void analyzePostCategories()
        {
            if (AnalyticsFacade != null)
            {
                var categories = AnalyticsFacade.GetPostCategoriesAnalysis();
            }
        }

        private void analyzeOptimalPostLength()
        {
            if (AnalyticsFacade != null)
            {
                var lengthData = AnalyticsFacade.GetOptimalPostLengthAnalysis();
            }
        }

        private void analyzeMutualInteractions()
        {
            if (AnalyticsFacade != null)
            {
                var interactions = AnalyticsFacade.GetMutualInteractionsAnalysis();
            }
        }

        private void analyzeFriendGroups()
        {
            if (AnalyticsFacade != null)
            {
                var groups = AnalyticsFacade.GetFriendGroupsAnalysis();
            }
        }

        private void getAllRecommendations()
        {
            if (AnalyticsFacade != null)
            {
                var allRecommendations = AnalyticsFacade.GetAllRecommendations();
            }
        }
    }
}