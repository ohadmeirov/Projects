using System;
using System.Collections.Generic;
using FacebookWrapper.ObjectModel;

namespace FacebookWinFormsApp.Iterator
{
    public class FacebookPostCollection : IFacebookCollection
    {
        private List<Post> m_Posts;

        public FacebookPostCollection()
        {
            m_Posts = new List<Post>();
        }
        public FacebookPostCollection(ICollection<Post> i_Collection)
        {
            m_Posts = new List<Post>(i_Collection);
        }

        public IFacebookIterator CreateIterator()
        {
            return new FacebookPostIterator(this);
        }

        public void AddPost(Post i_Post)
        {
            m_Posts.Add(i_Post);
        }

        public void RemovePost(Post i_Post)
        {
            m_Posts.Remove(i_Post);
        }

        public int Count
        {
            get { return m_Posts.Count; }
        }

        public Post GetPost(int i_Index)
        {
            Post result = null;
            
            if (i_Index >= 0 && i_Index < m_Posts.Count)
            {
                result = m_Posts[i_Index];
            }
            
            return result;
        }
    }
} 