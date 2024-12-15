using System.ComponentModel.DataAnnotations;

namespace Shopping_Toturial.Models
{
    public class ProductModel
    {
        [Key]
        public int Id { get; set; }
        [Required, MinLength(4, ErrorMessage = "Yêu cầu nhập tên sản phẩm ")]
        public string Name { get; set; }
public string Slug { get; set; }
        [Required, MinLength(4, ErrorMessage = "Yêu cầu nhập mô tả sản phẩm  ")]
        
        public string Desciption { get; set; }
        [Required, MinLength(4, ErrorMessage = "Yêu cầu nhập giá sản phẩm ")]
        public String Images {  get; set; }
        public decimal Price { get; set; }
        [Required]
        public int BrandId { get; set; }
public int  CategoryId { get; set; }
        public  CategoryModel Category { get; set; }
        public BrandModel Brand { get; set; }




        
    }
}
