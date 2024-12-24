using Microsoft.EntityFrameworkCore;
using ShoopingWeb.Models;

namespace ShoopingWeb.Data
{
    public class ShopDbContext:DbContext
    {
        public ShopDbContext(DbContextOptions<ShopDbContext> 
            options):base(options)
        {

        }

        //----------------------------------------------------
        /// <summary>
        ///档案
        /// </summary>
        public DbSet<storehouse_Table> storehouse_Table { get; set; }
        public DbSet<supplier_Table> supplier_Table { get; set; }
        public DbSet<Customer> Customer_Table { get; set; }
        public DbSet<product_Table> product_Table { get; set; }

        //----------------------------------------------------



        /// <summary>
        /// 采购
        /// </summary>
        public DbSet<purchase_orders> purchase_orders { get; set; }
        public DbSet<order_products> order_products { get; set; }




        /// <summary>
        /// 应付明细表
        /// </summary>
        public DbSet<AP_Detail> AP_Detail { get; set; }

        /// <summary>
        /// 应收明细表
        /// </summary>
        public DbSet<AR_Detail> AR_Detail { get; set; }


        ///销售表
        public DbSet<sales_orders> sales_orders { get; set; }

        public DbSet<order_sales_Table> order_sales_Table { get; set; }



        public DbSet<Category> Categoryise { get; set; }
        public DbSet<User> user_Table { get; set; }


        public DbSet<Role> Role_Table { get; set; }
        public DbSet<Permission> Permission_Table { get; set; }


        public DbSet<RolePermission> RolePermissions { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<UserPermission> UserPermissions { get; set; }


 

        // 定义每个实体的 DbSet
      
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryData> CategoryData { get; set; }
        public DbSet<View_CategoryData> View_CategoryData_WithCategoryName { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            // 配置角色与权限的多对多关系
            modelBuilder.Entity<RolePermission>()
                .HasKey(rp => new { rp.RoleId, rp.PermissionId });

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId);

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId);

            // 配置多对多关系
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserPermission>()
                .HasKey(up => new { up.UserId, up.PermissionId });

            modelBuilder.Entity<RolePermission>()
                .HasKey(rp => new { rp.RoleId, rp.PermissionId });

        



            modelBuilder.Entity<UserRole>()
                .ToTable("UserRoles");


            modelBuilder.Entity<Category>()
                 .ToTable("Categories");

            // 配置外键
            modelBuilder.Entity<CategoryData>()
                .HasOne(cd => cd.Category)
                .WithMany()
                .HasForeignKey(cd => cd.CategoryId);





        }

      


    }
}
