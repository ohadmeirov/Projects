using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{
    public class GenderQuestionStrategy : IQuestionStrategy
    {
        public Question CreateQuestion(User i_Friend, int i_NumOfPossibleAnswers)
        {
            User.eGender? friendGender = i_Friend.Gender;
            List<object> wrongAnswers = getWrongGenderAnswers(i_Friend, i_NumOfPossibleAnswers);

            return new Question
            {
                Text = $"What the gender of {i_Friend.Name}?",
                CorrectAnswer = friendGender,
                WrongAnswers = wrongAnswers
            };
        }

        private List<object> getWrongGenderAnswers(User i_Friend, int i_NumOfPossibleAnswers)
        {
            List<object> wrongAnswers = new List<object>();

            foreach (User.eGender gender in Enum.GetValues(typeof(User.eGender)))
            {
                if (gender != i_Friend.Gender)
                {
                    wrongAnswers.Add(gender);

                    if (wrongAnswers.Count == i_NumOfPossibleAnswers - 1)
                        break;
                }
            }

            return wrongAnswers;
        }
    }
}