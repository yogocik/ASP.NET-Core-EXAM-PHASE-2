using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookListRazor.Models
{
    public class ApplicationDBContext : DbContext
    {
        public DbSet<Books> books { get; set; }
        protected override void OnModelBuilder(ModelBuilder modelBuilder) => modelBuilder.UseNpgsql("Host=localhost;Database=booklist;Username=postgres;Password=darticecek12");
    }
}
