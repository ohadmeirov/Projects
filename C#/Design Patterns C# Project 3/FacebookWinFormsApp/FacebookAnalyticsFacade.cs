using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures.BasicFacebookFeatures
{
    public class FacebookAnalyticsFacade
    {
        private PostAnalyzer m_PostAnalyzer;
        private FriendAnalyzer m_FriendAnalyzer;

        public FacebookAnalyticsFacade(User i_LoggedInUser)
        {
            m_PostAnalyzer = new PostAnalyzer(i_LoggedInUser);
            m_FriendAnalyzer = new FriendAnalyzer(i_LoggedInUser);
        }

        public Dictionary<string, (int TotalLikes, int PostCount)> GetPostingTimeAnalysis()
        {
            return m_PostAnalyzer.TryGetBestTimeToPostOrMockData();
        }

        public List<(string FriendName, int LikeCount)> GetTopFriendsAnalysis()
        {
            return m_FriendAnalyzer.TryGetTopLikeingFriendsOrMockData();
        }

        public (string BestTimeToPost, double AverageLikesPerPost) GetBestPostingRecommendation()
        {
            var timeData = m_PostAnalyzer.TryGetBestTimeToPostOrMockData();
            var bestTime = "";
            double bestAverage = 0;

            foreach (var time in timeData)
            {
                if (time.Value.PostCount > 0)
                {
                    double average = (double)time.Value.TotalLikes / time.Value.PostCount;
                    if (average > bestAverage)
                    {
                        bestAverage = average;
                        bestTime = time.Key;
                    }
                }
            }

            return (bestTime, bestAverage);
        }

        public Dictionary<string, List<Post>> GetPostCategoriesAnalysis()
        {
            return m_PostAnalyzer.GetPostsByCategory();
        }

        public Dictionary<string, (int Likes, int Comments)> GetEngagementAnalysis()
        {
            return m_PostAnalyzer.GetEngagementByTimeOfDay();
        }

        public List<string> GetContentRecommendations()
        {
            return m_PostAnalyzer.GetContentRecommendations();
        }

        public Dictionary<string, double> GetOptimalPostLengthAnalysis()
        {
            return m_PostAnalyzer.GetOptimalPostLength();
        }

        public Dictionary<string, (int ReceivedLikes, int GivenLikes)> GetMutualInteractionsAnalysis()
        {
            return m_FriendAnalyzer.TryGetMutualInteractionsOrMockData();
        }

        public Dictionary<string, HashSet<string>> GetFriendGroupsAnalysis()
        {
            return m_FriendAnalyzer.IdentifyFriendGroups();
        }

        public List<(string FriendName, string Recommendation)> GetFriendshipRecommendations()
        {
            return m_FriendAnalyzer.GetFriendshipRecommendations();
        }

        public (List<string> ContentRecommendations, List<(string FriendName, string Recommendation)> FriendshipRecommendations) GetAllRecommendations()
        {
            return (GetContentRecommendations(), GetFriendshipRecommendations());
        }
    }
}