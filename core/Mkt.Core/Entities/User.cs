namespace Mkt.Core.Entities;

public class User : BaseEntity
{
    public User()
    {
    }

    public User(string fullName, string email, string phone, DateTime birthDate, string password, string role, string generateCodeReset)
    {
        FullName = fullName;
        Email = email;
        Phone = phone;
        BirthDate = birthDate;
        CreatedAt = DateTime.Now;
        Active = true;
        Password = password;
        Role = role;
        GenerateCodeReset = generateCodeReset;
    }

    public void Update(string fullName, string email, string phone, DateTime birthDate, string role)
    {
        FullName = fullName;
        Email = email;
        Phone = phone;
        BirthDate = birthDate;
        Role = role;
    }

    public void UpdateGenerateCodeReset(string generateCodeReset)
    {
        GenerateCodeReset = generateCodeReset;
    }

    public string FullName { get; private set; }
    public string Email { get; private set; }
    public string Phone { get; private set; }
    public DateTime BirthDate { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool Active { get; set; }
    public string Password { get; private set; }
    public string GenerateCodeReset { get; private set; }
    public string Role { get; private set; }

}
