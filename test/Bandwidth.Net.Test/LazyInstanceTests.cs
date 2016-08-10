using System;
using System.Threading.Tasks;
using Xunit;

namespace Bandwidth.Net.Test
{
  public class LazyInstanceTests
  {
    [Fact]
    public void TestConstructorWithMissingId()
    {
      Assert.Throws<ArgumentNullException>(() => new LazyInstance<object>(null, () => Task.FromResult<object>(null)));
    }

    [Fact]
    public void TestConstructorWithMissingGetInstance()
    {
      Assert.Throws<ArgumentNullException>(() => new LazyInstance<object>("id", null));
    }

    [Fact]
    public void TestGetInstance()
    {
      var callCount = 0;
      var instance = new LazyInstance<string>("id", () =>
      {
        callCount++;
        return Task.FromResult("value");
      });
      Assert.Equal("id", instance.Id);
      Assert.Equal(0, callCount);
      Assert.Equal("value", instance.Instance);
      Assert.Equal(1, callCount);
      Assert.Equal("value", instance.Instance);
      Assert.Equal(1, callCount);
    }
  }
}
