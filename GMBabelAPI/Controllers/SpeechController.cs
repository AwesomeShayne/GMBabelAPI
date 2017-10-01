using GMBabelAPI.Models;
using System;
using System;
using Amazon.Polly;
using Amazon.Polly.Model;
using System.IO;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Net.Http.Headers;

namespace GMBabelAPI.Controllers
{
    
    public class SpeechController : ApiController
    {
        List<Speech> speeches = new List<Speech> {  };

        [HttpGet]
        public IEnumerable<Speech> GetAllSpeeches()
        {
            return speeches;
        }

        [HttpGet]
        public HttpResponseMessage GetSpeech(string id)
        {
            var speech = speeches.FirstOrDefault((p) => p.Id == id);
            if (speech == null)
            {
                
            }

            var stream = new MemoryStream();
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(stream.ToArray())
            };
            result.Content.Headers.ContentDisposition =
                new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                {
                    FileName = speech.Dir
                };
            result.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/octet-stream");

            return result;
            
        }

        [HttpPost]
        public HttpResponseMessage Post(string _text, string _language, string _voiceid, string _inflection, string _id)
        {
            Speech _speech = new Speech(_language, _voiceid, _inflection, _text, _id);
            speeches.Add(_speech);
            var stream = new MemoryStream();
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(stream.ToArray())
            };
            result.Content.Headers.ContentDisposition =
                new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                {
                    FileName = _speech.Dir
                };
            result.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/octet-stream");

            return result;
        }


    }
}
