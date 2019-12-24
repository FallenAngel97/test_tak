using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SigmaTestTask.Models
{
    public class OrderContext : IdentityDbContext<SigmaUser>
    {
        public OrderContext()
        {
            Database.SetInitializer<OrderContext>(new OrderInitializer());
        }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductInCart> ProductsInCart { get; set; }
    }


    class OrderInitializer : DropCreateDatabaseAlways<OrderContext> 
    {
        protected override void Seed(OrderContext context)
        {
            Product jeans = new Product() { Price = 10, ProductName = "Jeans" };
            Product sweater = new Product() { Price = 5.5, ProductName = "Sweater" };
            Product glasses = new Product() { Price = 10.5, ProductName = "Glasses" };
            Product pants = new Product() { Price = 8, ProductName = "Pants" };
            Product tshirt = new Product() { Price = 12, ProductName = "T-Shirt" };
            context.Products.Add(jeans);
            context.Products.Add(sweater);
            context.Products.Add(glasses);
            context.Products.Add(pants);
            context.Products.Add(tshirt);

            SigmaUser sigmaUser1 = new SigmaUser() { UserName = "Alex", Email="alex@mail.com", PasswordHash = new PasswordHasher().HashPassword("sigma user 1"), PersonalDiscount = 0.15f };
            SigmaUser sigmaUser2 = new SigmaUser() { UserName = "Joe", Email="joe@mail.com", PasswordHash = new PasswordHasher().HashPassword("sigma user 2"), PersonalDiscount = 0.3f };

            context.Users.Add(sigmaUser1);
            context.Users.Add(sigmaUser2);

            context.SaveChanges();
            base.Seed(context);
        }
    }
}