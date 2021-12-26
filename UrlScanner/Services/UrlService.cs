using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
//using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace UrlScanner.Services
{
    public class UrlService
    {
        public static List<string> DomainExtentions;
        public UrlService()
        {
            string json = File.ReadAllText("domainext.json");
            DomainExtentions = JsonConvert.DeserializeObject<List<string>>(json);
        }
        public string UrlFromTextService(string text)
        {
            text = text.Replace("\"", " ").Replace("</"," ");
            List<string> urls = new List<string>();
            var linkParser = new Regex(@"\b((?:(?:https?):\/\/)|[\w/\-?=%.]+\.)\S+\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Uri uriResult;
            foreach (Match m in linkParser.Matches(text))
            {
                //if (CheckValidUrl(m.Value, out uriResult))
                //{
                //    urls.Add(CheckUrlScheme(m.Value));
                //}
                if (CheckUrl(m.Value))
                {
                    urls.Add(CheckUrlScheme(m.Value));
                }
            }
            return JsonConvert.SerializeObject(urls.Distinct().ToList()); 
        }
        private bool CheckValidUrl(string url, out Uri resultURI)
        {
            try
            {
                if (!Regex.IsMatch(url, @"^https?:\/\/", RegexOptions.IgnoreCase))
                    url = "http://" + url;
                if (Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out resultURI))
                {
                    var uri = new Uri(url);
                    var hostsplit = uri.Host.Split('.');
                    string ext = hostsplit[hostsplit.Length - 1];
                    if (DomainExtentions.Contains(ext.ToUpper()))
                        return true;
                }
                return false;
            }
            catch (Exception)
            {
                resultURI = null;
                return false;
            }
            
        }
        private string CheckUrlScheme(string url)
        {
            if (!Regex.IsMatch(url, @"^https?:\/\/", RegexOptions.IgnoreCase))
                url = "http://" + url;
            return url;
        }

        private bool CheckUrl(string url)
        {
            try
            {
                if (!Regex.IsMatch(url, @"^https?:\/\/", RegexOptions.IgnoreCase))
                    url = "http://" + url;
                url = HttpUtility.UrlEncode(url);
                bool isUri = Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute);
                url = HttpUtility.UrlDecode(url);
                if (isUri)
                {
                    var uri = new Uri(url);
                    var host = uri.Host;
                    var splittxt = host.Split('.');
                    string ext = splittxt[splittxt.Length - 1];
                    if (DomainExtentions.Contains(ext.ToUpper()))
                        return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
            
        }
    }
}
