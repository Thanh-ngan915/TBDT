using Microsoft.EntityFrameworkCore;
using Shopping_Toturial.Models;

namespace Shopping_Toturial.Reponsitory
{
    public class SeedData
    {
        public static void SeedingData(DataContext _context)
        {
            _context.Database.Migrate();
            if (!_context.Products.Any())
            {
                CategoryModel apple = new CategoryModel
                {
                    Name = "Apple",
                    Slug = "apple",
                    Desciption = "Apple là một thương hiệu lớn trên thế giới ",
                    Status = 1

                };
                CategoryModel Samsung = new CategoryModel
                {
                    Name = "Samsung",
                    Slug = "samsung",
                    Desciption = "Samsung là một thương hiệu lớn trên thế giới ",
                    Status = 1

                };
                BrandModel dell = new BrandModel
                {
                    Name = "dell",
                    Slug = "dell",
                    Desciption = "dell là một thương hiệu lớn trên thế giới ",
                    Status = 1

                };
                BrandModel sony = new BrandModel
                {
                    Name = "sony",
                    Slug = "sony",
                    Desciption = "sony là một thương hiệu lớn trên thế giới ",
                    Status = 1

                };

                _context.Products.AddRange(
                    new ProductModel { Name = "Macbook", Slug = "Macbook", Desciption = "Macbook là một trong những thương hiệu nổi tiếng trên thế giới", Images = "gallery1.jpg", Category = apple, Brand = dell, Price = 12 },
                    new ProductModel { Name = "Pc", Slug = "Pc", Desciption = "Pc là một trong những thương hiệu nổi tiếng trên thế giới", Images = "gallery1.jpg", Category = apple, Brand = dell, Price = 12 }

                    );
                _context.SaveChanges();

            }
        }


    }
}
