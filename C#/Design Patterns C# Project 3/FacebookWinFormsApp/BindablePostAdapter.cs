using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{
    public class BindablePost : INotifyPropertyChanged
    {
        private readonly Post m_Post;
        private string m_Message;
        private string m_PictureNormalURL;
        private DateTime? m_CreatedTime;

        public BindablePost(Post i_Post)
        {
            m_Post = i_Post;
            m_Message = i_Post.Message;
            m_PictureNormalURL = i_Post.From.PictureNormalURL;
            m_CreatedTime = i_Post.CreatedTime;
        }

        public string UserName
        {
            get => m_Post.From.Name;
            set
            {
                if (m_Post.From.Name != value)
                {
                    m_Post.From.Name = value;
                    OnPropertyChanged(nameof(UserName));
                }
            }
        }

        public string Message
        {
            get => m_Message;
            set
            {
                if (m_Message != value)
                {
                    m_Message = value;
                    OnPropertyChanged(nameof(Message));
                }
            }
        }

        public string PictureUrl
        {
            get => m_PictureNormalURL;
            set
            {
                if (m_PictureNormalURL != value)
                {
                    m_PictureNormalURL = value;
                    OnPropertyChanged(nameof(PictureUrl));
                }
            }
        }

        public DateTime? CreatedTime
        {
            get => m_CreatedTime;
            set
            {
                if (m_CreatedTime != value)
                {
                    m_CreatedTime = value;
                    OnPropertyChanged(nameof(CreatedTime));
                }
            }
        }

        public void Like() => m_Post.Like();
        public void Comment(string i_Text) => m_Post.Comment(i_Text);

        public Post InnerPost => m_Post;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string i_PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(i_PropertyName));
        }
    }
}
