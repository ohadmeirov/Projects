using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
namespace BasicFacebookFeatures
{
    public class PostAnalyzer
    {
        public User LoggedInUser { get; set; }

        public PostAnalyzer(User i_LoggedInUser)
        {
            LoggedInUser = i_LoggedInUser;
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

            foreach (Post post in LoggedInUser.Posts)
            {
                if (post.CreatedTime.HasValue)
                {
                    string timeOfDay = GetTimeOfDayCategory(post.CreatedTime.Value);
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

        private string GetTimeOfDayCategory(DateTime i_Datetime)
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
    }
}