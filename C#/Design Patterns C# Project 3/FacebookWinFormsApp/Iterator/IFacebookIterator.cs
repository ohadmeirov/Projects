using System;
using FacebookWrapper.ObjectModel;

namespace FacebookWinFormsApp.Iterator
{
    public interface IFacebookIterator
    {
        bool HasNext();
        Post Next();
        void Reset();
    }
} 