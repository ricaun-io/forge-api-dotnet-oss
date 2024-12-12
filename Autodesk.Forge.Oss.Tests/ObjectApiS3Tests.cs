using NUnit.Framework;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Autodesk.Forge.Oss.Tests
{
    public class ObjectApiS3Tests
    {
        private static Autodesk.Forge.Oss.OssClient OssClient;
        private static string BucketKey;
        private static string ObjectName;
        private static string DataString;

        public ObjectApiS3Tests()
        {
            OssClient = OssClientFactory.CreateDefault();
        }
        [SetUp]
        public async Task ObjectApi_CreateBucket()
        {
            BucketKey = TestFactory.CreateBucketKey();
            ObjectName = TestFactory.CreateObjectName();
            DataString = TestFactory.CreateDataString();
            File.WriteAllText(ObjectName, DataString);
            await OssClient.CreateBucketAsync(BucketKey);
        }

        [TearDown]
        public async Task ObjectApi_DeleteBucket()
        {
            File.Delete(ObjectName);
            await OssClient.DeleteBucketAsync(BucketKey);
        }
        [Test]
        public async Task ObjectApi_GetS3UploadURL()
        {
            var result = await OssClient.GetS3UploadURLAsync(BucketKey, ObjectName);
            Console.WriteLine(result);
        }
    }
}