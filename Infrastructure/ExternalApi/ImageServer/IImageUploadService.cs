using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ExternalApi
{
    public interface IImageUploadService
    {
        List<string> Upload(List<IFormFile> files);
    }
    public class ImageUploadService : IImageUploadService
    {
        public List<string> Upload(List<IFormFile> files)
        {
            var client = new RestClient("https://localhost:7281/api/Images?apikey=mysecretkey");
            var request = new RestRequest();
            request.Method = Method.Post;

            foreach (var item in files)
            {
                byte[] bytes;
                using(var ms = new MemoryStream())
                {
                    item.CopyToAsync(ms);
                    bytes = ms.ToArray();
                }
                request.AddFile(item.FileName, bytes, item.FileName, item.ContentType);
            }

            Task<RestResponse> response = client.ExecuteAsync(request);
            UploadDto upload = JsonConvert.DeserializeObject<UploadDto>(response.Result.Content);
            return upload.FileNameAddress;
        }
    }

    public class UploadDto
    {
        public bool Status { get; set; }
        public List<string> FileNameAddress { get; set; }
    }
}
