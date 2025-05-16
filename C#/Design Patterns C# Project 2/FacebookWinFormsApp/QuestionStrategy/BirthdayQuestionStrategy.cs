using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{
    public class BirthdayQuestionStrategy : IQuestionStrategy
    {
        public Question CreateQuestion(User i_Friend, int i_NumOfPossibleAnswers)
        {
            DateTime friendBirthday = DateTime.Parse(i_Friend.Birthday);
            List<object> wrongAnswers = getWrongBirthdayAnswers(i_Friend, i_NumOfPossibleAnswers);

            return new Question
            {
                Text = $"When was {i_Friend.Name} born?",
                CorrectAnswer = friendBirthday,
                WrongAnswers = wrongAnswers
            };
        }

        private List<object> getWrongBirthdayAnswers(User i_Friend, int i_NumOfPossibleAnswers)
        {
            DateTime friendBirthday = DateTime.Parse(i_Friend.Birthday);
            DateTime wrongAnswer;
            int year = friendBirthday.Year;
            List<object> wrongAnswers = new List<object>();
            Random random = new Random();
            for (int i = 0; i < i_NumOfPossibleAnswers - 1; i++)
            {
                int month = random.Next(1, 13);
                int day = random.Next(1, DateTime.DaysInMonth(year, month) + 1);

                wrongAnswer = new DateTime(year, month, day);
                wrongAnswers.Add(wrongAnswer);
            }

            return wrongAnswers;
        }
    }
}