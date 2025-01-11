﻿using System.ComponentModel.DataAnnotations;

namespace Shopping_Toturial.Models
{
    public class CategoryModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Yêu cầu nhập tên Danh mục ")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập mô tả Danh mục ")]
        public string Desciption{ get; set; }
        // [Required]
        public string Slug { get; set; }

        public int Status { get; set; } 
    }
}