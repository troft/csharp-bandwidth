using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Samples
{
    //This sample calls method 'list' of user's resourses ans shows results on console
    public  class ShowResources
    {
        private static JsonSerializerSettings _jsonSerializerSettings;

        public static void Run()
        {
            Show("Application"); //equal to var list = await Application.List(); foreach(item in list) {Console.WriteLine(ToJson(item));}
            Show("Bridge");
            Show("Call");
            Show("Domain");
            Show("Error");
            Show("Media", "Files");
            Show("Message");
            Show("PhoneNumber", "Numbers");
            Show("Recording");
        }


        /*
         This function will call static method "List" (via reflection) for given resource and show results in console
         */
        public static void Show(string resource, string title = null)
        {
            InitJsonSettings();
            if (title == null)
            {
                title = resource + "s";
            }
            Console.WriteLine(title);
            Console.WriteLine("========================");
            try
            {
                //get resource type
                var type = Type.GetType(string.Format("Bandwidth.Net.Model.{0}, Bandwidth.Net", resource));
                //try to call method "List(IDictionary<string, object> query = null)" first
                var method = type.GetMethod("List", new []{typeof(IDictionary<string, object>)});
                IEnumerable list;
                if (method == null)
                {
                    //otherwise call "List()"
                    method = type.GetMethod("List", new Type[0]);
                    list = ((dynamic)method.Invoke(null, new object[0])).Result as IEnumerable;
                }
                else
                {
                    list = ((dynamic)method.Invoke(null, new object[] { null })).Result as IEnumerable;
                }
                //List contains result of async task returned by "List"
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        //Show result in JSON format
                        Console.WriteLine(JsonConvert.SerializeObject(item, _jsonSerializerSettings));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
            }
            Console.WriteLine();
        }

        private static void InitJsonSettings()
        {
            if (_jsonSerializerSettings != null) return;
            _jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            _jsonSerializerSettings.Converters.Add(new StringEnumConverter
            {
                CamelCaseText = true,
                AllowIntegerValues = false
            });
            _jsonSerializerSettings.DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate;
            
        }
    }
}