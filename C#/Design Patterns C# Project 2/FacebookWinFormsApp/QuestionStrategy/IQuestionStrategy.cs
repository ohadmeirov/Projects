using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{
    public interface IQuestionStrategy
    {
        Question CreateQuestion(User i_Friend, int i_NumOfPossibleAnswers);
    }
}
