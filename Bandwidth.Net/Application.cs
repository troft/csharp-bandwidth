namespace Bandwidth.Net
{
    public class Application
    {
        public string Name { get; private set; }
        public string IncomingCallUrl { get; private set; }
        public string IncomingSmsUrl { get; private set; }
        public string Script { get; private set; }

        public Application(string name, string incomingCallUrl, string incomingSmsUrl, string script)
        {
            Name = name;
            IncomingCallUrl = incomingCallUrl;
            IncomingSmsUrl = incomingSmsUrl;
            Script = script;
        }
    }
}
