using System.Threading.Tasks;
using Bandwidth.Net;

namespace Samples
{
    class Program
    {
        static void Main()
        {
            //Fill these options before run this demo
            Client.GlobalOptions = new ClientOptions
            {
                UserId = "",
                ApiToken = "",
                ApiSecret = ""
            };
            RunSamples().Wait();
        }

        private static async Task RunSamples()
        {
            await Applications.Run();
            await BuyPhoneNumber.Run();
            await MakeCall.Run();
            await SendMms.Run();
            await SendSms.Run();
            await ReceiveMms.Run();
            ShowResources.Run();
        }
    }
}
