using System;
using ChildrenCheckIn.Models;
using Microsoft.EntityFrameworkCore;

namespace ChildrenCheckIn
{
    public class ChildContext : DbContext
    {
        public ChildContext(DbContextOptions<ChildContext> options) : base(options)
        {
        }

        public DbSet<Child> Children { get; set; }
    }
}
