using Microsoft.EntityFrameworkCore;
using ShoopingWeb.Data;
using ShoopingWeb.Models;
using ShoopingWeb.Services.Interfaces;
using System.Net;
using System.Numerics;

namespace ShoopingWeb.Services.Implmentation
{
    public class File_Management:IFile_Management
    {
        private readonly ShopDbContext _context;


        public File_Management(ShopDbContext context)
        {
            _context = context;

        }



      
  
        public async Task<IEnumerable<product_Table>> GetDataByProduct_Table()
        {
            return await _context.product_Table
                .ToListAsync();
        }
        public async Task<IEnumerable<Customer>> GetDataByCustomer_Table()
        {
            return await _context.Customer_Table
                .ToListAsync();
        }


        public async Task<IEnumerable<supplier_Table>> GetDataBySupplier_Table()
        {
            return await _context.supplier_Table
                .ToListAsync();
        }


   



        public async Task Addproduct(string Name, string Product_code, string Product_type, string supplier_name,int Unit_price,int Count)
        {
            var categoryData = new product_Table
            {
                  Name = Name,
                  Product_code = Product_code,
                  Product_type = Product_type,
                    supplier_name = supplier_name,
                    Unit_price = Unit_price,
                    Count = Count

            };
            await _context.product_Table.AddAsync(categoryData);
            await _context.SaveChangesAsync();
        }


        public async Task AddCustomer_Table(string Name, string Phone, string Address)
        {
            var categoryData = new Customer
            {
                Name = Name,
                Phone = Phone,
                Address = Address

            };
            await _context.Customer_Table.AddAsync(categoryData);
            await _context.SaveChangesAsync();
        }
        public async Task AddSupplier_Table(string Name, string Supplier_phone, string Supplier_address)
        {
            var categoryData = new supplier_Table
            {
                Name = Name,
                Supplier_phone = Supplier_phone,
                Supplier_address = Supplier_address,
            };
            await _context.supplier_Table.AddAsync(categoryData);
            await _context.SaveChangesAsync();
        }
        public async Task AddStorehouse_Table(string Name, string storehouse_address, string Product_code,string Product_type, int Unit_price, int Count)
        {
            var categoryData = new storehouse_Table
            {
                Name = Name,
                storehouse_address = storehouse_address,
                Product_code = Product_code,
                Product_type = Product_type,
                Unit_price = Unit_price,
                Count = Count

            };
            await _context.storehouse_Table.AddAsync(categoryData);
            await _context.SaveChangesAsync();
        }


        public async Task Updateproduct_Table(int id, string name, string Product_code, string Product_type, string supplier_name, int Unit_price, int Count)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Category name is required.", nameof(name));
            }

            var category = await _context.product_Table.FindAsync(id);
            if (category == null) throw new Exception("Category not found");

            category.Name = name;
            category.Product_code = Product_code;
            category.Product_type = Product_type;
            category.supplier_name = supplier_name;
            category.Unit_price = Unit_price;
            category.Count = Count;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCustomer(int id, string name, string Phone, string Address)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Category name is required.", nameof(name));
            }
    

            var category = await _context.Customer_Table.FindAsync(id);
            if (category == null) throw new Exception("Category not found");

            category.Name = name;
            category.Phone = Phone;
            category.Address = Address;
            await _context.SaveChangesAsync();
        }


       

        public async Task Updatesupplier_Table(int id, string name, string Supplier_phone, string Supplier_address)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Category name is required.", nameof(name));
            }


            var category = await _context.supplier_Table.FindAsync(id);
            if (category == null) throw new Exception("Category not found");

            category.Name = name;
            category.Supplier_phone = Supplier_phone;
            category.Supplier_address = Supplier_address;
            await _context.SaveChangesAsync();
        }



        public async Task Updatestorehouse_Table(int id, string name, string storehouse_address, string Product_code, string Product_type, int Unit_price, int Count)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Category name is required.", nameof(name));
            }


            var category = await _context.storehouse_Table.FindAsync(id);
            if (category == null) throw new Exception("Category not found");

            category.Name = name;
            category.storehouse_address = storehouse_address;
            category.Product_code = Product_code;
            category.Product_type = Product_type;
            category.Unit_price = Unit_price;
            category.Count = Count;
            await _context.SaveChangesAsync();
        }
    }
}
