using NUnit.Framework;
using OMS.Data;
using OMS.Logic;

namespace OMS.Tests.Data;

[TestFixture]
[TestOf(typeof(ValidateUser))]
public class ValidateUserTest
{
    private string User;
    private string fakeUser;
    private string Password;
    private string fakePassword;
    
    [SetUp]
    public void Setup()
    {
        User = "admin";
        Password = "admin123";
        
        fakeUser = "TEST";
        fakeUser = "admin123";
        
        
    }
    
    [Test]
    public void Check_User()
    {
        
        Assert.That(ValidateUser.Validater(User, Password), Is.True);
    }
    [Test]
    public void Check_Fake_User()
    {
        
        Assert.That(ValidateUser.Validater(fakeUser, fakeUser), Is.False);
    }
}