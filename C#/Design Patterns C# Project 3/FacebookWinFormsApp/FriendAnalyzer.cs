using System;
using System.Collections.Generic;
using System.Linq;
using FacebookWrapper.ObjectModel;

namespace BasicFacebookFeatures
{
    public class FriendAnalyzer
    {
        public User m_LoggedInUser { get; set; }
        private Dictionary<string, HashSet<string>> m_FriendGroups;

        public FriendAnalyzer(User i_LoggedInUser)
        {
            m_LoggedInUser = i_LoggedInUser;
            m_FriendGroups = new Dictionary<string, HashSet<string>>();
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
            foreach (Post post in m_LoggedInUser.Posts)
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

        public Dictionary<string, (int ReceivedLikes, int GivenLikes)> GetMutualInteractions()
        {
            Dictionary<string, (int ReceivedLikes, int GivenLikes)> mutualInteractions = new Dictionary<string, (int, int)>();
            foreach (Post post in m_LoggedInUser.Posts)
            {
                if (post.LikedBy != null)
                {
                    foreach (User friend in post.LikedBy)
                    {
                        if (!mutualInteractions.ContainsKey(friend.Name))
                        {
                            mutualInteractions[friend.Name] = (0, 0);
                        }

                        var current = mutualInteractions[friend.Name];
                        mutualInteractions[friend.Name] = (current.ReceivedLikes + 1, current.GivenLikes);
                    }
                }
            }
            if (m_LoggedInUser.Friends != null)
            {
                foreach (User friend in m_LoggedInUser.Friends)
                {
                    if (friend.Posts != null)
                    {
                        foreach (Post friendPost in friend.Posts)
                        {
                            if (friendPost.LikedBy != null && friendPost.LikedBy.Any(user => user.Id == m_LoggedInUser.Id))
                            {
                                if (!mutualInteractions.ContainsKey(friend.Name))
                                {
                                    mutualInteractions[friend.Name] = (0, 0);
                                }
                                var current = mutualInteractions[friend.Name];
                                mutualInteractions[friend.Name] = (current.ReceivedLikes, current.GivenLikes + 1);
                            }
                        }
                    }
                }
            }

            return mutualInteractions;
        }

        public Dictionary<string, (int ReceivedLikes, int GivenLikes)> TryGetMutualInteractionsOrMockData()
        {
            Dictionary<string, (int ReceivedLikes, int GivenLikes)> result;
            try
            {
                result = GetMutualInteractions();
            }
            catch (Exception)
            {
                result = getMockMutualInteractions();
            }

            return result;
        }

        private Dictionary<string, (int ReceivedLikes, int GivenLikes)> getMockMutualInteractions()
        {
            return new Dictionary<string, (int ReceivedLikes, int GivenLikes)>
        {
            { "Alice Johnson", (50, 40) },
            { "Bob Smith", (45, 30) },
            { "Charlie Brown", (40, 45) },
            { "David White", (35, 20) },
            { "Eve Black", (30, 25) },
            { "Frank Green", (25, 10) },
            { "Grace Harris", (20, 30) },
            { "Hannah Clark", (15, 5) },
            { "Ivy Lee", (10, 0) },
            { "Jack King", (5, 15) }
        };
        }

        public Dictionary<string, HashSet<string>> IdentifyFriendGroups()
        {
            Dictionary<string, HashSet<string>> friendGroups = new Dictionary<string, HashSet<string>>();
            try
            {
                if (m_LoggedInUser.Friends != null)
                {
                    foreach (User friend1 in m_LoggedInUser.Friends)
                    {
                        foreach (User friend2 in m_LoggedInUser.Friends)
                        {
                            if (friend1.Id != friend2.Id && areConnected(friend1, friend2))
                            {
                                string groupName = $"Group of {friend1.Name}";

                                if (!friendGroups.ContainsKey(groupName))
                                {
                                    friendGroups[groupName] = new HashSet<string>();
                                }
                                friendGroups[groupName].Add(friend1.Name);
                                friendGroups[groupName].Add(friend2.Name);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                friendGroups = getMockFriendGroups();
            }
            m_FriendGroups = friendGroups;

            return friendGroups;
        }

        private bool areConnected(User i_Friend1, User i_Friend2)
        {
            bool connected = false;
            try
            {
                if (i_Friend1.Posts != null)
                {
                    foreach (Post post in i_Friend1.Posts)
                    {
                        if (post.LikedBy != null && post.LikedBy.Any(u => u.Id == i_Friend2.Id))
                        {
                            connected = true;
                            break;
                        }
                        if (post.Comments != null)
                        {
                            foreach (Comment comment in post.Comments)
                            {
                                if (comment.From.Id == i_Friend2.Id)
                                {
                                    connected = true;
                                    break;
                                }
                            }

                            if (connected) break;
                        }
                    }
                }
            }
            catch
            {
                connected = false;
            }

            return connected;
        }

        private Dictionary<string, HashSet<string>> getMockFriendGroups()
        {
            Dictionary<string, HashSet<string>> mockGroups = new Dictionary<string, HashSet<string>>();
            mockGroups["Work Friends Group"] = new HashSet<string> { "Alice Johnson", "Bob Smith", "Charlie Brown" };
            mockGroups["School Friends Group"] = new HashSet<string> { "David White", "Eve Black", "Frank Green" };
            mockGroups["Childhood Friends Group"] = new HashSet<string> { "Grace Harris", "Hannah Clark" };
            mockGroups["Family Group"] = new HashSet<string> { "Ivy Lee", "Jack King" };
            return mockGroups;
        }

        public List<(string FriendName, string Recommendation)> GetFriendshipRecommendations()
        {
            List<(string FriendName, string Recommendation)> recommendations = new List<(string, string)>();
            var mutualInteractions = TryGetMutualInteractionsOrMockData();
            var imbalancedFriends = mutualInteractions
                .Select(f => new
                {
                    Name = f.Key,
                    Ratio = f.Value.ReceivedLikes > 0 ? (double)f.Value.GivenLikes / f.Value.ReceivedLikes : 0,
                    Received = f.Value.ReceivedLikes,
                    Given = f.Value.GivenLikes
                })
                .OrderBy(f => f.Ratio)
                .Take(5)
                .ToList();
            foreach (var friend in imbalancedFriends)
            {
                if (friend.Ratio < 0.5)
                {
                    recommendations.Add((friend.Name, $"Gives you a lot of likes ({friend.Received}) but doesn't receive much in return ({friend.Given}). You might want to engage more with their posts."));
                }
                else if (friend.Received == 0 && friend.Given > 0)
                {
                    recommendations.Add((friend.Name, $"You are giving likes ({friend.Given}) but not receiving any in return. Maybe check if this friend is active on Facebook."));
                }
                else if (friend.Received > 0 && friend.Given == 0)
                {
                    recommendations.Add((friend.Name, $"Receiving likes from you ({friend.Received}) but not engaging with your posts. Consider if your content interests them."));
                }
            }
            if (m_FriendGroups.Count == 0)
            {
                IdentifyFriendGroups();
            }
            HashSet<string> allGroupMembers = new HashSet<string>();
            foreach (var group in m_FriendGroups)
            {
                foreach (var member in group.Value)
                {
                    allGroupMembers.Add(member);
                }
            }
            var allFriends = mutualInteractions.Keys.ToList();
            var lonelyFriends = allFriends.Where(f => !allGroupMembers.Contains(f)).Take(3).ToList();
            foreach (var friend in lonelyFriends)
            {
                recommendations.Add((friend, "Not found in any connected friend groups. You might want to introduce them to other friends."));
            }

            return recommendations;
        }
    }
}