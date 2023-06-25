using Mkt.Core.Entities;
using Xunit;

namespace Mkt.UnitTests.Core.Entities;
public class UserTests
{
    [Fact]
    public void TestUser()
    {
        var entity = new User("fulano de tal", "fulanodetal@teste.com.br", "5551985623312", new DateTime(1986, 03, 18), "@D19e03m1963", "admin", "-");

        Assert.NotNull(entity.FullName);
        Assert.NotEmpty(entity.FullName);

        Assert.NotNull(entity.Phone);
        Assert.NotEmpty(entity.Phone);

        Assert.NotNull(entity.Email);
        Assert.NotEmpty(entity.Email);

        Assert.NotNull(entity.Password);
        Assert.NotEmpty(entity.Password);

        Assert.NotNull(entity.Role);
        Assert.NotEmpty(entity.Role);

    }
}
