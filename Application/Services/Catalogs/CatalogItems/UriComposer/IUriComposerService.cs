using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Catalogs.CatalogItems.UriComposer
{
    public interface IUriComposerService
    {
        string ComposeImageUri(string src);
    }

    public class UriComposerService : IUriComposerService
    {
        public IConfiguration Configuration { get; }
        public UriComposerService(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public string ComposeImageUri(string src)
        {
            if(src == null)
            {
                return null;
            }
            return Configuration["StaticFileDomain"] + src.Replace("\\", "//");
        }
    }

}
