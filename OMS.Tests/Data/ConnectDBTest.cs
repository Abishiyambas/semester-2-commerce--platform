using NUnit.Framework;
using OMS.Data;

namespace OMS.Tests.Data;

[TestFixture]
[TestOf(typeof(ConnectDb))]
public class ConnectDbTest
{

    [Test]
    public void ShouldCheckConnection()
    {
        Assert.That(ConnectDb.GetNewConnection(), Is.Not.Null);
    }
}