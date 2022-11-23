using System;
using System.Security.Cryptography;
using SecurityQuestionsDemo.BL.SecurityQuestions;
using SecurityQuestionsDemo.BL.Users;
using SecurityQuestionsDemo.DAL;
using SecurityQuestionsDemo.Entity;

namespace FactoryDesignPattern
{
    class Program
    {
        private static User currentUser = new User();

        static void Main(string[] args)
        {
            DoSecurityQuestionsDemo();
        }

        /// <summary>
        /// Method for running the SecurityQuestionsDemo.
        /// </summary>
        private static void DoSecurityQuestionsDemo()
        {
            PromptUserForName();

            if (currentUser.SecurityQuestions.Count == 0)
            {
                SetUserSecurityQuestions();
            }
            else
            {
                bool hasValidInput = false;
                string userInput = string.Empty;

                Console.Write("Do you want to answer a security question? Y or N ");

                while (!hasValidInput)
                {
                    userInput = Console.ReadLine();

                    if (userInput.ToLower() == "y" || userInput.ToLower() == "n")
                    {
                        hasValidInput = true;
                    }
                    else
                    {
                        Console.Write("You did not provide a valid response. Please enter Y or N.");
                    }
                }

                if (userInput.ToLower() == "y")
                {
                    //Present the Answer flow.
                    AnswerUserSecurityQuestions();
                }
                else if (userInput.ToLower() == "n")
                {
                    //Send to Store flow to re-do answers.
                    SetUserSecurityQuestions();
                }
            }
        }

        /// <summary>
        /// "Initial" flow for setting the user.
        /// </summary>
        private static void PromptUserForName()
        {
            bool hasValidInput = false;
            string userName = string.Empty;
            currentUser = new User();

            //Prompt user for name. 
            Console.Clear();
            Console.Write("Hi, what is your name? ");

            while (!hasValidInput)
            {
                userName = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(userName))
                {
                    //A name was not provided. 
                    Console.WriteLine("Sorry, you did not enter a name.");
                    Console.Write("Please enter your name: ");
                }
                else
                {
                    //Check if this user exists. 
                    currentUser = UserManager.GetUserByName(userName);

                    if (currentUser == null)
                    {
                        currentUser = UserManager.AddNewUser(userName);
                    }
                    hasValidInput = true;
                }
            }
        }

        /// <summary>
        /// "Store" flow for capturing the user's security questions/answers.
        /// </summary>
        private static void SetUserSecurityQuestions()
        {
            List<UserSecurityQuestion> userSecurityQuestions = new List<UserSecurityQuestion>();
            List<SecurityQuestion> defaultSecurityQuestions = SecurityQuestionsManager.GetDefaultSecurityQuestions();
            List<SecurityQuestion> availableSecurityQuestions = defaultSecurityQuestions;

            bool hasValidInput = false;
            bool hasValidSecurityQuestions = false;
            string userInput = string.Empty;
            int selectedQuestionId = 0;

            Console.WriteLine("Please select 3 of the following questions and provide an answer for each.");
            Console.WriteLine(string.Empty);

            do
            {
                for (int currQuestionIndex = 0; currQuestionIndex < availableSecurityQuestions.Count; currQuestionIndex++)
                {
                    Console.WriteLine(string.Format("{0}. {1}", currQuestionIndex + 1, availableSecurityQuestions[currQuestionIndex].Question));
                }

                Console.WriteLine(string.Empty);
                Console.Write("Enter the number of the question you would like to use: ");
                hasValidInput = false;

                while (!hasValidInput)
                {
                    userInput = Console.ReadLine();

                    if (!int.TryParse(userInput, out selectedQuestionId))
                    {
                        Console.Write("You did not enter a number. Please enter the number of the question you would like to use: ");
                    }
                    else
                    {
                        //Verify that the number entered matches one of the numbered questions.
                        if (selectedQuestionId == 0 || selectedQuestionId > availableSecurityQuestions.Count)
                        {
                            Console.Write($"The number {selectedQuestionId} is not a valid option. Please enter the number of the question you would like to use: ");
                        }
                        else
                        {
                            hasValidInput = true;
                        }
                    }
                }

                hasValidInput = false;

                //Prompt the user for answer to the selected security question. 
                Console.WriteLine("Awesome! Now provide an answer for the question you've selected.");
                Console.Write($"{availableSecurityQuestions[selectedQuestionId - 1].Question} ");

                while (!hasValidInput)
                {
                    userInput = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(userInput))
                    {
                        Console.WriteLine("You did not provide an answer to the security question.");
                        Console.Write($"{availableSecurityQuestions[selectedQuestionId - 1].Question} ");
                    }
                    else
                    {
                        //A valid answer was provided. Save the response. 
                        UserSecurityQuestion userSecurityQuestion = new UserSecurityQuestion();
                        userSecurityQuestion.UserId = currentUser.Id;
                        userSecurityQuestion.SecurityQuestion = new SecurityQuestion { Id = availableSecurityQuestions[selectedQuestionId - 1].Id };
                        userSecurityQuestion.Answer = userInput;

                        userSecurityQuestions.Add(userSecurityQuestion);
                        hasValidInput = true;
                    }
                }

                if (userSecurityQuestions.Count < 3)
                {
                    //Filter the list of default security questions to exclude previously selected questions.
                    var selectedQuestionIds = (from x in userSecurityQuestions
                                               select x.SecurityQuestion.Id).ToList();

                    availableSecurityQuestions = (from x in defaultSecurityQuestions
                                                  where !selectedQuestionIds.Contains(x.Id)
                                                  select x).ToList();

                    Console.WriteLine(string.Empty);
                    Console.WriteLine($"Now select {3 - selectedQuestionIds.Count} more security question(s).");
                    Console.WriteLine(string.Empty);
                }
                else
                {
                    hasValidSecurityQuestions = true;
                }
            } while (!hasValidSecurityQuestions);

