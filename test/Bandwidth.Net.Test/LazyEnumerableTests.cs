using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bandwidth.Net.Test.Mocks;
using LightMock;
using Xunit;

namespace Bandwidth.Net.Test
{
  public class LazyEnumerableTests
  {
    [Fact]
    public void TestConstructorWithNullParameters()
    {
      Assert.Throws<ArgumentNullException>(() => new LazyEnumerable<string>(null, () => Task.FromResult(new HttpResponseMessage())));
      Assert.Throws<ArgumentNullException>(() => new LazyEnumerable<string>(new Client("userId", "apiToken", "apiSecret"), null));
    }

    [Fact]
    public void TestGetEnumerator()
    {
      var response = new HttpResponseMessage
      {
        Content = new StringContent("[\"1\", \"2\"]", Encoding.UTF8, "application/json")
      };
      var list = new LazyEnumerable<string>(new Client("userId", "apiToken", "apiSecret"), () => Task.FromResult(response));
      Assert.Equal(new[] { "1", "2" }, list);
    }

    [Fact]
    public void TestGetEnumeratorWithLinkHeader()
    {
      var response = new HttpResponseMessage
      {
        Content = new StringContent("[\"1\", \"2\"]", Encoding.UTF8, "application/json")
      };
      response.Headers.Add("Link", "<https://api.catapult.inetwork.com/v1/users/userId/account/transactions?page=0&size=25>; rel=\"first\"");
      var list = new LazyEnumerable<string>(new Client("userId", "apiToken", "apiSecret"), () => Task.FromResult(response));
      Assert.Equal(new[] { "1", "2" }, list);
    }

    [Fact]
    public void TestGetEnumeratorWithNextPage()
    {
      var response = new HttpResponseMessage
      {
        Content = new StringContent("[\"1\"]", Encoding.UTF8, "application/json")
      };
      response.Headers.Add("Link", "<https://api.catapult.inetwork.com/v1/users/userId/account/transactions?page=0&size=25>; rel=\"first\", <https://api.catapult.inetwork.com/v1/users/userId/account/transactions?page=1&size=25>; rel=\"next\"");
      var nextPageResponse = new HttpResponseMessage
      {
        Content = new StringContent("[\"2\", \"3\"]", Encoding.UTF8, "application/json")
      };
      var context = new MockContext<IHttp>();
      context.Arrange(h => h.SendAsync(The<HttpRequestMessage>.Is(m => IsValidRequest(m)), HttpCompletionOption.ResponseContentRead, CancellationToken.None)).Returns(Task.FromResult(nextPageResponse));
      var list = new LazyEnumerable<string>(new Client("userId", "apiToken", "apiSecret", "url", new Http(context)), () => Task.FromResult(response));
      Assert.Equal(new[] { "1", "2", "3" }, list);
    }

    public static bool IsValidRequest(HttpRequestMessage request)
    {
      return request.RequestUri.ToString() == "https://api.catapult.inetwork.com/v1/users/userId/account/transactions?page=1&size=25"
       && request.Method == HttpMethod.Get;
    }

    [Fact]
    public void TestGetEnumeratorWithWrongLinkHeader()
    {
      var response = new HttpResponseMessage
      {
        Content = new StringContent("[\"1\", \"2\"]", Encoding.UTF8, "application/json")
      };
      response.Headers.Add("Link", "<https://api.catapult.inetwork.com/v1/users/userId/account/transactions?page=0&size=25> rel=\"next\"");
      var list = new LazyEnumerable<string>(new Client("userId", "apiToken", "apiSecret"), () => Task.FromResult(response));
      Assert.Equal(new[] { "1", "2" }, list);
    }
  }
}
