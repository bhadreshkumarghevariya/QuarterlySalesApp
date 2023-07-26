using System;
using QuarterlySalesApp.Models;
using Microsoft.EntityFrameworkCore;
namespace QuarterlySalesApp.Data
{
	public class ApplicationDbContext  : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{

		}

		public DbSet<Employee> Employees { get; set; }
		public DbSet<Sale> Sales { get; set; }
	}
}

