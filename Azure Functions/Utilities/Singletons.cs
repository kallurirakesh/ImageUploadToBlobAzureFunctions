using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Text;

namespace Azure_Functions.Utilities
{
    public class Singletons
    {
        private static CloudBlobClient _cloudBlobClient;
        public static CloudBlobClient CloudBlobClient
        {
            get
            {
                if (_cloudBlobClient == null)
                {
                    var connectionString = Environment.GetEnvironmentVariable("BlobConeectionString");
                    _cloudBlobClient = CloudStorageAccount.Parse(connectionString).CreateCloudBlobClient();
                }
                return _cloudBlobClient;
            }
        }
    }
}
