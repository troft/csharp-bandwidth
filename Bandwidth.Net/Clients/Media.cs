using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Bandwidth.Net.Data;

namespace Bandwidth.Net.Clients
{
    public class Media
    {
        private const string MediaPath = "media";
        private readonly Client _client;

        internal Media(Client client)
        {
            _client = client;
        }


        /// <summary>
        ///     Uploads a media file to the name you choose (via Stream)
        /// </summary>
        public Task Set(string mediaName, Stream stream, string mediaType = null)
        {
            if (string.IsNullOrEmpty(mediaName)) throw new ArgumentNullException("mediaName");
            if (stream == null) throw new ArgumentNullException("stream");
            mediaType = mediaType ?? "application/octet-stream";
            return
                _client.PutData(
                    _client.ConcatUserPath(string.Format("{0}/{1}", MediaPath, Uri.EscapeDataString(mediaName))), stream,
                    mediaType, true);
        }

        /// <summary>
        ///     Uploads a media file to the name you choose (via byte array)
        /// </summary>
        public Task Set(string mediaName, byte[] buffer, string mediaType = null)
        {
            if (string.IsNullOrEmpty(mediaName)) throw new ArgumentNullException("mediaName");
            if (buffer == null) throw new ArgumentNullException("buffer");
            mediaType = mediaType ?? "application/octet-stream";
            return
                _client.PutData(
                    _client.ConcatUserPath(string.Format("{0}/{1}", MediaPath, Uri.EscapeDataString(mediaName))), buffer,
                    mediaType, true);
        }

        /// <summary>
        ///     Downloads a media file
        /// </summary>
        public async Task<MediaContent> Get(string mediaName, bool asStream = false)
        {
            if (mediaName == null) throw new ArgumentNullException("mediaName");

            HttpResponseMessage response =
                await
                    _client.MakeGetRequest(_client.ConcatUserPath(MediaPath), null, Uri.EscapeDataString(mediaName));
            var content = new MediaContent(response) {MediaType = response.Content.Headers.ContentType.MediaType};
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

        /// <summary>
        ///     Get a list of user's media files
        /// </summary>
        public Task<Data.Media[]> GetAll()
        {
            return _client.MakeGetRequest<Data.Media[]>(_client.ConcatUserPath(MediaPath));
        }

        /// <summary>
        ///     Permanently deletes a media file
        /// </summary>
        public Task Remove(string mediaName)
        {
            if (string.IsNullOrEmpty(mediaName)) throw new ArgumentNullException("mediaName");
            return
                _client.MakeDeleteRequest(
                    _client.ConcatUserPath(string.Format("{0}/{1}", MediaPath, Uri.EscapeDataString(mediaName))));
        }
    }
}