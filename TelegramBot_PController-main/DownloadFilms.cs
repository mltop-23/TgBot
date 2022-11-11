using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace PController
{
    public class DownloadFilms
    {
        public static async Task<string> HtmlRequest(string requestUrl)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Method", "Get");
            httpClient.DefaultRequestHeaders.Add("KeepAlive", "false");
            httpClient.DefaultRequestHeaders.Add("UserAgent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.174 YaBrowser/22.1.2.834 Yowser/2.5 Safari/537.36");
            var response = await httpClient.GetStringAsync(requestUrl);
            return response;
        }

        private static HtmlDocument GetHtmlDocument(string url)
        {
            var doc = new HtmlDocument();
            string html = HtmlRequest(url).Result;
            doc.LoadHtml(html);

            return doc;
        }


        public static string GetLink(string name)
        {
            var link = "";
            var download = "";

            while (link.Length == 0 || download.Length == 0)
            {
                try
                {
                    var url = FindUrl(name);

                    Thread.Sleep(new Random().Next(100, 150));

                    link = OpenFilm(url);

                    download = Download($"https:{link}");
                }
                catch (Exception)
                {
                    Console.WriteLine("Эх...");
                    Thread.Sleep(new Random().Next(100, 150));
                    continue;
                }
            }

            return GetDownloadLink(download);
        }

        private static string Download(string url)
        {
            var doc = GetHtmlDocument(url);

            var a = doc.GetElementbyId("files");
            return a.GetAttributeValue("value", "");
        }

        private static string GetDownloadLink(string encryptedLink)
        {
            var result = "";

            for (int i = 0; i < encryptedLink.Length - 7; i++)
            {
                if (encryptedLink.Substring(i, 7) == "[1080p]")
                {
                    result = encryptedLink.Substring(i + 11);
                    break;
                }
            }

            var count = 0;
            var endIndex = 0;
            for (int i = 0; i < result.Length - 2; i++)
            {
                if (result.Substring(i, 2) == @"\/")
                {
                    count++;
                    if (count == 4)
                    {
                        endIndex = i;
                        break;
                    }
                }
            }

            result = $"https://{result.Substring(0, endIndex).Replace(@"\/", @"/")}/720.mp4";

            return result;
        }

        private static string OpenFilm(string url)
        {
            string result = "";

            var doc = GetHtmlDocument(url.Replace("lordfilm.vet", "r.greenfilm.vip"));

            var a = doc.DocumentNode.SelectNodes("//div[@class='tabs-b video-box']//iframe");

            foreach (var b in a)
            {
                var link = b.GetAttributeValue("src", "");
                if (link.Contains("greenfilm"))
                {
                    result = link;
                }
            }

            return result;
        }
        private static string FindUrl(string name)
        {
            string result = "";

            var url = $"https://yandex.ru/search/?text={name.Replace(" ", "+")}&lr=213&clid=2270455&win=495&src=suggest_Pers";
            var doc = GetHtmlDocument(url);

            var a = doc.DocumentNode.SelectNodes("//div[@class='Path Organic-Path path organic__path']//a");

            foreach (var b in a)
            {
                var link = b.GetAttributeValue("href", "");
                if (link.Contains("lordfilm"))
                {
                    result = link;
                    break;
                }
            }

            return result;
        }
    }
}
