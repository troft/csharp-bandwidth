using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Bandwidth.Net
{
    public interface IHttp
    {
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,  HttpCompletionOption completionOption, CancellationToken cancellationToken);
    }

    internal class Http: IHttp
    {
        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,  HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            using(var client = new HttpClient())
            {
                return await client.SendAsync(request, completionOption, cancellationToken);
            }
        }
    }
}
