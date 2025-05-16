using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{
    public class FriendTriviaGameDirector : ITriviaGameDirector
    {
        private readonly ITriviaGameBuilder r_Builder;
        public int CorrectAnswerScore { get; set; } 
        public eDifficulty Difficulty { get; set; }
        public ICollection<string> QuestionTypes { get; set; }

        public int NumOfQuestions { get; set; }

        public FriendTriviaGameDirector(ITriviaGameBuilder i_Builder)
        {
            r_Builder = i_Builder;
            CorrectAnswerScore = 10;
            NumOfQuestions = 5;
            Difficulty = eDifficulty.Medium;
            initQuestionTypes();
        }

        private void initQuestionTypes()
        {
            Dictionary<string, IQuestionStrategy> strategies = r_Builder.GetQuestionsStrategies();

            QuestionTypes = strategies.Keys;
        }

        public ITriviaGame Construct()
        {
            int i = 0;
            ITriviaGameBuilder builder = r_Builder.CreateGame().SetCorrectAnswerScore(CorrectAnswerScore)
                .SetDifficulty(Difficulty);

            foreach (string questionType in QuestionTypes)
            {
                if (i >= NumOfQuestions)
                {
                    break;
                }

                builder.AddQuestion(questionType);
                i++;
            }

            return builder.GetGame();
        }
    }
}
