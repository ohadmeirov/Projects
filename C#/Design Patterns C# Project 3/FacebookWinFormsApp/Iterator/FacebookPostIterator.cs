using System;
using FacebookWrapper.ObjectModel;

namespace FacebookWinFormsApp.Iterator
{
    public class FacebookPostIterator : IFacebookIterator
    {
        private FacebookPostCollection m_Collection;
        private int m_CurrentIndex;

        public FacebookPostIterator(FacebookPostCollection i_Collection)
        {
            m_Collection = i_Collection;
            m_CurrentIndex = 0;
        }

        public bool HasNext()
        {
            return m_CurrentIndex < m_Collection.Count;
        }

        public Post Next()
        {
            Post result = null;
            
            if (HasNext())
            {
                result = m_Collection.GetPost(m_CurrentIndex);
                m_CurrentIndex++;
            }
            
            return result;
        }

        public void Reset()
        {
            m_CurrentIndex = 0;
        }
    }
} 