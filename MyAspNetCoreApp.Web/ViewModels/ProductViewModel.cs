using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MyAspNetCoreApp.Web.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Remote(action:"HasProductName", controller:"Product")]
        [Required(ErrorMessage = "Ürün ismi boş olamaz.")]
        [StringLength(maximumLength:50, MinimumLength = 2, 
            ErrorMessage = "Ürün ismi 2 ile 50 karakter arasında olmalıdır.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Ürün fiyatı boş olamaz.")]
        [Range(1, 1000, ErrorMessage = "Ürün fiyat aralığı 1 ile 1000 arasında olmalıdır.")]
        public decimal? Price { get; set; }

        [Required(ErrorMessage = "Ürün adedi boş olamaz.")]
        [Range(1,200, ErrorMessage = "Ürün adedi aralığı 1 ile 200 arasında olmalıdır.")]
        public int? Stock { get; set; }

        [Required(ErrorMessage = "Ürün açıklaması boş olamaz.")]
        [StringLength(maximumLength:500,MinimumLength =2,
            ErrorMessage = "Ürün açıklaması 2 ile 500 karakter arasında olmalıdır.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Ürün renk seçimi boş olamaz.")]
        public string? Color { get; set; }

        [Required(ErrorMessage = "Ürün yayınlanma tarihi boş olamaz.")]
        public DateTime? PublishDate { get; set; }

        public bool isPublish { get; set; }

        [Required(ErrorMessage = "Ürün yayınlanma süresi boş olamaz.")]
        public int? Expire { get; set; }
    }
}
