namespace crud_app.Repositories
{
    public interface IJwtToken
    {
        public string TokenGenerator(string? email, string? username, string password);
    }
}
