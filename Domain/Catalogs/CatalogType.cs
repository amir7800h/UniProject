using Domain.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Catalogs
{
    [Auditable]
    public class CatalogType
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int? ParentCatalogTypeId { get; set; }
        public CatalogType ParentCatalogType { get; set; }  
        public ICollection<CatalogType> SubType { get; set; }

    }

    [Auditable]
    public class CatalogItem
    {

    }

    [Auditable]
    public class CatalogBrand
    {
        public int Id { get; set; }
        public string Brand { get; set; }
    }
}
