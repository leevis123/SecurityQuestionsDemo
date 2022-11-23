namespace SecurityQuestionsDemo.Entity
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public List<UserSecurityQuestion> SecurityQuestions { get; set; }

        public User()
        {
            Id = 0;
            Name = String.Empty;
            SecurityQuestions = new List<UserSecurityQuestion>();
        }
    }
}