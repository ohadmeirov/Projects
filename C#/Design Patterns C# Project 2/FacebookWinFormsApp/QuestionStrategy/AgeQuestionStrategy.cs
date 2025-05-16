using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{
    public class AgeQuestionStrategy : IQuestionStrategy
    {
        private readonly Random r_Random = new Random();

        public Question CreateQuestion(User i_Friend, int i_NumOfPossibleAnswers)
        {
            int friendAge = DateTime.Now.Year - DateTime.Parse(i_Friend.Birthday).Year;
            List<object> wrongAnswers = getWrongAgeAnswers(i_NumOfPossibleAnswers);

            return new Question
            {
                Text = $"What the age of {i_Friend.Name}?",
                CorrectAnswer = friendAge,
                WrongAnswers = wrongAnswers
            };
        }

        private List<object> getWrongAgeAnswers(int i_NumOfPossibleAnswers)
        {
            List<object> wrongAnswers = new List<object>();
            for (int i = 0; i < i_NumOfPossibleAnswers - 1; i++)
            {
                int randomAge = r_Random.Next(18, 80);

                wrongAnswers.Add(randomAge);
            }

            return wrongAnswers;
        }
    }
}