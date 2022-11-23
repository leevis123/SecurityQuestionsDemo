using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecurityQuestionsDemo.DAL;
using SecurityQuestionsDemo.Entity;

namespace SecurityQuestionsDemo.BL.SecurityQuestions
{
    public static class SecurityQuestionsManager
    {
        public static List<SecurityQuestion> GetDefaultSecurityQuestions()
        {
            return DataManager.GetDefaultSecurityQuestions();
        }
    }
}
