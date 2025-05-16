using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{
    public interface ITriviaGame
    {
        void AddQuestion(Question i_Question);
        void RemoveQuestion(Question i_Question);
        bool CheckAnswer(Question i_Question, object i_Answer);
        void Start();
        void Reset();
        Question GetQuestion();
        ICollection<Question> GetQuestions();
        int GetScore();
        void SetPointsforCorrectAnswer(int i_Score);
    }
}
