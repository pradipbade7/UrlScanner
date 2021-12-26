using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace UrlScanner.Services
{
    public class UrlService
    {
        private static List<string> DomainExtentions;
        public UrlService()
        {
            string json = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "Extentions/domainext.json"));
            DomainExtentions = JsonConvert.DeserializeObject<List<string>>(json);
        }
        public string UrlFromTextService(string text)
        {
            text = text.Replace("\"", " ").Replace("</", " ").Replace(");", " ");
            List<string> urls = new List<string>();
            var linkParser = new Regex(@"\b((?:(?:https?):\/\/)|[\w/\-?=%.]+\.)\S+\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            foreach (Match m in linkParser.Matches(text))
            {
                if (CheckUrl(m.Value, out string urltxt))
                {
                    urls.Add(urltxt);
                }
            }
            return JsonConvert.SerializeObject(urls.Distinct().ToList());
        }
        private bool CheckUrl(string url, out string urltxt)
        {
            try
            {
                #region CheckEmail
                const string EmailPattern = @"(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
                 + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
                 + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
                 + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})";

                Regex remail = new Regex(EmailPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                if (remail.IsMatch(url))
                {
                    urltxt = null;
                    return false;
                }
                #endregion
                if (!Regex.IsMatch(url, @"^https?:\/\/", RegexOptions.IgnoreCase))
                    url = "http://" + url;
                url = HttpUtility.UrlEncode(url);
                bool isUri = Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute);
                urltxt = url = HttpUtility.UrlDecode(url);
                if (isUri)
                {
                    var uri = new Uri(url);
                    var host = uri.Host;
                    string[] splittxt = host.Split('.');
                    string ext = splittxt[splittxt.Length - 1];
                    if (DomainExtentions.Contains(ext.ToUpper()))
                        return true;
                }
                return false;
            }
            catch (Exception)
            {
                urltxt = null;
                return false;
            }

        }

    }
}
