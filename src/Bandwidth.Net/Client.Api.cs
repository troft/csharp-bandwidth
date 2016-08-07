using Bandwidth.Net.Api;

namespace Bandwidth.Net
{
  public partial class Client
  {
    /// <summary>
    /// Access to Account Api
    /// </summary>
    public IAccount Account { get; private set; }


    private void SetupApis()
    {
      Account = new AccountApi {Client = this};
    }
  }
}
