using System;
using System.Windows.Forms;

namespace FacebookWinFormsApp.Observer
{
    public class PostEditNotification : IFacebookObserver
    {
        private readonly string m_UserName;

        public PostEditNotification(string i_UserName)
        {
            m_UserName = i_UserName;
        }

        public void Update(string i_Message)
        {
            MessageBox.Show(
                $"Post by {m_UserName} has been updated!\nNew content: {i_Message}",
                "Post Update",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
} 