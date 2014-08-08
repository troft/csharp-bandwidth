using System;
using System.IO;
using System.Threading.Tasks;
using Bandwidth.Net.Data;

namespace Bandwidth.Net.Clients
{
    public class Media
    {
        private readonly Client _client;

        internal Media(Client client)
        {
            _client = client;
        }

        private const string MediaPath = "media";

       

        public Task Set(string mediaName, Stream stream, string mediaType = null)
        {
            if (string.IsNullOrEmpty(mediaName)) throw new ArgumentNullException("mediaName");
            if (stream == null) throw new ArgumentNullException("stream");
            return _client.PutFile(_client.ConcatUserPath(string.Format("{0}/{1}", MediaPath, mediaName)), stream, mediaType, true);
        }

        public Task Set(string mediaName, byte[] buffer, string mediaType = null)
        {
            if (string.IsNullOrEmpty(mediaName)) throw new ArgumentNullException("mediaName");
            if (buffer == null) throw new ArgumentNullException("buffer");
            return _client.PutFile(_client.ConcatUserPath(string.Format("{0}/{1}", MediaPath, mediaName)), buffer, mediaType, true);
        }

        public async Task<MediaContent> Get(string mediaName, bool asStream = false)
        {
            if (mediaName == null) throw new ArgumentNullException("mediaName");
            using (var response = await _client.MakeGetRequest(_client.ConcatUserPath(MediaPath), null, mediaName))
            {
                var content = new MediaContent {MediaType = response.Content.Headers.ContentType.MediaType};
                if (asStream)
                {
                    content.Stream = await response.Content.ReadAsStreamAsync();
                }
                else
                {
                    content.Buffer = await response.Content.ReadAsByteArrayAsync();
                }
                return content;
            }
        }
        
        public Task<Data.Media[]> GetAll()
        {
            return _client.MakeGetRequest<Data.Media[]>(_client.ConcatUserPath(MediaPath));
        }


        public Task Remove(string mediaName)
        {
            if (string.IsNullOrEmpty(mediaName)) throw new ArgumentNullException("mediaName");
            return _client.MakeDeleteRequest(_client.ConcatUserPath(string.Format("{0}/{1}", MediaPath, mediaName)));
        }
    }
}