using System;

namespace FacebookWinFormsApp.Observer
{
    public interface IFacebookSubject
    {
        void Attach(IFacebookObserver i_Observer);
        void Detach(IFacebookObserver i_Observer);
        void NotifyObservers();
    }
} 