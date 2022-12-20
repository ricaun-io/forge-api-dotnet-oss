using System;

namespace Autodesk.Forge.Oss.Tests
{
    public class OssClientFactory
    {
        public static Autodesk.Forge.Oss.OssClient CreateDefault()
        {
            return new Autodesk.Forge.Oss.OssClient(new()
            {
                //ClientId = Environment.GetEnvironmentVariable("FORGE_CLIENT_ID"),
                //ClientSecret = Environment.GetEnvironmentVariable("FORGE_CLIENT_SECRET"),
            });
        }
    }

    public class TestFactory
    {
        public static string CreateBucketKey()
        {
            return Guid.NewGuid().ToString();
        }
        public static string CreateObjectName()
        {
            return Guid.NewGuid().ToString();
        }
        public static string CreateDataString()
        {
            return Guid.NewGuid().ToString();
        }
    }
}