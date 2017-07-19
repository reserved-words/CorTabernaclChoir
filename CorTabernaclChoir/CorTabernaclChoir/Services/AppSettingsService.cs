using System;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Xml.Linq;
using CorTabernaclChoir.Common.Services;

namespace CorTabernaclChoir.Services
{
    public class AppSettingsService : IAppSettingsService
    {
        private const string SettingElementName = "setting";
        private const string KeyAttributeName = "key";
        private const string ValueAttributeName = "value";
        private readonly string _configFilePath = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "PrivateSettings.config");

        private readonly Lazy<XElement> _configRoot;

        public AppSettingsService()
        {
            _configRoot = new Lazy<XElement>(GetConfigRoot);
        }

        private XElement GetConfigRoot()
        {
            return File.Exists(_configFilePath)
                ? XDocument.Load(_configFilePath).Element("settings")
                : null;
        }

        public int NumberOfItemsPerPage => 10;
        public string YouTubeApiKey => GetValue("YouTubeApiKey", "");
        public string YouTubeChannelId => GetValue("YouTubeChannelId", "");
        public int NumberOfVideosToDisplay => 8;
        public int NumberOfNewsItemsInSidebar => 3;
        public int NumberOfEventsInSidebar => 3;

        private string GetValue(string key, string defaultValue)
        {
            if (_configRoot.Value == null)
                return defaultValue;

            return _configRoot.Value.Elements(SettingElementName)
                .Single(s => s.Attribute(KeyAttributeName).Value == key)
                .Attribute(ValueAttributeName).Value;
        }
    }
}
