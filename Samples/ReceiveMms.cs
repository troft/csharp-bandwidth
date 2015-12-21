using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Bandwidth.Net;
using Bandwidth.Net.Model;
namespace Samples
{
    //This is a demo of receiving of mms
    public class ReceiveMms
    {
        public static async Task Run()
        {
            //Upload test media file if need ( to download it later)
            //It is required for this demo only. Don't use that in real apps.
            var file = (await Media.List()).FirstOrDefault(f => f.MediaName == "net_test.png");
            if (file == null)
            {
                await Media.Upload("net_test.png",
                    Assembly.GetExecutingAssembly().GetManifestResourceStream("Samples.test.png"), "image/png");
                file = (await Media.List()).FirstOrDefault(f => f.MediaName == "net_test.png");
            }

            //Getting event json data
            //In real apps you should read json data from http request payload 
            using (var reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Samples.mms.json")))
            {
                var json = await reader.ReadToEndAsync();

                var mms = BaseEvent.CreateFromString(json) as MmsEvent;

                if (mms != null) //if eventType is "mms"
                {
                    //Download all media files from message
                    foreach (var url in mms.Media)
                    {
                        var fileName = url.Split('/').Last(); //it will be equal 'net_test.png'. we uploaded this file before 
                        using (var result = await Media.Download(fileName))
                        {
                            File.WriteAllBytes(fileName, result.Buffer);
                        }
                    }
                    
                }
            }
        }
    }
}