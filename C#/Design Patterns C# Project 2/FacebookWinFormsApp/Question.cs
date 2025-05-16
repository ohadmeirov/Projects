using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{
    public class Question
    {
        private static readonly Random sr_Random = new Random();

        public string Text { get; set; }
        public ICollection<object> WrongAnswers { get; set; }
        public object CorrectAnswer { get; set; }

        public ICollection<object> GetShuffledAnswers()
        {
            List<object> allAnswers = new List<object>(WrongAnswers);

            allAnswers.Add(CorrectAnswer);

            return allAnswers.OrderBy(x => sr_Random.Next()).ToList();
        }
    }
}
