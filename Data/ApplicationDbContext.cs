using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Rocky.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options ):base (options)
    {
        
    }

    public DbSet<Models.Category> Category { get; set; }
    public DbSet<Models.Article> Article { get; set; }  
    public DbSet<Models.Product> Product { get; set; }  
}

