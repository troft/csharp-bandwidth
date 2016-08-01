using LightMock;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Bandwidth.Net.Test.Mocks
{
  public class Http : IHttp
  {
    private readonly IInvocationContext<IHttp> context;

    public Http(IInvocationContext<IHttp> context)
    {
      this.context = context;
    }

    public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken)
    {
      return context.Invoke(f => f.SendAsync(request, completionOption, cancellationToken));
    }
  }
}
