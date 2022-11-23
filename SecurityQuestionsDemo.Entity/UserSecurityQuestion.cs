namespace SecurityQuestionsDemo.Entity
{
    public class UserSecurityQuestion : BaseEntity
    {
        public int UserId { get; set; } 
        public SecurityQuestion SecurityQuestion { get; set; }
        public string Answer { get; set; }
    }
}
