using ShoopingWeb.Models;

namespace ShoopingWeb.Services.Interfaces
{
    public interface IFile_Management
    {
        Task<IEnumerable<product_Table>> GetDataByProduct_Table();
        Task<IEnumerable<Customer>> GetDataByCustomer_Table();
        Task<IEnumerable<supplier_Table>> GetDataBySupplier_Table();
        Task<IEnumerable<storehouse_Table>> GetDataByStorehouse_Table();




        Task Addproduct(string Name, string Product_code, string Product_type, string supplier_name, int Unit_price, int Count);






        Task AddCustomer_Table(string Name, string Phone, string Address);
        Task AddSupplier_Table(string Name, string Supplier_phone, string Supplier_address);
        Task AddStorehouse_Table(string Name, string Product_code, string storehouse_address, string Product_type, int Unit_price, int Count);





        Task UpdateCustomer(int id, string name, string Phone, string Address);

        Task Updateproduct_Table(int id, string name, string Product_code, string Product_type, string supplier_name, int Unit_price, int Count);

        Task Updatesupplier_Table(int id, string name, string Supplier_phone, string Supplier_address);

        Task Updatestorehouse_Table(int id, string name, string storehouse_address, string Product_code, string Product_type, int Unit_price, int Count);

    }
}
