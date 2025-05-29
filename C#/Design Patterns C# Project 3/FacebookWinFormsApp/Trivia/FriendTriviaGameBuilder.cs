using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{
    public class FriendTriviaGameBuilder : IFriendTriviaGameBuilder
    {
        private int m_NumOfPossibleAnswers;
        private Random m_Random;
        private User m_LoggedInUser;
        private User m_SelectedFriend;
        private readonly ICollection<User> r_Friends;
        private ITriviaGame m_Game;
        public Dictionary<string, IQuestionStrategy> QuestionStrategies { get; private set; }

        public FriendTriviaGameBuilder(User i_LoggedInUser)
        {
            m_LoggedInUser = i_LoggedInUser;
            r_Friends = m_LoggedInUser.Friends;
            m_Random = new Random();
            initStrategies();
        }

        private void initStrategies()
        {
            QuestionStrategies = new Dictionary<string, IQuestionStrategy>
            {
                { "Birthday", new BirthdayQuestionStrategy() },
                { "Location", new LocationQuestionStrategy() },
                { "Age", new AgeQuestionStrategy() },
                { "Relationship", new RelationshipQuestionStrategy() },
                { "Gender", new GenderQuestionStrategy() },
            };
        }

        public ITriviaGameBuilder CreateGame()
        {
            m_Game = new TriviaGame();
            selectRandomFriend();

            return this;
        }

        public ITriviaGameBuilder AddQuestion(object i_Context)
        {
            ITriviaGameBuilder result = null;

            if (i_Context is string context && QuestionStrategies.TryGetValue(context, out IQuestionStrategy strategy))
            {
                Question question = strategy.CreateQuestion(m_SelectedFriend, m_NumOfPossibleAnswers);
                m_Game.AddQuestion(question);
                result = this;
            }

            return result;
        }


        public ITriviaGameBuilder RemoveQuestion(Question i_Question)
        {
            m_Game.RemoveQuestion(i_Question);

            return this;
        }

        public ITriviaGameBuilder SetCorrectAnswerScore(int i_Score)
        {
            m_Game.SetPointsforCorrectAnswer(i_Score);

            return this;
        }

        public ITriviaGameBuilder SetDifficulty(eDifficulty i_Difficulty)
        {
            switch (i_Difficulty)
            {
                case eDifficulty.Easy:
                    m_NumOfPossibleAnswers = 4;
                    break;
                case eDifficulty.Medium:
                    m_NumOfPossibleAnswers = 6;
                    break;
                case eDifficulty.Hard:
                    m_NumOfPossibleAnswers = 8;
                    break;
            }

            return this;
        }

        public ITriviaGame GetGame()
        {
            return m_Game;
        }

        public User GetSelectedFriend()
        {
            return m_SelectedFriend;
        }

        public Dictionary<string, IQuestionStrategy> GetQuestionsStrategies()
        {
            return QuestionStrategies;
        }

        private User selectRandomFriend()
        {
            int randomIndex = m_Random.Next(0, r_Friends.Count);

            if (r_Friends.Count > 0)
            {
                m_SelectedFriend = r_Friends.ElementAt(randomIndex);
            }
            
            return m_SelectedFriend;
        }
    }
}