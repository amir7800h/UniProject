using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Sevices.GoogleRecaptchaService
{
    public class GoogleRecaptcha
    {
        public bool Verify(string googleResponse)
        {
            string sec = "6LdokY4eAAAAAOI43TmQsJ41op8uJghQFDYV8zqT";
            HttpClient client = new HttpClient();
            var result = client.PostAsync($"https://www.google.com/recaptcha/api/siteverify?secret={sec}&response={googleResponse}", null).Result;
            if(result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return false;
            }

            string content = result.Content.ReadAsStringAsync().Result;
            dynamic jsonData = JObject.Parse(content);
            if(jsonData.success == "true")
            {
                return true;
            }

            return false;
        }
    }
}
