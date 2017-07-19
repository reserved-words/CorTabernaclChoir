using System.Collections.Generic;
using System.Net.Http;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Common.ViewModels;
using Newtonsoft.Json.Linq;

namespace CorTabernaclChoir.Services
{
    public class YouTubeService : IYouTubeService
    {
        private const string YouTubeApiUrlFormat = "https://www.googleapis.com/youtube/v3/search?part=snippet&channelId={0}&key={1}&maxResults={2}&order=date&type=video";
        private const string YouTubeVideoUrlFormat = "https://www.youtube.com/embed/{0}";

        public List<Video> GetVideos(string channelId, string apiKey, int maxResults)
        {
            if (string.IsNullOrEmpty(channelId) || string.IsNullOrEmpty(apiKey))
                return new List<Video>();

            var result = GetResponse(channelId, apiKey, maxResults);

            return result.IsSuccessStatusCode
                ? ParseYouTubeResponse(result.Content.ReadAsStringAsync().Result)
                : new List<Video>();
        }
        
        private static HttpResponseMessage GetResponse(string channelId, string apiKey, int maxResults)
        {
            var url = string.Format(YouTubeApiUrlFormat, channelId, apiKey, maxResults);

            using (var client = new HttpClient())
            {
                return client.GetAsync(url).Result;
            }
        }

        private static List<Video> ParseYouTubeResponse(string response)
        {
            var videos = new List<Video>();

            dynamic parsed = JObject.Parse(response);

            var items = parsed.items;

            foreach (var item in items)
            {
                if (item.id.videoId == null)
                    continue;

                var video = new Video
                {
                    Url = string.Format(YouTubeVideoUrlFormat, item.id.videoId),
                    Published = item.snippet.publishedAt,
                    Title = item.snippet.title
                };

                videos.Add(video);
            }

            return videos;
        }
    }
}
