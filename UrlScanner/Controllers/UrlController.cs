using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UrlScanner.Services;


namespace UrlScanner.Controllers
{

    [ApiController]
    public class UrlController : ControllerBase
    {
        [Route("urlscanner")]
        public string Get()
        {
            return  "Welcome to Url Scanner";
        }

        [Route("api/scanurl")]
        [HttpPost]
        public async Task<string> ScanUrl()
        {
            try
            {
                using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
                {
                    string text = await reader.ReadToEndAsync();
                    UrlService objService = new UrlService();
                    if (!string.IsNullOrEmpty(text))
                    {
                        return objService.UrlFromTextService(text);
                    }
                    else
                    {
                        return null;
                    }
                }
                
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
