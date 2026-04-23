public class User
{
    public string Username { get; set; }
    public string Password { get; set; }
    public bool IsEmployee { get; set; }

    public User(string username, string password, bool isEmployee = false)
    {
        Username = username;
        Password = password;
        IsEmployee = isEmployee;
    }
}