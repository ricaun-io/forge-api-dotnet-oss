using Autodesk.Forge;
using Autodesk.Forge.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Autodesk.Forge.Oss
{
    public partial class OssClient
    {
        /// <summary>
        /// Gets the S3 upload URL asynchronously.
        /// </summary>
        /// <param name="bucketKey">The bucket key.</param>
        /// <param name="objectName">The object name.</param>
        /// <param name="opts">The optional parameters.</param>
        /// <returns>The signed S3 upload response item.</returns>
        public async Task<PostBatchSignedS3UploadResponseItem> GetS3UploadURLAsync(string bucketKey, string objectName, Dictionary<string, object> opts = null)
        {
            var value = await this.ObjectsApi.getS3UploadURLAsync(bucketKey, objectName, opts) as DynamicJsonResponse;
            return value.ToObject<PostBatchSignedS3UploadResponseItem>();
        }

        /// <summary>
        /// Gets the S3 upload URL asynchronously.
        /// </summary>
        /// <param name="bucketKey">The bucket key.</param>
        /// <param name="objectName">The object name.</param>
        /// <param name="parts">The number of parts.</param>
        /// <param name="opts">The optional parameters.</param>
        /// <returns>The signed S3 upload response item.</returns>
        public async Task<PostBatchSignedS3UploadResponseItem> GetS3UploadURLAsync(string bucketKey, string objectName, int parts, Dictionary<string, object> opts = null)
        {
            opts ??= new Dictionary<string, object>();
            opts.Add("parts", parts);
            var value = await this.ObjectsApi.getS3UploadURLAsync(bucketKey, objectName, opts) as DynamicJsonResponse;
            return value.ToObject<PostBatchSignedS3UploadResponseItem>();
        }

        /// <summary>
        /// Completes the S3 upload asynchronously.
        /// </summary>
        /// <param name="bucketKey">The bucket key.</param>
        /// <param name="objectName">The object name.</param>
        /// <param name="uploadKey">The upload key.</param>
        /// <param name="opts">The optional parameters.</param>
        /// <returns>The object details.</returns>
        public async Task<ObjectDetails> CompleteS3UploadAsync(string bucketKey, string objectName, string uploadKey, Dictionary<string, object> opts = null)
        {
            return await this.CompleteS3UploadAsync(bucketKey, objectName, uploadKey, null, opts);
        }
        /// <summary>
        /// Completes the S3 upload asynchronously.
        /// </summary>
        /// <param name="bucketKey">The bucket key.</param>
        /// <param name="objectName">The object name.</param>
        /// <param name="uploadKey">The upload key.</param>
        /// <param name="size">The size of the object.</param>
        /// <param name="opts">The optional parameters.</param>
        /// <returns>The object details.</returns>
        public async Task<ObjectDetails> CompleteS3UploadAsync(string bucketKey, string objectName, string uploadKey, int? size, Dictionary<string, object> opts = null)
        {
            var body = new PostCompleteS3UploadPayload(uploadKey, size);
            var value = await this.ObjectsApi.completeS3UploadAsync(bucketKey, objectName, body, opts) as DynamicJsonResponse;
            return value.ToObject<ObjectDetails>();
        }
    }
}