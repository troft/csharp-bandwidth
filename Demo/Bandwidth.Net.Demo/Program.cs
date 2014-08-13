using System;
using System.Linq;
using System.Threading.Tasks;
using Bandwidth.Net.Data;

namespace Bandwidth.Net.Demo
{
    class Program
    {
        static void Main()
        {
            try
            {
                RunDemos().Wait();
                Console.WriteLine("Done");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
                Environment.ExitCode = 1;
            }
        }

        static async Task RunDemos()
        {
            await GetApplicationsDemo.Run();
            await GetAvailableNumbersDemo.Run();
            await GetErrorsDemo.Run();
            await GetMediaDemo.Run();
            await GetMessagesDemo.Run();
            await BuyPhoneNumberDemo.Run();
            await GetPhoneNumberDemo.Run();
            await GetPhoneNumbersDemo.Run();
            await GetRecordingsDemo.Run();
            await MakingCallDemo.Run();
            await GetCallsDemo.Run();
            await PlayAudioDemo.Run();
            await SendSmsDemo.Run();
        }
    }
}
