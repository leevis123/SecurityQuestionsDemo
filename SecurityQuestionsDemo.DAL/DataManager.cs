using SecurityQuestionsDemo.Entity;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SecurityQuestionsDemo.DAL
{
    public static class DataManager
    {
        //Usually would not set the connection string within this class. Instead would use App.Config, other config files, or other methods
        //to keep connection string secure and easy to manage. 
        private static string dataDirectory = $"{Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName}\\SecurityQuestionsDemo.DAL\\";
        private static string connectionString = $"DataSource={dataDirectory}SecurityQuestionsDemoDB.db;";


        /// <summary>
        /// Adds a new user to the database.
        /// </summary>
        /// <param name="user">The user object to be added.</param>
        public static void InsertNewUser(User user)
        {
            string insertSql = @"INSERT INTO User (Name) VALUES ('{0}')";

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = string.Format(insertSql, user.Name);
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }

        /// <summary>
        /// Retrieves a user object and their stored security questions. 
        /// </summary>
        /// <param name="name">The name of the user.</param>
        /// <returns></returns>
        public static User GetUserByName(string name)
        {
            User user = new User();
            StringBuilder thisSql = new StringBuilder();

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                thisSql.Clear().AppendLine("SELECT u.Id, u.Name, usq.Id AS UserSecurityQuestionId,  usq.SecurityQuestionId, sq.Question, usq.Answer")
                    .AppendLine("FROM User u")
                    .AppendLine("LEFT JOIN UserSecurityQuestion usq ON usq.UserId = u.Id")
                    .AppendLine("LEFT JOIN SecurityQuestion sq ON sq.Id = usq.SecurityQuestionId")
                    .AppendLine($"WHERE lower(u.Name) = '{name.ToLower()}'");

                using (SQLiteCommand cmd = new SQLiteCommand(thisSql.ToString(), conn))
                {
                    SQLiteDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        if (string.IsNullOrEmpty(user.Name))
                        {
                            user.Id = Convert.ToInt32(dataReader["Id"]);
                            user.Name = Convert.ToString(dataReader["Name"]);

                            user.SecurityQuestions = new List<UserSecurityQuestion>();
                        }

                        if (dataReader["SecurityQuestionId"] != DBNull.Value)
                        {
                            SecurityQuestion securityQuestion = new SecurityQuestion();
                            securityQuestion.Id = Convert.ToInt32(dataReader["SecurityQuestionId"]);
                            securityQuestion.Question = Convert.ToString(dataReader["Question"]);

                            UserSecurityQuestion userSecurityQuestion = new UserSecurityQuestion();
                            userSecurityQuestion.Id = Convert.ToInt32(dataReader["UserSecurityQuestionId"]);
                            userSecurityQuestion.UserId = user.Id;
                            userSecurityQuestion.SecurityQuestion = securityQuestion;
                            userSecurityQuestion.Answer = Convert.ToString(dataReader["Answer"]);

                            user.SecurityQuestions.Add(userSecurityQuestion);
                        }
                    }
                }
                conn.Close();
            }
            return user;
        }

        public static List<UserSecurityQuestion> GetUserSecurityQuestions(User user)
        {
            List<UserSecurityQuestion> userSecurityQuestions = new List<UserSecurityQuestion>();
            StringBuilder thisSql = new StringBuilder();

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                thisSql.Clear().AppendLine("SELECT *")
                    .AppendLine("FROM SecurityQuestion");

                using (SQLiteCommand cmd = new SQLiteCommand(thisSql.ToString(), conn))
                {
                    SQLiteDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        //userSecurityQuestions.Add(new SecurityQuestion { Id = Convert.ToInt32(dataReader["Id"]), Question = Convert.ToString(dataReader["Question"]) });
                    }
                }
                conn.Close();
            }
            return userSecurityQuestions;
        }

        #region Security Questions
        /// <summary>
        /// Retrieves a list of default SecurityQuestions stored in the database.
        /// </summary>
        /// <returns>List<SecurityQuestion></returns>
        public static List<SecurityQuestion> GetDefaultSecurityQuestions()
        {
            List<SecurityQuestion> securityQuestions = new List<SecurityQuestion>();
            StringBuilder thisSql = new StringBuilder();

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                thisSql.Clear().AppendLine("SELECT *")
                    .AppendLine("FROM SecurityQuestion");

                using (SQLiteCommand cmd = new SQLiteCommand(thisSql.ToString(), conn))
                {
                    SQLiteDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        securityQuestions.Add(new SecurityQuestion { Id = Convert.ToInt32(dataReader["Id"]), Question = Convert.ToString(dataReader["Question"]) });
                    }
                }
                conn.Close();
            }
            return securityQuestions;
        }

        /// <summary>
        /// Adds new user security questions.
        /// </summary>
        /// <param name="user">The user object containing the questions.</param>
        public static void InsertUserSecurityQuestions(User user)
        {
            StringBuilder thisSql = new StringBuilder();

            thisSql.AppendLine("INSERT INTO UserSecurityQuestion (UserId, SecurityQuestionId, Answer) ")
                   .Append($"VALUES ");

            foreach (UserSecurityQuestion currQuestion in user.SecurityQuestions)
            {
                thisSql.Append($"({user.Id}, {currQuestion.SecurityQuestion.Id}, '{currQuestion.Answer}'),");
            }

            if (!string.IsNullOrEmpty(thisSql.ToString()))
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    using (SQLiteCommand cmd = new SQLiteCommand(thisSql.ToString().TrimEnd(','), conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// Deletes a user's stored security questions.
        /// </summary>
        /// <param name="userId"></param>
        public static void DeleteUserSecurityQuestions(int userId)
        {
            string deleteSql = @"DELETE FROM UserSecurityQuestion WHERE UserId = {0}";

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = string.Format(deleteSql, userId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        #endregion
    }
}
