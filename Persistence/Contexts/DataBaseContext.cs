﻿using Application.Contexts.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Users;
using Domain.Attributes;

namespace Persistence.Contexts
{
    public class DataBaseContext:DbContext,IDataBaseContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> option):base(option)
        {
        }
 
        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach(var entityType in builder.Model.GetEntityTypes())
            {
                if(entityType.ClrType.GetCustomAttributes(typeof(AuditableAttribute), true).Length > 0)
                {
                    builder.Entity(entityType.Name).Property<DateTime?>("InsertTime");
                    builder.Entity(entityType.Name).Property<DateTime?>("UpdateTime");
                    builder.Entity(entityType.Name).Property<DateTime?>("RemoveTime");
                    builder.Entity(entityType.Name).Property<bool>("IsRemoved");
                }
            }
            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        { 
            var modifiedEntries = ChangeTracker.Entries()
                .Where(p=> p.State == EntityState.Modified ||
                p.State == EntityState.Deleted ||
                p.State == EntityState.Added);
               
            foreach(var item in modifiedEntries)
            {
                var entityType = item.Context.Model.FindEntityType(item.Entity.GetType());

                var inserted = entityType.FindProperty("InsertTime");
                var updated = entityType.FindProperty("UpdateTime");
                var removeTime = entityType.FindProperty("RemoveTime");
                var isRemoved = entityType.FindProperty("IsRemoved");

                if(item.State == EntityState.Added && inserted != null)
                {
                    item.Property("InsertTime").CurrentValue = DateTime.Now;                    
                }
                if(item.State == EntityState.Modified && updated != null)
                {
                    item.Property("UpdateTime").CurrentValue = DateTime.Now;
                }
                if(item.State == EntityState.Deleted && isRemoved != null && isRemoved != null)
                {
                    item.Property("RemoveTime").CurrentValue = DateTime.Now;
                    item.Property("IsRemoved").CurrentValue = true;
                }
            }
            return base.SaveChanges();
        }
    }
}