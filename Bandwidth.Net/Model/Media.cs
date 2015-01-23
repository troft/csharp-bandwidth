using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Bandwidth.Net.Model
{
    /// <summary>
    /// The Media resource lets you upload your media files to Bandwidth API servers so they can be used in application scripts without requiring a separate hosting provider
    /// </summary>
    /// <seealso href="https://catapult.inetwork.com/docs/api-docs/media/"/>
    public class Media
    {
        private const string MediaPath = "media";
        
        /// <summary>
        /// Downloads a media file
        /// </summary>
        /// <param name="client">Client instance</param>
        /// <param name="mediaName">File name to download</param>
        /// <param name="asStream">If true the file will be available as Stream object. Otherwise byte[] will be returned</param>
        /// <example>
        /// <code>
        /// using (var result = await Media.Download(client, "file1", true))
        /// {
        ///     // use result.Stream here
        /// }
        /// using (var result = await Media.Download(client, "file1"))
        /// {
        ///     // use result.Buffer here
        /// }
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/media/#GET-/v1/users/{userId}/media/{mediaName}"/>
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
        /// Uploads a media file to the name you choose (via Stream)
        /// </summary>
        /// <param name="client">Client instance</param>
        /// <param name="mediaName">File name</param>
        /// <param name="stream">Stream with file content</param>
        /// <param name="mediaType">Media type of file to upload</param>
        /// <example>
        /// <code>
        /// await Media.Upload(client, "file1.pdf", stream, "application/pdf");
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/media/#PUT-/v1/users/{userId}/media/{mediaName}"/>
        public static Task Upload(Client client, string mediaName, Stream stream, string mediaType = null)
        {
            if (string.IsNullOrEmpty(mediaName)) throw new ArgumentNullException("mediaName");
            if (stream == null) throw new ArgumentNullException("stream");
            mediaType = mediaType ?? "application/octet-stream";
            return client.PutData(
                    client.ConcatUserPath(string.Format("{0}/{1}", MediaPath, Uri.EscapeDataString(mediaName))), stream,
                    mediaType, true);
        }

        /// <summary>
        /// Uploads a media file to the name you choose (via byte array)
        /// </summary>
        /// <param name="client">Client instance</param>
        /// <param name="mediaName">File name</param>
        /// <param name="buffer">File content as byte array</param>
        /// <param name="mediaType">Media type of file to upload</param>
        /// <example>
        /// <code>
        /// await Media.Upload(client, "file1.pdf", buffer, "application/pdf");
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/media/#PUT-/v1/users/{userId}/media/{mediaName}"/>
        public static Task Upload(Client client, string mediaName, byte[] buffer, string mediaType = null)
        {
            if (string.IsNullOrEmpty(mediaName)) throw new ArgumentNullException("mediaName");
            if (buffer == null) throw new ArgumentNullException("buffer");
            mediaType = mediaType ?? "application/octet-stream";
            return client.PutData(
                    client.ConcatUserPath(string.Format("{0}/{1}", MediaPath, Uri.EscapeDataString(mediaName))), buffer,
                    mediaType, true);
        }

        /// <summary>
        /// Get a list of user's media files
        /// </summary>
        /// <param name="client">Client instance</param>
        /// <param name="query">Query parameters</param>
        /// <returns>List of files on the Bandwidth API server</returns>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/media/#GET-/v1/users/{userId}/media"/>
        public static Task<Media[]> List(Client client, IDictionary<string, object> query = null)
        {
            return client.MakeGetRequest<Media[]>(client.ConcatUserPath(MediaPath), query);
        }

        /// <summary>
        /// Get a list of user's media files
        /// </summary>
        /// <param name="client">Client instance</param>
        /// <param name="page">Page number</param>
        /// <param name="size">Page size</param>
        /// <returns>List of files on the Bandwidth API server</returns>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/media/#GET-/v1/users/{userId}/media"/>
        public static Task<Media[]> List(Client client, int page, int size = 25)
        {
            return List(client, new Dictionary<string, object>{{"page", page}, {"size", size}});
        }


        /// <summary>
        /// Permanently deletes a media file
        /// </summary>
        /// <param name="client">Client instance</param>
        /// <param name="mediaName">File name</param>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/media/#DELETE-/v1/users/{userId}/media/{mediaName}"/>
        public static Task Delete(Client client, string mediaName)
        {
            if (string.IsNullOrEmpty(mediaName)) throw new ArgumentNullException("mediaName");
            return
                client.MakeDeleteRequest(
                   client.ConcatUserPath(string.Format("{0}/{1}", MediaPath, Uri.EscapeDataString(mediaName))));
        }
#if !PCL
        /// <summary>
        /// Downloads a media file
        /// </summary>
        /// <param name="mediaName">File name to download</param>
        /// <param name="asStream">If true the file will be available as Stream object. Otherwise byte[] will be returned</param>
        /// <example>
        /// <code>
        /// using (var result = await Media.Download("file1", true))
        /// {
        ///     // use result.Stream here
        /// }
        /// using (var result = await Media.Download("file1"))
        /// {
        ///     // use result.Buffer here
        /// }
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/media/#GET-/v1/users/{userId}/media/{mediaName}"/>
        public static Task<MediaContent> Download(string mediaName, bool asStream = false)
        {
            return Download(Client.GetInstance(), mediaName, asStream);
        }

        /// <summary>
        /// Uploads a media file to the name you choose (via Stream)
        /// </summary>
        /// <param name="mediaName">File name</param>
        /// <param name="stream">Stream with file content</param>
        /// <param name="mediaType">Media type of file to upload</param>
        /// <example>
        /// <code>
        /// await Media.Upload(client, "file1.pdf", stream, "application/pdf");
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/media/#PUT-/v1/users/{userId}/media/{mediaName}"/>
        public static Task Upload(string mediaName, Stream stream, string mediaType = null)
        {
            return Upload(Client.GetInstance(), mediaName, stream, mediaType);
        }

        /// <summary>
        /// Uploads a media file to the name you choose (via byte array)
        /// </summary>
        /// <param name="mediaName">File name</param>
        /// <param name="buffer">File content as byte array</param>
        /// <param name="mediaType">Media type of file to upload</param>
        /// <example>
        /// <code>
        /// await Media.Upload(client, "file1.pdf", buffer, "application/pdf");
        /// </code>
        /// </example>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/media/#PUT-/v1/users/{userId}/media/{mediaName}"/>
        public static Task Upload(string mediaName, byte[] buffer, string mediaType = null)
        {
            return Upload(Client.GetInstance(), mediaName, buffer, mediaType);
        }

        /// <summary>
        /// Get a list of user's media files
        /// </summary>
        /// <param name="query">Query parameters</param>
        /// <returns>List of files on the Bandwidth API server</returns>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/media/#GET-/v1/users/{userId}/media"/>
        public static Task<Media[]> List(IDictionary<string, object> query = null)
        {
            return List(Client.GetInstance(), query);
        }

        /// <summary>
        /// Get a list of user's media files
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="size">Page size</param>
        /// <returns>List of files on the Bandwidth API server</returns>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/media/#GET-/v1/users/{userId}/media"/>
        public static Task<Media[]> List(int page, int size = 25)
        {
            return List(Client.GetInstance(), page, size);
        }

        /// <summary>
        /// Permanently deletes a media file
        /// </summary>
        /// <param name="mediaName">File name</param>
        /// <seealso href="https://catapult.inetwork.com/docs/api-docs/media/#DELETE-/v1/users/{userId}/media/{mediaName}"/>
        public static Task Delete(string mediaName)
        {
            return Delete(Client.GetInstance(), mediaName);
        } 
#endif

        /// <summary>
        /// Size of file
        /// </summary>
        public int ContentLength { get; set; }
        
        /// <summary>
        /// File name
        /// </summary>
        public string MediaName { get; set; }
        
        
        /// <summary>
        /// Url to file
        /// </summary>
        public string Content { get; set; }
    }

    /// <summary>
    /// Content of dowloaded file
    /// </summary>
    public sealed class MediaContent : IDisposable
    {
        private readonly IDisposable _owner;

        internal MediaContent(IDisposable owner)
        {
            if (owner == null) throw new ArgumentNullException("owner");
            _owner = owner;
        }

        /// <summary>
        /// Media type
        /// </summary>
        public string MediaType { get; set; }
        
        /// <summary>
        /// Content as Stream
        /// </summary>
        public Stream Stream { get; set; }
        
        /// <summary>
        /// Content as byte[]
        /// </summary>
        public byte[] Buffer { get; set; }

        public void Dispose()
        {
            _owner.Dispose();
        }
    }
}
