using Shopping_Toturial.Reponsitory.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopping_Toturial.Models
{
    public class ProductModel
    {
        [Key] public long Id { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập tên sản phẩm.")]
        
        // [Required, MinLength(4, ErrorMessage = "Yêu cầu nhập tên sản phẩm ")]
        public string Name { get; set; }

        public string Slug { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập mô tả sản phẩm.")]
        public string Desciption { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập giá sản phẩm.")]
      
        
     
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập giá sản phẩm.")]
        public int BrandId { get; set; }
        [Required ,Range(1,int.MaxValue,ErrorMessage = "Chọn 1 danh mục")] 
        public int CategoryId { get; set; }
      
        public BrandModel Brand { get; set; }
        public CategoryModel Category { get; set; }
        public String Images { get; set; }
        
        [NotMapped]
        [FileExtension]
        public IFormFile?ImageUpload { get; set; }
    }
}