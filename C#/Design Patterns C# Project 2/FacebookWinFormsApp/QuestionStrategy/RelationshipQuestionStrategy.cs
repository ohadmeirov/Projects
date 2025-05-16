using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{
    public class RelationshipQuestionStrategy : IQuestionStrategy
    {
        public Question CreateQuestion(User i_Friend, int i_NumOfPossibleAnswers)
        {
            User.eRelationshipStatus? friendRelationship = i_Friend.RelationshipStatus;
            List<object> wrongAnswers = getWrongRelationshipAnswers(i_Friend, i_NumOfPossibleAnswers);

            return new Question
            {
                Text = $"What is {i_Friend.Name} relationship status?",
                CorrectAnswer = friendRelationship,
                WrongAnswers = wrongAnswers
            };
        }

        private List<object> getWrongRelationshipAnswers(User i_Friend, int i_NumOfPossibleAnswers)
        {
            List<object> wrongAnswers = new List<object>();
            foreach (User.eRelationshipStatus status in Enum.GetValues(typeof(User.eRelationshipStatus)))
            {
                if (status != i_Friend.RelationshipStatus)
                {
                    wrongAnswers.Add(status);

                    if (wrongAnswers.Count == i_NumOfPossibleAnswers - 1)
                        break;
                }
            }

            return wrongAnswers;
        }
    }
}