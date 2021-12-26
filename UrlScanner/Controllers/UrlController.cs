using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlScanner.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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

        [Route("api/urlfromtext")]
        [HttpPost]
        public string UrlFromText([FromBody] TextModel objText)
        {
            try
            {
                UrlService objService = new UrlService();
                if (!string.IsNullOrEmpty(objText.Text))
                {
                    return objService.UrlFromTextService(objText.Text);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [Route("api/urlfromtextplaintext")]
        [HttpPost]
        public async Task<string> UrlFromTextPlainText()
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
