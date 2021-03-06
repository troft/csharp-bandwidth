using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LightMock;
using Xunit;

namespace Bandwidth.Net.Test
{
  public class ClientTests
  {
    [Fact]
    public void TestConstructorWithBaseUrl()
    {
      var api = new Client("userId", "apiToken", "apiSecret", "http://host");
      Assert.Equal("http://host/v1/", api.CreateRequest(HttpMethod.Get, "/").RequestUri.ToString());
    }

    [Fact]
    public void TestConstructorWithEmptyCredentialParams()
    {
      Assert.Throws<MissingCredentialsException>(() => new Client("", "apiToken", "apiSecret"));
    }

    [Fact]
    public void TestConstructorWithEmptyBaseUrl()
    {
      Assert.Throws<InvalidBaseUrlException>(() => new Client("userId", "apiToken", "apiSecret", ""));
    }


    [Fact]
    public void TestCreateRequest()
    {
      var api = Helpers.GetClient();
      var request = api.CreateRequest(HttpMethod.Get, "/test");
      Assert.Equal(HttpMethod.Get, request.Method);
      Assert.Equal("http://localhost/v1/test", request.RequestUri.ToString());
      var hash = Convert.ToBase64String(Encoding.UTF8.GetBytes("apiToken:apiSecret"));
      Assert.Equal($"Basic {hash}", request.Headers.Authorization.ToString());
    }

    [Fact]
    public void TestCreateRequestWithQuery()
    {
      var api = Helpers.GetClient();
      var request = api.CreateRequest(HttpMethod.Get, "/test",
        new
        {
          Field1 = 1,
          Field2 = "text value",
          Field3 = new DateTime(2016, 8, 1, 0, 0, 0, DateTimeKind.Utc),
          EmptyField = (string) null,
          EmptyField2 = ""
        });
      Assert.Equal(HttpMethod.Get, request.Method);
      Assert.Equal(
        "http://localhost/v1/test?field1=1&field2=text value&field3=2016-08-01T00:00:00.0000000Z",
        request.RequestUri.ToString());
      var hash = Convert.ToBase64String(Encoding.UTF8.GetBytes("apiToken:apiSecret"));
      Assert.Equal($"Basic {hash}", request.Headers.Authorization.ToString());
    }

    [Fact]
    public void TestCreateGetRequest()
    {
      var api = Helpers.GetClient();
      var request = api.CreateGetRequest("http://host/path");
      Assert.Equal(HttpMethod.Get, request.Method);
      Assert.Equal("http://host/path", request.RequestUri.ToString());
      var hash = Convert.ToBase64String(Encoding.UTF8.GetBytes("apiToken:apiSecret"));
      Assert.Equal($"Basic {hash}", request.Headers.Authorization.ToString());
    }


    [Fact]
    public async void TestMakeRequest()
    {
      var context = new MockContext<IHttp>();
      var api = Helpers.GetClient(context);
      var request = new HttpRequestMessage(HttpMethod.Get, "/test");
      context.Arrange(m => m.SendAsync(request, HttpCompletionOption.ResponseContentRead, null))
        .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));
      using (var response =
        await api.MakeRequestAsync(request))
      {
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
      }
    }

    [Fact]
    public async void TestMakeJsonTRequest()
    {
      var context = new MockContext<IHttp>();
      var api = Helpers.GetClient(context);
      context.Arrange(
        m => m.SendAsync(The<HttpRequestMessage>.IsAnyValue, HttpCompletionOption.ResponseContentRead, null))
        .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
        {
          Content = new StringContent("{\"test\": \"value\"}", Encoding.UTF8, "application/json")
        }));
      var result = await api.MakeJsonRequestAsync<MakeJsonRequestDemo>(HttpMethod.Get, "/test");
      Assert.Equal("value", result.Test);
    }

    [Fact]
    public async void TestMakeJsonRequest()
    {
      var context = new MockContext<IHttp>();
      var api = Helpers.GetClient(context);
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidRequestWithoutBody(r)),
            HttpCompletionOption.ResponseContentRead, null))
        .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));
      using (var response = await api.MakeJsonRequestAsync(HttpMethod.Get, "/test"))
      {
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
      }
    }

    [Fact]
    public async void TestMakeJsonWithoutRequestRequest()
    {
      var context = new MockContext<IHttp>();
      var api = Helpers.GetClient(context);
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidRequestWithoutBody(r)),
            HttpCompletionOption.ResponseContentRead, null))
        .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));
      await api.MakeJsonRequestWithoutResponseAsync(HttpMethod.Get, "/test");
      
    }

    [Fact]
    public async void TestMakeJsonRequestWithBody()
    {
      var context = new MockContext<IHttp>();
      var api = Helpers.GetClient(context);
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidRequestWithBody(r)),
            HttpCompletionOption.ResponseContentRead, null))
        .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));
      var response = await api.MakeJsonRequestAsync(HttpMethod.Post, "/test", null, null, new {Field = "value"});
      Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async void TestMakePostJsonRequest()
    {
      var context = new MockContext<IHttp>();
      var response = new HttpResponseMessage(HttpStatusCode.OK);
      response.Headers.Location = new Uri("http://host/path/id");
      var api = Helpers.GetClient(context);
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidRequestWithBody(r)),
            HttpCompletionOption.ResponseContentRead, null))
        .Returns(Task.FromResult(response));
      var id = await api.MakePostJsonRequestAsync("/test", null, new { Field = "value" });
      Assert.Equal("id", id);
    }

    [Fact]
    public async void TestMakePostJsonRequestWithoutLocationHeader()
    {
      var context = new MockContext<IHttp>();
      var response = new HttpResponseMessage(HttpStatusCode.OK);
      var api = Helpers.GetClient(context);
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidRequestWithBody(r)),
            HttpCompletionOption.ResponseContentRead, null))
        .Returns(Task.FromResult(response));
      var id = await api.MakePostJsonRequestAsync("/test", null, new { Field = "value" });
      Assert.True(string.IsNullOrEmpty(id));
    }


    public static bool IsValidRequestWithBody(HttpRequestMessage request)
    {
      return request.Content.Headers.ContentType.MediaType == "application/json"
             && request.Content.ReadAsStringAsync().Result == "{\"field\":\"value\"}";
    }

    public static bool IsValidRequestWithoutBody(HttpRequestMessage request)
    {
      return request.Content == null;
    }

    public class MakeJsonRequestDemo
    {
      public string Test { get; set; }
    }
  }
}
