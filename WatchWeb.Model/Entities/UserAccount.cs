namespace WatchWeb.Model.Entities
{
    public class UserAccount
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? PasswordResetToken { get; set; }
        public bool Active {  get; set; }
        public int Gender { get; set; }
        public string Phone { get; set; }
        public string TypeAccount { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<UserRole> UserRole { get; set; }
    }
}
