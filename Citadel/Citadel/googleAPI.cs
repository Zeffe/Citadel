/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Download;
using Google.Apis.Http;
using Google.Apis.Services;
using Google.Apis.Storage.v1;
using Google.Apis.Storage.v1.Data;
using Google.Apis.Auth;
using System.IO;
using System.Windows.Forms;
using System.Threading;

namespace Citadel
{
    public partial class Form1
    {

        void MainFunction(string[] args)
        {
            // Choose auth mechanism based on command-line flag.
            int flagIndex =
                Array.FindIndex(args, arg => "--askForCredentials" == arg);
            IConfigurableHttpClientInitializer credentials;
            if (flagIndex >= 0)
            {
                credentials = GetInstalledApplicationCredentials();
                var argList = new List<string>(args);
                argList.RemoveAt(flagIndex);
                args = argList.ToArray();
            }
            else
            {
                credentials = GetApplicationDefaultCredentials();
            }
            if (args.Length != 2)
            {
                //richTextBox1.AppendText(Usage);
            }
            else
            {
                Run(credentials, args[0], args[1]);
            }
        }

        private const int KB = 0x400;
        private const int DownloadChunkSize = 256 * KB;

        public IConfigurableHttpClientInitializer
            GetApplicationDefaultCredentials()
        {
            GoogleCredential credential =
                GoogleCredential.GetApplicationDefaultAsync().Result;
            if (credential.IsCreateScopedRequired)
            {
                credential = credential.CreateScoped(new[] {
                    StorageService.Scope.DevstorageReadWrite
                });
            }
            return credential;
        }

        public IConfigurableHttpClientInitializer
            GetInstalledApplicationCredentials()
        {
            var secrets = new ClientSecrets
            {
                // Replace these values with your own to use Installed
                // Application Credentials.
                // Pass --askForCredentials on the command line.
                // See https://developers.google.com/identity/protocols/OAuth2#installed
                ClientId = "52e283388f6f03608bbcc72bb3faac6a23b51aa2.apps.googleusercontent.com",
                ClientSecret = "YOUR_CLIENT_SECRET"
            };
            return GoogleWebAuthorizationBroker.AuthorizeAsync(
                secrets, new[] { StorageService.Scope.DevstorageFullControl },
                Environment.UserName, new CancellationTokenSource().Token)
                .Result;
        }

        public void Run(IConfigurableHttpClientInitializer credential,
            string projectId, string bucketName)
        {
            StorageService service = new StorageService(
                new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "GCS Sample",
                });

            richTextBox1.AppendText("List of buckets in current project");
            Buckets buckets = service.Buckets.List(projectId).Execute();

            foreach (var bucket in buckets.Items)
            {
                richTextBox1.AppendText(bucket.Name);
            }

            richTextBox1.AppendText("Total number of items in bucket: "
                + buckets.Items.Count);
            richTextBox1.AppendText("=============================");

            // using Google.Apis.Storage.v1.Data.Object to disambiguate from
            // System.Object
            Google.Apis.Storage.v1.Data.Object fileobj =
                new Google.Apis.Storage.v1.Data.Object()
                {
                    Name = "somefile.txt"
                };

            richTextBox1.AppendText("Creating " + fileobj.Name + " in bucket "
                + bucketName);
            byte[] msgtxt = Encoding.UTF8.GetBytes("Lorem Ipsum");

            service.Objects.Insert(fileobj, bucketName,
                new MemoryStream(msgtxt), "text/plain").Upload();

            richTextBox1.AppendText("Object created: " + fileobj.Name);

            richTextBox1.AppendText("=============================");

            richTextBox1.AppendText("Reading object " + fileobj.Name + " in bucket: "
                + bucketName);
            var req = service.Objects.Get(bucketName, fileobj.Name);
            Google.Apis.Storage.v1.Data.Object readobj = req.Execute();

            richTextBox1.AppendText("Object MediaLink: " + readobj.MediaLink);

            // download using Google.Apis.Download and display the progress
            string pathUser = Environment.GetFolderPath(
                Environment.SpecialFolder.UserProfile);
            var fileName = Path.Combine(pathUser, "Downloads") + "\\"
                + readobj.Name;
            richTextBox1.AppendText("Starting download to " + fileName);
            var downloader = new MediaDownloader(service)
            {
                ChunkSize = DownloadChunkSize
            };
            // add a delegate for the progress changed event for writing to
            // console on changes
            downloader.ProgressChanged += progress =>
                richTextBox1.AppendText(progress.Status + " "
                + progress.BytesDownloaded + " bytes");

            using (var fileStream = new System.IO.FileStream(fileName,
                System.IO.FileMode.Create, System.IO.FileAccess.Write))
            {
                var progress =
                    downloader.Download(readobj.MediaLink, fileStream);
                if (progress.Status == DownloadStatus.Completed)
                {
                    richTextBox1.AppendText(readobj.Name
                        + " was downloaded successfully");
                }
                else
                {
                }
            }
            richTextBox1.AppendText("=============================");
        }
    }
} */
