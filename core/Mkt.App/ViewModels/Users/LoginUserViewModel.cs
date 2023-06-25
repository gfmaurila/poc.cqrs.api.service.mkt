namespace Mkt.App.ViewModels.Users;
public class LoginUserViewModel
{
    public LoginUserViewModel(string email, string token)
    {
        Email = email;
        Token = token;
    }
    public string Email { get; set; }
    public string Token { get; set; }
}
