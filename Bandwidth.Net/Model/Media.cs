using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Bandwidth.Net.Model
{
    public class Media
    {
        private const string MediaPath = "media";
        
        /// <summary>
        ///     Downloads a media file
        /// </summary>
        public static async Task<MediaContent> Download(Client client, string mediaName, bool asStream = false)
        {
            if (mediaName == null) throw new ArgumentNullException("mediaName");

            var response = await client.MakeGetRequest(client.ConcatUserPath(MediaPath), null, Uri.EscapeDataString(mediaName));
            var content = new MediaContent(response) { MediaType = response.Content.Headers.ContentType.MediaType };
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
        ///     Uploads a media file to the name you choose (via Stream)
        /// </summary>
        public static async Task<Media> Upload(Client client, string mediaName, Stream stream, string mediaType = null)
        {
            if (string.IsNullOrEmpty(mediaName)) throw new ArgumentNullException("mediaName");
            if (stream == null) throw new ArgumentNullException("stream");
            mediaType = mediaType ?? "application/octet-stream";
            await client.PutData(
                    client.ConcatUserPath(string.Format("{0}/{1}", MediaPath, Uri.EscapeDataString(mediaName))), stream,
                    mediaType, true);
            return await GetUploadedMedia(client, mediaName);
        }

        private static async Task<Media> GetUploadedMedia(Client client, string mediaName)
        {
            var list = await List(client, 0, 5);
            return (from i in list where i.MediaName == mediaName select i).FirstOrDefault();
        }

        /// <summary>
        ///     Uploads a media file to the name you choose (via byte array)
        /// </summary>
        public static async Task<Media> Upload(Client client, string mediaName, byte[] buffer, string mediaType = null)
        {
            if (string.IsNullOrEmpty(mediaName)) throw new ArgumentNullException("mediaName");
            if (buffer == null) throw new ArgumentNullException("buffer");
            mediaType = mediaType ?? "application/octet-stream";
            await client.PutData(
                    client.ConcatUserPath(string.Format("{0}/{1}", MediaPath, Uri.EscapeDataString(mediaName))), buffer,
                    mediaType, true);
            return await GetUploadedMedia(client, mediaName);
        }

        /// <summary>
        ///     Get a list of user's media files
        /// </summary>
        public static Task<Media[]> List(Client client, IDictionary<string, object> query = null)
        {
            return client.MakeGetRequest<Media[]>(client.ConcatUserPath(MediaPath), query);
        }

        public static Task<Media[]> List(Client client, int page, int size = 25)
        {
            return List(client, new Dictionary<string, object>{{"page", page}, {"size", size}});
        }


        /// <summary>
        ///     Permanently deletes a media file
        /// </summary>
        public static Task Delete(Client client, string mediaName)
        {
            if (string.IsNullOrEmpty(mediaName)) throw new ArgumentNullException("mediaName");
            return
                client.MakeDeleteRequest(
                   client.ConcatUserPath(string.Format("{0}/{1}", MediaPath, Uri.EscapeDataString(mediaName))));
        }
#if !PCL
        public static Task<MediaContent> Download(string mediaName, bool asStream = false)
        {
            return Download(Client.GetInstance(), mediaName, asStream);
        }

        public static Task<Media> Upload(string mediaName, Stream stream, string mediaType = null)
        {
            return Upload(Client.GetInstance(), mediaName, stream, mediaType);
        }

        public static Task<Media> Upload(string mediaName, byte[] buffer, string mediaType = null)
        {
            return Upload(Client.GetInstance(), mediaName, buffer, mediaType);
        }

        public static Task<Media[]> List(IDictionary<string, object> query = null)
        {
            return List(Client.GetInstance(), query);
        }

        public static Task<Media[]> List(int page, int size = 25)
        {
            return List(Client.GetInstance(), page, size);
        }

        public static Task Delete(string mediaName)
        {
            return Delete(Client.GetInstance(), mediaName);
        } 
#endif


        public int ContentLength { get; set; }
        public string MediaName { get; set; }
        public string Content { get; set; }
    }

    public sealed class MediaContent : IDisposable
    {
        private readonly IDisposable _owner;

        internal MediaContent(IDisposable owner)
        {
            if (owner == null) throw new ArgumentNullException("owner");
            _owner = owner;
        }

        public string MediaType { get; set; }
        public Stream Stream { get; set; }
        public byte[] Buffer { get; set; }

        public void Dispose()
        {
            _owner.Dispose();
        }
    }
}
