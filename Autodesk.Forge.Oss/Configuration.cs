using Autodesk.Forge;
using Autodesk.Forge.Model;
using System;
using System.Threading.Tasks;

namespace Autodesk.Forge.Oss
{
    /// <summary>
    /// Represents a set of configuration settings
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// CreateDefault
        /// </summary>
        /// <returns></returns>
        public static Configuration CreateDefault()
        {
            return new Configuration();
        }

        /// <summary>
        /// Application ClientId 
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// Application ClientSecret 
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Data Management Scopes
        /// </summary>
        private readonly Scope[] SCOPES = new Scope[] {
            Scope.DataRead, Scope.DataWrite, Scope.DataCreate, Scope.DataSearch,
            Scope.BucketCreate, Scope.BucketRead, Scope.BucketUpdate, Scope.BucketDelete
        };

        /// <summary>
        /// GetConfiguration
        /// </summary>
        /// <returns></returns>
        public Autodesk.Forge.Client.Configuration GetConfiguration()
        {
            return new Autodesk.Forge.Client.Configuration() { Bearer = GetBearer() };
        }

        /// <summary>
        /// GetBearer
        /// </summary>
        /// <returns></returns>
        private Bearer GetBearer()
        {
            return GetBearerAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// GetBearerAsync
        /// <code>Based: https://github.com/Autodesk-Forge/forge-api-dotnet-client/blob/master/Sample/Program.cs</code>
        /// </summary>
        /// <returns></returns>
        private async Task<Bearer> GetBearerAsync()
        {
            string clientId = ClientId ?? Environment.GetEnvironmentVariable("FORGE_CLIENT_ID");
            string clientSecret = ClientSecret ?? Environment.GetEnvironmentVariable("FORGE_CLIENT_SECRET");

            if (string.IsNullOrEmpty(clientId))
                throw new Exception("'clientId' parameter is null or empty.");

            if (string.IsNullOrEmpty(clientSecret))
                throw new Exception("'clientSecret' parameter is null or empty.");

            var twoLeggedApi = new TwoLeggedApi();
            var bearer = await twoLeggedApi.AuthenticateAsyncWithHttpInfo(
                clientId, clientSecret,
                oAuthConstants.CLIENT_CREDENTIALS, SCOPES);

            return new Bearer(bearer);
        }
    }
}

