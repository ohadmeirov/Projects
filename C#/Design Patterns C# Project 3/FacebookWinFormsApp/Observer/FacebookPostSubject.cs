using System;
using System.Collections.Generic;

namespace FacebookWinFormsApp.Observer
{
    public class FacebookPostSubject : IFacebookSubject
    {
        private List<IFacebookObserver> m_Observers = new List<IFacebookObserver>();
        private string m_PostMessage;

        public string PostMessage
        {
            get { return m_PostMessage; }
            set
            {
                m_PostMessage = value;
                NotifyObservers();
            }
        }

        public void Attach(IFacebookObserver i_Observer)
        {
            m_Observers.Add(i_Observer);
        }

        public void Detach(IFacebookObserver i_Observer)
        {
            m_Observers.Remove(i_Observer);
        }

        public void NotifyObservers()
        {
            foreach (IFacebookObserver observer in m_Observers)
            {
                observer.Update(m_PostMessage);
            }
        }
    }
}