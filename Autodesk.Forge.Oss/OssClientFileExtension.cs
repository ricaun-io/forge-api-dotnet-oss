using Autodesk.Forge.Model;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Autodesk.Forge.Oss
{
    /// <summary>
    /// OssClientFileExtension
    /// </summary>
    public static class OssClientFileExtension
    {
        /// <summary>
        /// Uploads a file to the OSS bucket asynchronously.
        /// </summary>
        /// <param name="oss">The OssClient instance.</param>
        /// <param name="bucketKey">The URL-encoded bucket key.</param>
        /// <param name="objectName">The URL-encoded object name.</param>
        /// <param name="localFullName">The local full name path of the file to upload.</param>
        /// <returns>The ObjectDetails of the uploaded file.</returns>
        public static async Task<ObjectDetails> UploadFileAsync(this OssClient oss, string bucketKey, string objectName, string localFullName)
        {
            // 2MB is minimal, clamp to it
            int chunkMbSize = 12; // 12/2 => 6.0 > 5MB is minimal
            long chunkSize = chunkMbSize * 1024 * 1024;
            int chunkNumber = 1;

            using var fileReadStream = File.OpenRead(localFullName);

            // determine if we need to upload in chunks or in one piece
            long sizeToUpload = fileReadStream.Length;

            // Auto Chunk Size
            if (sizeToUpload > chunkSize)
            {
                chunkNumber = (int) Math.Ceiling((double)sizeToUpload / (double)chunkSize);
                chunkSize = (long)Math.Ceiling(((double)sizeToUpload / chunkNumber));
            }

            var signeds3uploadResponse = await oss.GetS3UploadURLAsync(bucketKey, objectName, chunkNumber);

            // use chunks for all files greater than chunk size
            if (sizeToUpload > chunkSize)
            {
                string sessionId = Guid.NewGuid().ToString();
                long begin = 0;
                int urlIndex = 0;
                byte[] buffer = new byte[chunkSize];

                while (begin < sizeToUpload - 1)
                {
                    int memoryStreamSize = sizeToUpload - begin < chunkSize ? (int)(sizeToUpload - begin) : (int)chunkSize;
                    var bytesRead = await fileReadStream.ReadAsync(buffer, 0, memoryStreamSize);
                    using var chunkStream = new MemoryStream(buffer, 0, memoryStreamSize);

                    using HttpClient httpClient = new HttpClient();
                    using StreamContent streamContent = new StreamContent(chunkStream);
                    HttpResponseMessage response = await httpClient.PutAsync(signeds3uploadResponse.urls[urlIndex++], streamContent);

                    begin += bytesRead;
                }
            }
            else
            {
                var url = signeds3uploadResponse.urls.FirstOrDefault();
                using HttpClient httpClient = new HttpClient();
                using StreamContent streamContent = new StreamContent(fileReadStream);
                HttpResponseMessage response = await httpClient.PutAsync(url, streamContent);
            }
            return await oss.CompleteS3UploadAsync(bucketKey, objectName, signeds3uploadResponse.uploadKey, (int)sizeToUpload);
        }
        internal static async Task<ObjectDetails> UploadFileAsync2(this OssClient oss, string bucketKey, string objectName, string localFullName)
        {
            var signeds3uploadResponse = await oss.GetS3UploadURLAsync(bucketKey, objectName);
            var url = signeds3uploadResponse.urls.FirstOrDefault();

            using (FileStream fileStream = File.OpenRead(localFullName))
            {
                HttpClient httpClient = new HttpClient();
                StreamContent streamContent = new StreamContent(fileStream);
                HttpResponseMessage response = await httpClient.PutAsync(url, streamContent);
            }

            return await oss.CompleteS3UploadAsync(bucketKey, objectName, signeds3uploadResponse.uploadKey);
        }
        #region File
        /// <summary>
        /// Upload File
        /// <code>Based: https://github.com/autodesk-platform-services/aps-configurator-inventor/blob/master/WebApplication/State/OssBucket.cs</code>
        /// </summary>
        /// <param name="oss"></param>
		/// <param name="bucketKey">URL-encoded bucket key</param>
		/// <param name="objectName">URL-encoded object name</param>
        /// <param name="localFullName">Local full name path</param>
        /// <returns></returns>
        [Obsolete]
        internal static async Task<ObjectDetails> UploadFileAsyncObsolete(this OssClient oss, string bucketKey, string objectName, string localFullName)
        {
            ObjectDetails objectDetails = null;

            // 2MB is minimal, clamp to it
            int chunkMbSize = 5; // 5/2 => 2.5 > 2MB is minimal
            chunkMbSize = Math.Max(2, chunkMbSize);
            long chunkSize = chunkMbSize * 1024 * 1024;

            using var fileReadStream = File.OpenRead(localFullName);

            // determine if we need to upload in chunks or in one piece
            long sizeToUpload = fileReadStream.Length;

            // Auto Chunk Size
            if (sizeToUpload > chunkSize)
            {
                var chunkNumber = Math.Ceiling((double)sizeToUpload / (double)chunkSize);
                chunkSize = (long)Math.Ceiling((sizeToUpload / chunkNumber));
            }

            // use chunks for all files greater than chunk size
            if (sizeToUpload > chunkSize)
            {
                string sessionId = Guid.NewGuid().ToString();
                long begin = 0;
                byte[] buffer = new byte[chunkSize];

                while (begin < sizeToUpload - 1)
                {
                    int memoryStreamSize = sizeToUpload - begin < chunkSize ? (int)(sizeToUpload - begin) : (int)chunkSize;
                    var bytesRead = await fileReadStream.ReadAsync(buffer, 0, memoryStreamSize);
                    using var chunkStream = new MemoryStream(buffer, 0, memoryStreamSize);
                    var contentRange = $"bytes {begin}-{begin + bytesRead - 1}/{sizeToUpload}";
                    objectDetails = await oss.UploadChunkAsync(bucketKey, objectName, memoryStreamSize, contentRange, sessionId, chunkStream);
                    begin += bytesRead;
                }
            }
            else
            {
                objectDetails = await oss.UploadObjectAsync(bucketKey, objectName, (int)sizeToUpload, fileReadStream);
            }
            return objectDetails;
        }
        /// <summary>
        /// Download File
        /// </summary>
        /// <param name="oss"></param>
        /// <param name="bucketKey">URL-encoded bucket key</param>
        /// <param name="objectName">URL-encoded object name</param>
        /// <param name="localFullName">Local full name path</param>
        /// <returns></returns>
        public static async Task DownloadFileAsync(this OssClient oss, string bucketKey, string objectName, string localFullName)
        {
            var signedUrl = await oss.CreateSignedFileAsync(bucketKey, objectName);

            using var client = new HttpClient();
            using var httpStream = await client.GetStreamAsync(signedUrl);
            using var localStream = File.Create(localFullName);
            await httpStream.CopyToAsync(localStream);
        }

        /// <summary>
        /// CreateSignedFileAsync
        /// </summary>
        /// <param name="oss"></param>
		/// <param name="bucketKey">URL-encoded bucket key</param>
		/// <param name="objectName">URL-encoded object name</param>
        /// <param name="access">Access for signed resource Acceptable values: &#x60;read&#x60;, &#x60;write&#x60;, &#x60;readwrite&#x60;. Default value: &#x60;read&#x60;  (optional, default to read)</param>
        /// <param name="MinutesExpiration">Expiration time in minutes Default value: 60</param>
        /// <param name="singleUse">?Single use Default value: false</param>
        /// <returns></returns>
        public static async Task<string> CreateSignedFileAsync(this OssClient oss,
            string bucketKey, string objectName,
            string access = null, int? MinutesExpiration = null, bool singleUse = false)
        {
            var postObjectSigned = await oss.CreateSignedResourceAsync(bucketKey, objectName, new PostBucketsSigned(MinutesExpiration, singleUse), access);
            var signedUrl = postObjectSigned.SignedUrl;
            return signedUrl;
        }

        /// <summary>
        /// CreateSignedFileWriteAsync
        /// </summary>
        /// <param name="oss"></param>
		/// <param name="bucketKey">URL-encoded bucket key</param>
		/// <param name="objectName">URL-encoded object name</param>
        /// <param name="MinutesExpiration">Expiration time in minutes Default value: 60</param>
        /// <param name="singleUse">?Single use Default value: false</param>
        /// <returns></returns>
        public static async Task<string> CreateSignedFileWriteAsync(this OssClient oss, string bucketKey, string objectName, int? MinutesExpiration = null, bool singleUse = false)
        {
            return await CreateSignedFileAsync(oss, bucketKey, objectName, "write", MinutesExpiration, singleUse);
        }

        #endregion
    }
}

