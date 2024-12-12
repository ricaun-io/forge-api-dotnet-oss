using Autodesk.Forge;
using Autodesk.Forge.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Autodesk.Forge.Oss
{
    /// <summary>
    /// OssClient
    /// </summary>
    public partial class OssClient
    {
        /// <summary>
        /// BucketsApi
        /// </summary>
        public IBucketsApi BucketsApi { get; }
        /// <summary>
        /// ObjectsApi
        /// </summary>
        public IObjectsApi ObjectsApi { get; }
        /// <summary>
        /// Configuration
        /// </summary>
        public Configuration Configuration { get; }
        /// <summary>
        /// OssClient
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="bucketsApi"></param>
        /// <param name="objectsApi"></param>
        public OssClient(
            Configuration configuration = null,
            IBucketsApi bucketsApi = null,
            IObjectsApi objectsApi = null)
        {

            this.Configuration = configuration ?? Configuration.CreateDefault();
            var clientConfiguration = Configuration.GetConfiguration();

            this.BucketsApi = bucketsApi ?? new BucketsApi(clientConfiguration);
            this.ObjectsApi = objectsApi ?? new ObjectsApi(clientConfiguration);
        }

        #region BucketsApi
        /// <summary>
        /// This endpoint will return the buckets owned by the application. This endpoint supports pagination. 
        /// </summary>
        /// <exception cref="Autodesk.Forge.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="region">The region where the bucket resides Acceptable values: &#x60;US&#x60;, &#x60;EMEA&#x60; Default is &#x60;US&#x60;  (optional, default to US)</param>
        /// <param name="limit">Limit to the response size, Acceptable values: 1-100 Default &#x3D; 10  (optional, default to 10)</param>
        /// <param name="startAt">Key to use as an offset to continue pagination This is typically the last bucket key found in a preceding GET buckets response  (optional)</param>
        /// <returns>Task of Buckets</returns>
        public async Task<Buckets> GetBucketsAsync(string region = null, int? limit = null, string startAt = null)
        {
            var value = await BucketsApi.GetBucketsAsync(region, limit, startAt) as DynamicJsonResponse;
            return value.ToObject<Buckets>();
        }

        /// <summary>
		/// This endpoint will return the buckets owned by the application. This endpoint supports pagination.
		/// </summary>
		/// <exception cref="Autodesk.Forge.Client.ApiException">Thrown when fails to make API call</exception>
		/// <param name="bucketKey">URL-encoded bucket key</param>
		/// <returns>Task of Bucket</returns>
        public async Task<Bucket> GetBucketDetailsAsync(string bucketKey)
        {
            var value = await BucketsApi.GetBucketDetailsAsync(bucketKey) as DynamicJsonResponse;
            return value.ToObject<Bucket>();
        }

        /// <summary>
		/// This endpoint will return the buckets owned by the application. This endpoint supports pagination.
		/// </summary>
		/// <param name="bucketKey">URL-encoded bucket key</param>
		/// <returns>Task of Bucket or null if not exist</returns>
        public async Task<Bucket> TryGetBucketDetailsAsync(string bucketKey)
        {
            try
            {
                return await GetBucketDetailsAsync(bucketKey);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
		/// This endpoint will delete a bucket. 
		/// </summary>
		/// <exception cref="Autodesk.Forge.Client.ApiException">Thrown when fails to make API call</exception>
		/// <param name="bucketKey">URL-encoded bucket key</param>
		/// <returns>Task of void</returns>
        public async Task DeleteBucketAsync(string bucketKey)
        {
            await BucketsApi.DeleteBucketAsync(bucketKey);
        }

        /// <summary>
		/// Use this endpoint to create a bucket. Buckets are arbitrary spaces created and owned by applications. Bucket keys are globally unique across all regions, regardless of where they were created, and they cannot be changed. The application creating the bucket is the owner of the bucket. 
		/// </summary>
		/// <exception cref="Autodesk.Forge.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="bucketKey">Bucket key (required).</param>
        /// <param name="allow">.</param>
        /// <param name="policyKey">[Data retention policy](https://developer.autodesk.com/en/docs/data/v2/overview/retention-policy/)  Acceptable values: &#x60;transient&#x60;, &#x60;temporary&#x60; or &#x60;persistent&#x60;  (required).</param>
        /// <param name="xAdsRegion">The region where the bucket resides Acceptable values: &#x60;US&#x60;, &#x60;EMEA&#x60; Default is &#x60;US&#x60;  (optional, default to US)</param>
        /// <returns></returns>
        public async Task<Bucket> CreateBucketAsync(
            string bucketKey,
            List<PostBucketsPayloadAllow> allow = null,
            PostBucketsPayload.PolicyKeyEnum policyKey = PostBucketsPayload.PolicyKeyEnum.Transient,
            string xAdsRegion = null)
        {
            var postBuckets = new PostBucketsPayload(bucketKey, allow, policyKey);
            return await this.CreateBucketAsync(postBuckets, xAdsRegion);
        }

        /// <summary>
		/// Use this endpoint to create a bucket. Buckets are arbitrary spaces created and owned by applications. Bucket keys are globally unique across all regions, regardless of where they were created, and they cannot be changed. The application creating the bucket is the owner of the bucket. 
		/// </summary>
		/// <exception cref="Autodesk.Forge.Client.ApiException">Thrown when fails to make API call</exception>
		/// <param name="postBuckets">Body Structure</param>
		/// <param name="xAdsRegion">The region where the bucket resides Acceptable values: &#x60;US&#x60;, &#x60;EMEA&#x60; Default is &#x60;US&#x60;  (optional, default to US)</param>
		/// <returns>Task of Bucket</returns>
        public async Task<Bucket> CreateBucketAsync(PostBucketsPayload postBuckets, string xAdsRegion = null)
        {
            var value = await BucketsApi.CreateBucketAsync(postBuckets, xAdsRegion) as DynamicJsonResponse;
            return value.ToObject<Bucket>();
        }
        #endregion

        #region ObjectsApi
        /// <summary>
		/// List objects in a bucket. It is only available to the bucket creator.
		/// </summary>
		/// <exception cref="Autodesk.Forge.Client.ApiException">Thrown when fails to make API call</exception>
		/// <param name="bucketKey">URL-encoded bucket key</param>
		/// <param name="limit">Limit to the response size, Acceptable values: 1-100 Default &#x3D; 10  (optional, default to 10)</param>
		/// <param name="beginsWith">Provides a way to filter the based on object key name (optional)</param>
		/// <param name="startAt">Key to use as an offset to continue pagination This is typically the last bucket key found in a preceding GET buckets response  (optional)</param>
		/// <returns>Task of BucketObjects</returns>
        public async Task<BucketObjects> GetObjectsAsync(string bucketKey, int? limit = null, string beginsWith = null, string startAt = null)
        {
            var value = await this.ObjectsApi.GetObjectsAsync(bucketKey, limit, beginsWith, startAt) as DynamicJsonResponse;
            return value.ToObject<BucketObjects>();
        }
        //      /// <summary>
        ///// Download an object.
        ///// </summary>
        ///// <exception cref="Autodesk.Forge.Client.ApiException">Thrown when fails to make API call</exception>
        ///// <param name="bucketKey">URL-encoded bucket key</param>
        ///// <param name="objectName">URL-encoded object name</param>
        ///// <param name="range">A range of bytes to download from the specified object. (optional)</param>
        ///// <param name="ifNoneMatch">The value of this header is compared to the ETAG of the object. If they match, the body will not be included in the response. Only the object information will be included. (optional)</param>
        ///// <param name="ifModifiedSince">If the requested object has not been modified since the time specified in this field, an entity will not be returned from the server; instead, a 304 (not modified) response will be returned without any message body.  (optional)</param>
        ///// <param name="acceptEncoding">When gzip is specified, a gzip compressed stream of the object’s bytes will be returned in the response. Cannot use “Accept-Encoding:gzip” with Range header containing an end byte range. End byte range will not be honored if “Accept-Encoding: gzip” header is used.  (optional)</param>
        ///// <returns>Task of System.IO.Stream</returns>
        //[Obsolete]
        //public async Task<Stream> GetObjectAsync(string bucketKey, string objectName, string range = null, string ifNoneMatch = null, DateTime? ifModifiedSince = null, string acceptEncoding = null)
        //{
        //    var value = await this.ObjectsApi.GetObjectAsync(bucketKey, objectName, range, ifNoneMatch, ifModifiedSince, acceptEncoding);
        //    return value as Stream;
        //}
        /// <summary>
        /// GetObjectAsync using CreateSignedFileAsync and GetStreamAsync
        /// </summary>
        /// <param name="bucketKey"></param>
        /// <param name="objectName"></param>
        /// <returns></returns>
        public async Task<Stream> GetObjectAsync(string bucketKey, string objectName)
        {
            var signedUrl = await this.CreateSignedFileAsync(bucketKey, objectName);
            return await new System.Net.Http.HttpClient().GetStreamAsync(signedUrl);
        }
        
        /// <summary>
		/// Returns object details in JSON format.
		/// </summary>
		/// <exception cref="Autodesk.Forge.Client.ApiException">Thrown when fails to make API call</exception>
		/// <param name="bucketKey">URL-encoded bucket key</param>
		/// <param name="objectName">URL-encoded object name</param>
		/// <param name="ifModifiedSince">If the requested object has not been modified since the time specified in this field, an entity will not be returned from the server; instead, a 304 (not modified) response will be returned without any message body.  (optional)</param>
		/// <param name="with">Extra information in details; multiple uses are supported Acceptable values: &#x60;createdDate&#x60;, &#x60;lastAccessedDate&#x60;, &#x60;lastModifiedDate&#x60;  (optional)</param>
		/// <returns>Task of ObjectFullDetails</returns>
        public async Task<ObjectDetails> GetObjectDetailsAsync(string bucketKey, string objectName, DateTime? ifModifiedSince = null, string with = null)
        {
            var value = await this.ObjectsApi.GetObjectDetailsAsync(bucketKey, objectName, ifModifiedSince, with) as DynamicJsonResponse;
            return value.ToObject<ObjectDetails>();
        }
        /// <summary>
        /// Upload an object. If the specified object name already exists in the bucket, the uploaded content will overwrite the existing content for the bucket name/object name combination. 
        /// </summary>
        /// <exception cref="Autodesk.Forge.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="bucketKey">URL-encoded bucket key</param>
        /// <param name="objectName">URL-encoded object name</param>
        /// <param name="contentLength">Indicates the size of the request body.</param>
        /// <param name="body"></param>
        /// <param name="contentDisposition">The suggested default filename when downloading this object to a file after it has been uploaded. (optional)</param>
        /// <param name="ifMatch">If-Match header containing a SHA-1 hash of the bytes in the request body can be sent by the calling service or client application with the request. If present, OSS will use the value of If-Match header to verify that a SHA-1 calculated for the uploaded bytes server side matches what was sent in the header. If not, the request is failed with a status 412 Precondition Failed and the data is not written.  (optional)</param>
        /// <param name="contentType"></param>
        /// <returns>Task of ObjectDetails</returns>
        [Obsolete]
        public async Task<ObjectDetails> UploadObjectAsync(string bucketKey, string objectName, int? contentLength, Stream body, string contentDisposition = null, string ifMatch = null, string contentType = "application/octet-stream")
        {
            var value = await this.ObjectsApi.UploadObjectAsync(bucketKey, objectName, contentLength, body, contentDisposition, ifMatch, contentType) as DynamicJsonResponse;
            return value.ToObject<ObjectDetails>();
        }
        /// <summary>
        /// This endpoint allows resumable uploads for large files in chunks.
        /// </summary>
        /// <exception cref="Autodesk.Forge.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="bucketKey">URL-encoded bucket key</param>
        /// <param name="objectName">URL-encoded object name</param>
        /// <param name="contentLength">Indicates the size of the request body.</param>
        /// <param name="contentRange">Byte range of a segment being uploaded</param>
        /// <param name="sessionId">Unique identifier of a session of a file being uploaded</param>
        /// <param name="body"></param>
        /// <param name="contentDisposition">The suggested default filename when downloading this object to a file after it has been uploaded. (optional)</param>
        /// <param name="ifMatch">If-Match header containing a SHA-1 hash of the bytes in the request body can be sent by the calling service or client application with the request. If present, OSS will use the value of If-Match header to verify that a SHA-1 calculated for the uploaded bytes server side matches what was sent in the header. If not, the request is failed with a status 412 Precondition Failed and the data is not written.  (optional)</param>
        /// <param name="contentType"></param>
        /// <returns>Task of ObjectDetails</returns>
        [Obsolete]
        public async Task<ObjectDetails> UploadChunkAsync(string bucketKey, string objectName, int? contentLength, string contentRange, string sessionId, System.IO.Stream body, string contentDisposition = null, string ifMatch = null, string contentType = "application/octet-stream")
        {
            var value = await this.ObjectsApi.UploadChunkAsync(bucketKey, objectName, contentLength, contentRange, sessionId, body, contentDisposition, ifMatch, contentType) as DynamicJsonResponse;
            return value.ToObject<ObjectDetails>();
        }
        /// <summary>
		/// Copies an object to another object name in the same bucket.
		/// </summary>
		/// <exception cref="Autodesk.Forge.Client.ApiException">Thrown when fails to make API call</exception>
		/// <param name="bucketKey">URL-encoded bucket key</param>
		/// <param name="objectName">URL-encoded object name</param>
		/// <param name="newObjectName">URL-encoded Object key to use as the destination</param>
		/// <returns>Task of ObjectDetails</returns>
        [Obsolete]
        public async Task<ObjectDetails> CopyToAsync(string bucketKey, string objectName, string newObjectName)
        {
            var value = await this.ObjectsApi.CopyToAsync(bucketKey, objectName, newObjectName) as DynamicJsonResponse;
            return value.ToObject<ObjectDetails>();
        }
        /// <summary>
		///  Deletes an object from the bucket.
		/// </summary>
		/// <exception cref="Autodesk.Forge.Client.ApiException">Thrown when fails to make API call</exception>
		/// <param name="bucketKey">URL-encoded bucket key</param>
		/// <param name="objectName">URL-encoded object name</param>
		/// <returns>Task of void</returns>
        public async Task DeleteObjectAsync(string bucketKey, string objectName)
        {
            await this.ObjectsApi.DeleteObjectAsync(bucketKey, objectName);
        }
        #region Signed
        /// <summary>
        /// This endpoint creates a signed URL that can be used to download an object within the specified expiration time. Be aware that if the object the signed URL points to is deleted or expires before the signed URL expires, then the signed URL will no longer be valid. A successful call to this endpoint requires bucket owner access.
        /// </summary>
        /// <exception cref="Autodesk.Forge.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="bucketKey">URL-encoded bucket key</param>
        /// <param name="objectName">URL-encoded object name</param>
        /// <param name="postBucketsSigned">Body Structure</param>
        /// <param name="access">Access for signed resource Acceptable values: &#x60;read&#x60;, &#x60;write&#x60;, &#x60;readwrite&#x60;. Default value: &#x60;read&#x60;  (optional, default to read)</param>
        /// <param name="useCdn">If true, this will generate a CloudFront URL for the S3 object</param>
        /// <returns>Task of PostObjectSigned</returns>
        public async Task<PostObjectSigned> CreateSignedResourceAsync(string bucketKey, string objectName, PostBucketsSigned postBucketsSigned, string access = null, bool useCdn = true)
        {
            var value = await this.ObjectsApi.CreateSignedResourceAsync(bucketKey, objectName, postBucketsSigned, access, useCdn) as DynamicJsonResponse;
            return value.ToObject<PostObjectSigned>();
        }
        /*
        /// <summary>
		/// Download an object using a signed URL.
		/// </summary>
		/// <exception cref="Autodesk.Forge.Client.ApiException">Thrown when fails to make API call</exception>
		/// <param name="id">Id of signed resource</param>
		/// <param name="range">A range of bytes to download from the specified object. (optional)</param>
		/// <param name="ifNoneMatch">The value of this header is compared to the ETAG of the object. If they match, the body will not be included in the response. Only the object information will be included. (optional)</param>
		/// <param name="ifModifiedSince">If the requested object has not been modified since the time specified in this field, an entity will not be returned from the server; instead, a 304 (not modified) response will be returned without any message body.  (optional)</param>
		/// <param name="acceptEncoding">When gzip is specified, a gzip compressed stream of the object’s bytes will be returned in the response. Cannot use “Accept-Encoding:gzip” with Range header containing an end byte range. End byte range will not be honored if “Accept-Encoding: gzip” header is used.  (optional)</param>
		/// <param name="region">The region where the bucket resides Acceptable values: &#x60;US&#x60;, &#x60;EMEA&#x60; Default is &#x60;US&#x60;  (optional, default to US)</param>
		/// <returns>Task of System.IO.Stream</returns>
        public async Task<Stream> GetSignedResourceAsync(string id, string range = null, string ifNoneMatch = null, DateTime? ifModifiedSince = null, string acceptEncoding = null, string region = null)
        {
            var value = await this.ObjectsApi.GetSignedResourceAsync(id, range, ifNoneMatch, ifModifiedSince, acceptEncoding, region);
            return value as Stream;
        }
        /// <summary>
        /// Delete a signed URL. A successful call to this endpoint requires bucket owner access.
        /// </summary>
        /// <exception cref="Autodesk.Forge.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Id of signed resource</param>
        /// <param name="region">The region where the bucket resides Acceptable values: &#x60;US&#x60;, &#x60;EMEA&#x60; Default is &#x60;US&#x60;  (optional, default to US)</param>
        /// <returns>Task of void</returns>
        public async Task DeleteSignedResourceAsync(string id, string region = null)
        {
            await this.ObjectsApi.DeleteSignedResourceAsync(id, region);
        }
        */
        #endregion
        #endregion
    }
}