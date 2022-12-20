using NUnit.Framework;
using System;
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