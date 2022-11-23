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
        /// <summary>
        /// Returns a list of the default security questions from our database.
        /// </summary>
        /// <returns>A collection of SecurityQuestion</returns>
        public static List<SecurityQuestion> GetDefaultSecurityQuestions()
        {
            return DataManager.GetDefaultSecurityQuestions();
        }
    }
}
