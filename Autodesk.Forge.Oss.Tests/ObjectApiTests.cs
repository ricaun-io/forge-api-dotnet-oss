using Autodesk.Forge.Client;
using Autodesk.Forge.Oss;
using NUnit.Framework;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Autodesk.Forge.Oss.Tests
{
    public class ObjectApiTests
    {
        private static Autodesk.Forge.Oss.OssClient OssClient;
        private static string BucketKey;
        private static string ObjectName;
        private static string DataString;

        public ObjectApiTests()
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
        public async Task ObjectApi_UploadAndDelete()
        {
            var fileDetail = await OssClient.UploadFileAsync(BucketKey, ObjectName, ObjectName);
            Assert.AreEqual(DataString.Length, fileDetail.Size);

            var objectDetails = await OssClient.GetObjectDetailsAsync(BucketKey, ObjectName);
            Assert.AreEqual(DataString.Length, objectDetails.Size);

            await OssClient.DeleteObjectAsync(BucketKey, ObjectName);
            Assert.ThrowsAsync<ApiException>(() => OssClient.GetObjectDetailsAsync(BucketKey, ObjectName));
        }

        [Test]
        public async Task ObjectApi_UploadAndUpload()
        {
            await OssClient.UploadFileAsync(BucketKey, ObjectName, ObjectName);
            for (int i = 2; i < 5; i++)
            {
                File.AppendAllText(ObjectName, DataString);
                var fileDetail = await OssClient.UploadFileAsync(BucketKey, ObjectName, ObjectName);
                Assert.AreEqual(DataString.Length * i, fileDetail.Size);
            }
        }

        [Test]
        public async Task ObjectApi_UploadGetObjects()
        {
            await OssClient.UploadFileAsync(BucketKey, ObjectName, ObjectName);
            await OssClient.GetObjectDetailsAsync(BucketKey, ObjectName);
            var objects = await OssClient.GetObjectsAsync(BucketKey);
            var count = objects.Items.Count;
            Console.WriteLine(objects.ToJson());

            if (count == 0)
                Assert.Ignore("Sometimes the count is 0, probably some delay in the Autodesk server.");

            Assert.AreEqual(1, count);

            foreach (var objectDetails in objects.Items)
            {
                Assert.AreEqual(BucketKey, objectDetails.BucketKey);
                Assert.AreEqual(ObjectName, objectDetails.ObjectKey);
            }
        }

        [Test]
        public async Task ObjectApi_UploadGetObject()
        {
            var fileDetail = await OssClient.UploadFileAsync(BucketKey, ObjectName, ObjectName);
            Assert.AreEqual(DataString.Length, fileDetail.Size);

            var stream = await OssClient.GetObjectAsync(BucketKey, ObjectName);
            var dataString = await new StreamReader(stream).ReadToEndAsync();
            Console.WriteLine(dataString);
            Assert.AreEqual(DataString, dataString);
        }

        [Test]
        public async Task ObjectApi_UploadCopyObject()
        {
            await OssClient.UploadFileAsync(BucketKey, ObjectName, ObjectName);
            var copyObjectName = TestFactory.CreateObjectName();
            var fileDetail = await OssClient.CopyToAsync(BucketKey, ObjectName, copyObjectName);
            Assert.AreEqual(copyObjectName, fileDetail.ObjectKey);
            Console.WriteLine(fileDetail.ObjectKey);
        }

        [Test]
        public async Task ObjectApi_UploadDownload()
        {
            await OssClient.UploadFileAsync(BucketKey, ObjectName, ObjectName);
            var downloadObjectName = TestFactory.CreateObjectName();
            await OssClient.DownloadFileAsync(BucketKey, ObjectName, downloadObjectName);
            var downloadDataString = File.ReadAllText(downloadObjectName);
            Assert.AreEqual(DataString, downloadDataString);
            File.Delete(downloadObjectName);
        }

        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(8)]
        [TestCase(10)]
        public async Task ObjectApi_UploadBigFile(int mbSize)
        {
            var fileSize = CreateDumpFile(ObjectName, mbSize);
            Console.WriteLine($"Create and Upload: {fileSize}");
            var fileDetail = await OssClient.UploadFileAsync(BucketKey, ObjectName, ObjectName);
            Console.WriteLine(fileDetail.ToJson());
            Assert.AreEqual(fileSize, fileDetail.Size);
        }

        [Test]
        public async Task ObjectApi_SignedCreate()
        {
            await OssClient.UploadFileAsync(BucketKey, ObjectName, ObjectName);

            var signedUrl = await OssClient.CreateSignedFileAsync(BucketKey, ObjectName);
            var getStringSignedUrl = await GetStringAsync(signedUrl);
            Assert.AreEqual(DataString, getStringSignedUrl);
        }

        [Test]
        public async Task ObjectApi_SignedWriteWithPut()
        {
            await OssClient.UploadFileAsync(BucketKey, ObjectName, ObjectName);
            var writeDataString = TestFactory.CreateDataString();
            var signedUrl = await OssClient.CreateSignedFileWriteAsync(BucketKey, ObjectName);
            var result = await PutStringAsync(signedUrl, writeDataString);
            Console.WriteLine(result);

            var stream = await OssClient.GetObjectAsync(BucketKey, ObjectName);
            var dataString = await new StreamReader(stream).ReadToEndAsync();
            Assert.AreNotEqual(DataString, dataString);
            Assert.AreEqual(writeDataString, dataString);
        }

        private async Task<string> GetStringAsync(string requestUri)
        {
            using (var client = new HttpClient())
            {
                return await client.GetStringAsync(requestUri);
            }
        }

        private async Task<string> PutStringAsync(string requestUri, string content)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PutAsync(requestUri, new StringContent(content));
                var bytes = await response.Content.ReadAsByteArrayAsync();
                var responseString = System.Text.Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                return responseString;
            }
        }

        private int CreateDumpFile(string fileName, int mbSize)
        {
            if (File.Exists(fileName))
                File.Delete(fileName);

            var fileSize = mbSize * 1024 * 1024;
            var fs = new FileStream(fileName, FileMode.CreateNew);
            fs.Seek(fileSize, SeekOrigin.Begin);
            fs.WriteByte(0);
            fs.Close();
            return fileSize + 1;
        }
    }
}