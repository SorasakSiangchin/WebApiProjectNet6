using Microsoft.EntityFrameworkCore;

namespace BackEnd.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) 
        {

        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<AddressInformation> AddressInformations { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<CategoryProduct> CategoryProducts { get; set; }
        public DbSet<Delivery> Deliverys { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<OrderCustomer> OrderCustomers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductAdd> ProductAdds { get; set; }
        public DbSet<ProductDescription> ProductDescriptions { get; set; }
        public DbSet<ProductList> ProductLists { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ReviewImage> ReviewImages { get; set; }
        public DbSet<StatusDelivery> StatusDeliveries { get; set; }
        public DbSet<CustomerPassword> CustomerPasswords { get; set; }
        public DbSet<CartCustomer> CartCustomers { get; set; }
        public DbSet<StatusAddress> StatusAddress { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<SellerMustConfrim> SellerMustConfirms { get; set; }
        public DbSet<ProofOfPaymentCancel> ProofOfPaymentCancels { get; set; }
        

    }
}
