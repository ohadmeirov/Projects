using System;
using FacebookWrapper.ObjectModel;

namespace FacebookWinFormsApp.Iterator
{
    public interface IFacebookCollection
    {
        IFacebookIterator CreateIterator();
        void AddPost(Post i_Post);
        void RemovePost(Post i_Post);
        int Count { get; }
    }
} 