using Autodesk.Forge.Model;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Autodesk.Forge.Oss
{
    /// <summary>
    /// OssClientFileExtension
    /// </summary>
    public static class OssClientFileExtension
    {
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
        public static async Task<ObjectDetails> UploadFileAsync(this OssClient oss, string bucketKey, string objectName, string localFullName)
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

