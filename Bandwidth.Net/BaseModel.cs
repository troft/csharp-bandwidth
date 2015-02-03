namespace Bandwidth.Net
{
    public class BaseModel
    {
        private Client _client;
        public string Id { get; set; }

        internal Client Client
        {
            get
            {
                if (_client == null)
                {
                    _client = Client.GetInstance();
                }
                return _client;
            }
            set { _client = value; }
        }

        public void SetClient(Client client)
        {
            _client = client;
        }
    }
}
