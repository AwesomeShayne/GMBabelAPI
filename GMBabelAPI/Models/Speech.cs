using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Amazon.Polly;
using Amazon.Polly.Model;
using System.Configuration;


namespace GMBabelAPI.Models
{
    public class Speech
    {
        AmazonPollyClient AWSPollyClient;
        SynthesizeSpeechRequest sreq;
        SynthesizeSpeechResponse sres;

        string Language { get; set; }
        string Voiceid { get; set; }
        string Inflection { get; set; }
        string Text { get; set; }
        string Sound { get; set; }
        public string Id { get; set; }
        public string Dir { get; set; }

        public Speech (string _language, string _voiceid, string _inflection, string _text, string _id)
        {

            Language = _language;
            Voiceid = _voiceid;
            Inflection = _inflection;
            Text = _text;
            Id = _id;

            string access = ConfigurationManager.AppSettings["Access"].ToString();
            string secret = ConfigurationManager.AppSettings["Secret"].ToString();
            AWSPollyClient = new AmazonPollyClient(access, secret, Amazon.RegionEndpoint.USWest2);
            sreq = new SynthesizeSpeechRequest();
            sreq.OutputFormat = OutputFormat.Mp3;
            sreq.VoiceId = VoiceId.Amy;
            sreq.TextType = TextType.Text;

            Dir = String.Concat("//mp3//",Id,"//");

            generateVoice();
        }

        private void generateVoice()
        {
            sreq.Text = Text;
            sres = AWSPollyClient.SynthesizeSpeech(sreq);

            string fileName = mp3Name();
            using (var fileStream = File.Create(Dir + fileName))
            {
                sres.AudioStream.CopyTo(fileStream);
                fileStream.Flush();
                fileStream.Close();
            }
            Sound = fileName;

        }

        private string mp3Name()
        {
            if (!Directory.Exists(Dir))
            {
                Directory.CreateDirectory(Dir);
            }
            string[] fileArray = Directory.GetFiles(Dir);

            if (fileArray.Length < 1)
            {
                return "0001";
            }
            else
            {
                Array.Sort(fileArray);
                string tempString = fileArray[fileArray.Length - 1];
                tempString = tempString.Split('\\')[1];
                tempString = tempString.Remove(tempString.Length - 4);
                int tempInteger = Int32.Parse(tempString) + 1;
                string returnString = tempInteger.ToString();
                return returnString;
            }
        }

    }
}
