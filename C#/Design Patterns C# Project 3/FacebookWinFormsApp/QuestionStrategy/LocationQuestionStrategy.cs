using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{
    public class LocationQuestionStrategy : IQuestionStrategy
    {
        public Question CreateQuestion(User i_Friend, int i_NumOfPossibleAnswers)
        {
            string friendLocation = i_Friend.Location?.ToString();
            List<object> wrongAnswers = getWrongLocationAnswers(i_NumOfPossibleAnswers);
            const string nullValue = "Unknown";

            return new Question
            {
                Text = $"Where was { i_Friend.Name } is located.",
                CorrectAnswer = friendLocation ?? nullValue,
                WrongAnswers = wrongAnswers
            };
        }

        private List<object> getWrongLocationAnswers(int i_NumOfPossibleAnswers)
        {
            List<object> allCities = new List<object>
            {
                "Holon", "Tel Aviv", "Ashdod", "Haifa", "Jerusalem", "Eilat",
                "Beersheba", "Netanya", "Ramat Gan", "Bat Yam"
            };
            List<object> wrongAnswers = allCities.Take(i_NumOfPossibleAnswers - 1).ToList();

            return wrongAnswers;
        }
    }
}