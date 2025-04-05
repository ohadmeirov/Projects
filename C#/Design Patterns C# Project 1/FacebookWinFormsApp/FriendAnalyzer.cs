using System;
using System.Collections.Generic;
using System.Linq;
using FacebookWrapper.ObjectModel;
namespace BasicFacebookFeatures
{
    public class FriendAnalyzer
    {
        public User LoggedInUser { get; set; }

        public FriendAnalyzer(User i_LoggedInUser)
        {
            LoggedInUser = i_LoggedInUser;
        }

        public List<(string FriendName, int LikeCount)> TryGetTopLikeingFriendsOrMockData()
        {
            List<(string FriendName, int LikeCount)> result;

            try
            {
                result = GetTopLikingFriends();
            }
            catch (Exception)
            {
                result = GetMockTopLikingFriends();
            }

            return result;
        }

        public List<(string FriendName, int LikeCount)> GetTopLikingFriends()
        {
            Dictionary<string, int> friendLikes = new Dictionary<string, int>();

            foreach (Post post in LoggedInUser.Posts)
            {
                if (post.LikedBy != null)
                {
                    foreach (User friend in post.LikedBy)
                    {
                        if (!friendLikes.ContainsKey(friend.Name))
                        {
                            friendLikes[friend.Name] = 0;
                        }

                        friendLikes[friend.Name]++;
                    }
                }
            }
            return friendLikes.OrderByDescending(entry => entry.Value)
                              .Take(10)
                              .Select(entry => (entry.Key, entry.Value))
                              .ToList();
        }

        public List<(string FriendName, int LikeCount)> GetMockTopLikingFriends()
        {
            return new List<(string FriendName, int LikeCount)>
            {
                ("Alice Johnson", 50),
                ("Bob Smith", 45),
                ("Charlie Brown", 40),
                ("David White", 35),
                ("Eve Black", 30),
                ("Frank Green", 25),
                ("Grace Harris", 20),
                ("Hannah Clark", 15),
                ("Ivy Lee", 10),
                ("Jack King", 5)
            };
        }
    }
}
