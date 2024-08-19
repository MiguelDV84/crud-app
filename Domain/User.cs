
namespace crud_app.Domain
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password_hash { get; set; }
        public DateOnly Created_at { get; set; }
        public DateOnly Last_login { get; set; }
    }
}
