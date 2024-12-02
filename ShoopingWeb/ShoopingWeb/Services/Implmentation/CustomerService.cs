using Microsoft.EntityFrameworkCore;
using ShoopingWeb.Data;
using ShoopingWeb.Models;
using ShoopingWeb.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoopingWeb.Services.Implmentation
{
    public class CustomerService : ICustomerService
    {
        private readonly ShopDbContext _context;

        public CustomerService(ShopDbContext context)
        {
            _context = context;
        }

        // 查询所有
        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            try
            {
                return await _context.Customer_Table.ToListAsync();
            }
            catch (Exception ex)
            {
                // 记录日志
                throw new Exception("Error occurred while fetching data", ex);
            }
        }

        // 按主键查询单条记录
        public async Task<Customer> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Customer_Table.FindAsync(id);
            }
            catch (Exception ex)
            {
                // 记录日志
                throw new Exception($"Error occurred while fetching customer with id {id}", ex);
            }
        }

        // 新增记录
        public async Task AddAsync(Customer customer)
        {
            try
            {
                if (customer == null) throw new ArgumentNullException(nameof(customer)); // 确保实体对象不为 null
                await _context.Customer_Table.AddAsync(customer);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // 记录日志
                throw new Exception("Error occurred while adding customer", ex);
            }
        }

        // 更新记录
        public async Task UpdateAsync(Customer customer)
        {
            try
            {
                if (customer == null) throw new ArgumentNullException(nameof(customer));
                _context.Customer_Table.Update(customer);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // 记录日志
                throw new Exception("Error occurred while updating customer", ex);
            }
        }

        // 删除记录
        public async Task DeleteAsync(Customer customer)
        {
            try
            {
                if (customer == null) throw new ArgumentNullException(nameof(customer));
                _context.Customer_Table.Remove(customer);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // 记录日志
                throw new Exception("Error occurred while deleting customer", ex);
            }
        }
    }
}
