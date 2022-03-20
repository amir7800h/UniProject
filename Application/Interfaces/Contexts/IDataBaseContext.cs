using Domain.Catalogs;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contexts.Interfaces
{
    public interface IDataBaseContext
    {

        DbSet<CatalogType> CatalogTypes { get; set; }
        DbSet<CatalogBrand> CatalogBrands { get; set; }
        DbSet<CatalogItem> CatalogItems { get; set; }
        DbSet<CatalogItemFeature> CatalogItemFeatures { get; set; }
        DbSet<CatalogItemImage> CatalogItemImages { get; set; }
        int SaveChanges();
        int SaveChanges(bool acceptAllChangesOnSuccess);
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}
