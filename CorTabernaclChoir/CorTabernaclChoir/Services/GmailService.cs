using System.Collections.Generic;
using CorTabernaclChoir.Common.Services;
using System.IO;
using System.Linq;
using System.Threading;
using CorTabernaclChoir.Common;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace CorTabernaclChoir.Services
{
    public class GmailService : IEmailService
    {
        // If modifying these scopes, delete previously saved credentials at _dataStorePath
        private static readonly string[] Scopes = { Google.Apis.Gmail.v1.GmailService.Scope.GmailReadonly };
        private static readonly string ApplicationName = Resources.Title;

        private readonly string _clientSecretPath;
        private readonly string _dataStorePath;

        public GmailService(string clientSecretPath, string dataStorePath)
        {
            _clientSecretPath = clientSecretPath;
            _dataStorePath = dataStorePath;
        }

        public List<string> GetForwardingAddresses()
        {
            var service = new Google.Apis.Gmail.v1.GmailService(new BaseClientService.Initializer
            {
                HttpClientInitializer = GetUserCredential(),
                ApplicationName = ApplicationName,
            });

            var request = service.Users.Settings.ForwardingAddresses.List("me");
            var addresses = request.Execute().ForwardingAddresses;

            return addresses.Select(a => a.ForwardingEmail).ToList();
        }

        private UserCredential GetUserCredential()
        {
            using (var stream = new FileStream(_clientSecretPath, FileMode.Open, FileAccess.Read))
            {
                return GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(_dataStorePath, true)
                ).Result;
            }
        }
    }
}