using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BasicFacebookFeatures
{
    public class PostAnalyzer
    {
        public User m_LoggedInUser { get; set; }
        private Dictionary<string, List<string>> m_CategoryKeywords;

        public PostAnalyzer(User i_LoggedInUser)
        {
            m_LoggedInUser = i_LoggedInUser;
            initializeCategoryKeywords();
        }

        private void initializeCategoryKeywords()
        {
            m_CategoryKeywords = new Dictionary<string, List<string>>
        {
            { "Personal", new List<string> { "i", "me", "my", "mine", "feeling", "today", "went", "happy", "sad" } },
            { "Food", new List<string> { "food", "eat", "dinner", "lunch", "breakfast", "restaurant", "delicious", "cooking" } },
            { "Travel", new List<string> { "travel", "trip", "vacation", "flight", "hotel", "beach", "visit", "explore" } },
            { "Work", new List<string> { "work", "job", "office", "project", "meeting", "boss", "colleague", "career" } },
            { "Family", new List<string> { "family", "mom", "dad", "sister", "brother", "son", "daughter", "parents" } }
        };
        }

        public Dictionary<string, (int TotalLikes, int PostCount)> TryGetBestTimeToPostOrMockData()
        {
            Dictionary<string, (int TotalLikes, int PostCount)> result;

            try
            {
                result = GetBestTimeToPost();
            }
            catch (Exception)
            {
                result = GetBestTimeToPostMockData();
            }

            return result;
        }

        public Dictionary<string, (int TotalLikes, int PostCount)> GetBestTimeToPost()
        {
            Dictionary<string, (int TotalLikes, int PostCount)> likesPerTimeOfDay = new Dictionary<string, (int, int)>();

            foreach (Post post in m_LoggedInUser.Posts)
            {
                if (post.CreatedTime.HasValue)
                {
                    string timeOfDay = getTimeOfDayCategory(post.CreatedTime.Value);
                    int likes = post.LikedBy?.Count ?? 0;

                    if (!likesPerTimeOfDay.ContainsKey(timeOfDay))
                    {
                        likesPerTimeOfDay[timeOfDay] = (0, 0);
                    }

                    var current = likesPerTimeOfDay[timeOfDay];
                    likesPerTimeOfDay[timeOfDay] = (current.TotalLikes + likes, current.PostCount + 1);
                }
            }

            return likesPerTimeOfDay;
        }

        public Dictionary<string, (int TotalLikes, int PostCount)> GetBestTimeToPostMockData()
        {
            Dictionary<string, (int TotalLikes, int PostCount)> mockData = new Dictionary<string, (int, int)>
        {
            { "Morning (5:00-12:00)", (150, 5) },
            { "Afternoon (12:00-17:00)", (100, 3) },
            { "Evening (17:00-21:00)", (200, 6) },
            { "Night (21:00-5:00)", (50, 2) }
        };

            return mockData;
        }

        private string getTimeOfDayCategory(DateTime i_Datetime)
        {
            int hour = i_Datetime.Hour;
            string result;

            if (hour >= 5 && hour < 12)
                result = "Morning (5:00-12:00)";
            else if (hour >= 12 && hour < 17)
                result = "Afternoon (12:00-17:00)";
            else if (hour >= 17 && hour < 21)
                result = "Evening (17:00-21:00)";
            else
                result = "Night (21:00-5:00)";

            return result;
        }

        public Dictionary<string, List<Post>> GetPostsByCategory()
        {
            Dictionary<string, List<Post>> postsByCategory = new Dictionary<string, List<Post>>();
            foreach (string category in m_CategoryKeywords.Keys)
            {
                postsByCategory[category] = new List<Post>();
            }
            postsByCategory["Other"] = new List<Post>();
            foreach (Post post in m_LoggedInUser.Posts)
            {
                if (string.IsNullOrEmpty(post.Message))
                {
                    continue;
                }
                bool categorized = false;
                string message = post.Message.ToLower();
                foreach (var category in m_CategoryKeywords)
                {
                    foreach (string keyword in category.Value)
                    {
                        if (Regex.IsMatch(message, $@"\b{keyword}\b"))
                        {
                            postsByCategory[category.Key].Add(post);
                            categorized = true;
                            break;
                        }
                    }
                    if (categorized)
                    {
                        break;
                    }
                }
                if (!categorized)
                {
                    postsByCategory["Other"].Add(post);
                }
            }

            return postsByCategory;
        }

        public Dictionary<string, (int Likes, int Comments)> GetEngagementByTimeOfDay()
        {
            Dictionary<string, (int Likes, int Comments)> engagementByTime = new Dictionary<string, (int, int)>();

            foreach (Post post in m_LoggedInUser.Posts)
            {
                if (post.CreatedTime.HasValue)
                {
                    string timeOfDay = getTimeOfDayCategory(post.CreatedTime.Value);
                    int likes = post.LikedBy?.Count ?? 0;
                    int comments = post.Comments?.Count ?? 0;
                    if (!engagementByTime.ContainsKey(timeOfDay))
                    {
                        engagementByTime[timeOfDay] = (0, 0);
                    }
                    var current = engagementByTime[timeOfDay];
                    engagementByTime[timeOfDay] = (current.Likes + likes, current.Comments + comments);
                }
            }

            return engagementByTime;
        }

        public Dictionary<string, double> GetOptimalPostLength()
        {
            Dictionary<string, List<(int Length, int Engagement)>> lengthData = new Dictionary<string, List<(int, int)>>
        {
            { "Short (< 100 chars)", new List<(int, int)>() },
            { "Medium (100-300 chars)", new List<(int, int)>() },
            { "Long (> 300 chars)", new List<(int, int)>() }
        };
            foreach (Post post in m_LoggedInUser.Posts)
            {
                if (string.IsNullOrEmpty(post.Message))
                {
                    continue;
                }
                int length = post.Message.Length;
                int engagement = (post.LikedBy?.Count ?? 0) + (post.Comments?.Count ?? 0);
                string category = getLengthCategory(length);
                lengthData[category].Add((length, engagement));
            }
            Dictionary<string, double> averageEngagement = new Dictionary<string, double>();
            foreach (var category in lengthData)
            {
                if (category.Value.Count > 0)
                {
                    double average = category.Value.Sum(item => item.Engagement) / (double)category.Value.Count;
                    averageEngagement[category.Key] = average;
                }
                else
                {
                    averageEngagement[category.Key] = 0;
                }
            }

            return averageEngagement;
        }

        private string getLengthCategory(int i_Length)
        {
            if (i_Length < 100)
                return "Short (< 100 chars)";
            else if (i_Length <= 300)
                return "Medium (100-300 chars)";
            else
                return "Long (> 300 chars)";
        }

        public List<string> GetContentRecommendations()
        {
            List<string> recommendations = new List<string>();
            var postsByCategory = GetPostsByCategory();
            string bestCategory = "";
            double bestAvgEngagement = 0;
            foreach (var category in postsByCategory)
            {
                if (category.Value.Count == 0) continue;

                double totalEngagement = category.Value.Sum(post =>
                    (post.LikedBy?.Count ?? 0) + (post.Comments?.Count ?? 0));
                double avgEngagement = totalEngagement / category.Value.Count;

                if (avgEngagement > bestAvgEngagement)
                {
                    bestAvgEngagement = avgEngagement;
                    bestCategory = category.Key;
                }
            }
            if (!string.IsNullOrEmpty(bestCategory))
            {
                recommendations.Add($"Your posts in the '{bestCategory}' category receive the best engagement. Consider posting more content about this topic.");
            }
            var lengthData = GetOptimalPostLength();
            var bestLength = lengthData.OrderByDescending(x => x.Value).FirstOrDefault();
            recommendations.Add($"Posts with {bestLength.Key} tend to receive an average of {bestLength.Value:F1} engagements - this is your optimal post length.");
            var timeData = GetBestTimeToPost();
            var bestTime = "";
            double bestTimeAvg = 0;
            foreach (var time in timeData)
            {
                if (time.Value.PostCount > 0)
                {
                    double avg = time.Value.TotalLikes / (double)time.Value.PostCount;
                    if (avg > bestTimeAvg)
                    {
                        bestTimeAvg = avg;
                        bestTime = time.Key;
                    }
                }
            }
            if (!string.IsNullOrEmpty(bestTime))
            {
                recommendations.Add($"Posting during {bestTime} brings you the highest number of likes ({bestTimeAvg:F1} likes per post on average).");
            }

            return recommendations;
        }
    }
}