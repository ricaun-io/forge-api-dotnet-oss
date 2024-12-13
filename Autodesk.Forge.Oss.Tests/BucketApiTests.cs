using Autodesk.Forge.Client;
using NUnit.Framework;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Autodesk.Forge.Oss.Tests
{
    public class BucketApiTests
    {
        private static Autodesk.Forge.Oss.OssClient OssClient;
        private static string BucketKey;

        public BucketApiTests()
        {
            OssClient = OssClientFactory.CreateDefault();
            BucketKey = TestFactory.CreateBucketKey();
        }

        [Test]
        public async Task BucketApi_Show()
        {
            var buckets = await OssClient.GetBucketsAsync();
            Console.WriteLine(buckets.ToJson());
        }

        [Test]
        public async Task BucketApi_CreateAndDelete()
        {
            var bucketCreated = await OssClient.CreateBucketAsync(BucketKey);
            Assert.AreEqual(BucketKey, bucketCreated.BucketKey);
            Assert.IsNotNull(await OssClient.GetBucketDetailsAsync(BucketKey));
            await OssClient.DeleteBucketAsync(BucketKey);
            Assert.IsNull(await OssClient.TryGetBucketDetailsAsync(BucketKey));
        }

        [TestCase("US")]
        [TestCase("EMEA")]
        [TestCase("AUS")]
        public async Task BucketApi_CreateAndDelete_Region(string region)
        {
            var bucketKey = $"{BucketKey}_{region}".ToLower();
            var bucketCreated = await OssClient.CreateBucketAsync(bucketKey, region: region);
            Assert.AreEqual(bucketKey, bucketCreated.BucketKey);
            Assert.IsNotNull(await OssClient.GetBucketDetailsAsync(bucketKey));


            // Test file in the region
            {
                var ObjectName = TestFactory.CreateObjectName();
                var DataString = TestFactory.CreateDataString();
                File.WriteAllText(ObjectName, DataString);
                var fileDetail = await OssClient.UploadFileAsync(bucketKey, ObjectName, ObjectName);
                Assert.AreEqual(DataString.Length, fileDetail.Size);

                var objectDetails = await OssClient.GetObjectDetailsAsync(bucketKey, ObjectName);
                Assert.AreEqual(DataString.Length, objectDetails.Size);

                await OssClient.DeleteObjectAsync(bucketKey, ObjectName);
                Assert.ThrowsAsync<ApiException>(() => OssClient.GetObjectDetailsAsync(bucketKey, ObjectName));

                File.Delete(ObjectName);
            }

            await OssClient.DeleteBucketAsync(bucketKey);
            Assert.IsNull(await OssClient.TryGetBucketDetailsAsync(bucketKey));
        }

        [Ignore("Skip Delete Buckets")]
        [Test]
        public async Task BucketApi_DeleteAll()
        {
            var buckets = await OssClient.GetBucketsAsync();
            foreach (var bucket in buckets.Items)
            {
                await OssClient.DeleteBucketAsync(bucket.BucketKey);
            }
        }
    }
}