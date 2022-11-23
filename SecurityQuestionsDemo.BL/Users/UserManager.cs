using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecurityQuestionsDemo.DAL;
using SecurityQuestionsDemo.Entity;

namespace SecurityQuestionsDemo.BL.Users
{
    public static class UserManager
    {
        /// <summary>
        /// Retrieves a User object from the database based on name.
        /// </summary>
        /// <param name="name">The name of the user in the system.</param>
        /// <returns>A User object if it exists, else null.</returns>
        public static User GetUserByName(string name)
        {
            var user = DataManager.GetUserByName(name);

            if (!string.IsNullOrEmpty(user.Name))
            {
                //User exists in the system.
                return user;
            }
            else
            {
                //User does not exist in the system.
                return null;
            }
        }

        /// <summary>
        /// Method for adding a new User.
        /// </summary>
        /// <param name="name">The name of the User.</param>
        /// <returns>A User object.</returns>
        public static User AddNewUser(string name)
        {
            //Add the new user.
            DataManager.InsertNewUser(new User { Id = 0, Name = name, SecurityQuestions = new List<UserSecurityQuestion>() });

            return DataManager.GetUserByName(name);
        }

        /// <summary>
        /// Method for saving a User's security questions/answers.
        /// </summary>
        /// <param name="user">The User object containing the security questions.</param>
        /// <returns>A User object.</returns>
        public static User SaveUserSecurityQuestions(User user)
        {
            if (user.SecurityQuestions.Count == 3)
            {
                //Delete the user's existing security questions/answers.
                DataManager.DeleteUserSecurityQuestions(user.Id);

                //Add the user's security questions/answers.
                DataManager.InsertUserSecurityQuestions(user);

                //Get the updated user object.
                user = DataManager.GetUserByName(user.Name);
            }
            return user;
        }
    }
}
