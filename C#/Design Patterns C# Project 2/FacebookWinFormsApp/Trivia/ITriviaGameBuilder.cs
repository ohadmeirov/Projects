using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{
    public interface ITriviaGameBuilder
    {
        ITriviaGameBuilder CreateGame();
        ITriviaGameBuilder AddQuestion(object i_Context);
        ITriviaGameBuilder SetCorrectAnswerScore(int i_Score);
        ITriviaGameBuilder SetDifficulty(eDifficulty i_Difficulty);
        ITriviaGame GetGame();
        Dictionary<string, IQuestionStrategy> GetQuestionsStrategies();
    }
}
