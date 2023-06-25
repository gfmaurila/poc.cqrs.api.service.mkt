namespace Mkt.App.ViewModels.Users;

public class UserViewModel
{
    public UserViewModel(string userName, string email)
    {
        FullName = userName;
        Email = email;
    }

    public UserViewModel(int id, string userName, string email, string phone)
    {
        Id = id;
        FullName = userName;
        Email = email;
        Phone = phone;
    }

    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
}
