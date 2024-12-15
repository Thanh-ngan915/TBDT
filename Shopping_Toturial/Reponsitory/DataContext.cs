using Microsoft.EntityFrameworkCore;
using Shopping_Toturial.Models;

namespace Shopping_Toturial.Reponsitory
{
    public class DataContext : DbContext

    {
        // ????????????????
        // hồi chiều hỏi xong cho coppy đoạn này qua sai đấy
        // sửa bục mặt
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }


        public DbSet<BrandModel> Brands { get; set; }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
    }
}
