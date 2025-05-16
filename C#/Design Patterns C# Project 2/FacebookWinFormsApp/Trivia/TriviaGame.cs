using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{
    class TriviaGame : ITriviaGame
    {
        private readonly List<Question> r_Questions = new List<Question>();
        private readonly List<Question> r_CheckedQuesitons = new List<Question>();
        private int m_CurrentQuestionIndex = 0;
        private int m_Score = 0;
        private int m_PointsForCorrectAnswer = 1;

        public TriviaGame()
        {
            Start();
        }
        
        public void RemoveQuestion(Question i_Question)
        {
            r_Questions.Remove(i_Question);
        }

        public void AddQuestion(Question i_Question)
        {
            r_Questions.Add(i_Question);
        }

        public bool CheckAnswer(Question i_Question, object i_Answer)
        {
            Question question = r_Questions.Find(q => q == i_Question);
            bool isCheckedBefore = r_CheckedQuesitons.Contains(question);
            bool result = false;

            if (!isCheckedBefore && question?.CorrectAnswer == i_Answer)
            {
                result = true;
                r_CheckedQuesitons.Add(question);
                addScore();
            }

            return result;
        }

        public void Reset()
        {
            r_Questions.Clear();
            m_Score = 0;
        }

        public void Start()
        {
            m_CurrentQuestionIndex = 0;
        }

        public Question GetQuestion()
        {
            Question current = null;

            if (m_CurrentQuestionIndex < r_Questions.Count)
            {
                current = r_Questions[m_CurrentQuestionIndex++];
            }

            return current;
        }

        public ICollection<Question> GetQuestions()
        {
            return r_Questions;
        }

        public int GetScore()
        {
            return m_Score;
        }

        public void SetPointsforCorrectAnswer(int i_Score)
        {
            m_PointsForCorrectAnswer = i_Score;
        }

        private void addScore()
        {
            m_Score += m_PointsForCorrectAnswer;
        }
    }
}