            Console.Write("Would you like to store your answers to the security questions? Y or N ");
            hasValidInput = false;

            while (!hasValidInput)
            {
                userInput = Console.ReadLine();

                if (userInput.ToLower() == "y")
                {
                    //Save the user's security questions/answers and redirect to PromptForName.
                    currentUser.SecurityQuestions = userSecurityQuestions;
                    UserManager.SaveUserSecurityQuestions(currentUser);
                    PromptUserForName();
                }
                else if (userInput.ToLower() == "n")
                {
                    //User declined to save questions/answers. Reset user object and redirect to PromptForName.
                    currentUser = new User();
                    PromptUserForName();
                }
                else
                {
                    //Input was not valid.
                    Console.WriteLine("You did not provide valid input. Please enter either Y or N.");
                    Console.Write("Would you like to store your answers to the security questions? Y or N ");
                }
            }
        }

        /// <summary>
        /// "Answer" flow for testing user's security questions/answers.
        /// </summary>
        private static void AnswerUserSecurityQuestions()
        {
            List<UserSecurityQuestion> availableSecurityQuestions = (from x in currentUser.SecurityQuestions
                                                                 select x).ToList();
            List<int> attemptedQuestionIds = new List<int>();
            bool hasValidAnswer = false;
            bool hasValidInput = false; 
            int questionIndex = 0;
            Random random = new Random();
            UserSecurityQuestion securityQuestion;
            string userInput = string.Empty;

            while (!hasValidAnswer)
            {
                questionIndex = random.Next(availableSecurityQuestions.Count);
                securityQuestion = availableSecurityQuestions[questionIndex];
                hasValidInput = false;

                Console.Write($"{securityQuestion.SecurityQuestion.Question} ");

                while (!hasValidInput)
                {
                    userInput = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(userInput))
                    {
                        //An answer was not provided. 
                        Console.WriteLine("Sorry, you did not provide an answer.");
                        Console.Write($"{securityQuestion.SecurityQuestion.Question} ");
                    }
                    else
                    {
                        hasValidInput = true;
                    }
                }

                //Compare the answer to the user's stored answer.                
                if (userInput.ToLower() != securityQuestion.Answer.ToLower())
                {
                    //Provided answer does not match the stored answer. 
                    attemptedQuestionIds.Add(securityQuestion.Id);

                    //Exclude the attempted question from the collection.
                    availableSecurityQuestions = (from x in currentUser.SecurityQuestions
                                                  where !attemptedQuestionIds.Contains(x.Id)
                                                  select x).ToList();

                    if (availableSecurityQuestions.Count > 0)
                    {
                        Console.WriteLine($"The answer provided was incorrect. You have {availableSecurityQuestions.Count} more attempt(s) to provide a correct answer.");
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine($"The answer provided was incorrect. You have run out of security questions to answer.");
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                        PromptUserForName();
                    }
                }
                else
                {
                    Console.WriteLine("Congratulations! You answered your security question correctly.");
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    PromptUserForName();
                }
            }
        }
    }
}