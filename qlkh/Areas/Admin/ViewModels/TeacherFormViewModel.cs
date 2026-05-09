using System.ComponentModel.DataAnnotations;

namespace qlkh.Areas.Admin.ViewModels
{
    public class TeacherFormViewModel
    {
        [Required(ErrorMessage = "Vui long nhap ma giang vien" )]
        [Display(Name = "Ma giang vien")]
        [StringLength(5)]
        public string TeacherId {get; set;} = string.Empty;

        [Required(ErrorMessage = "Vui long chon tai khoan")]
        [Display(Name = "Tai khoan")]
        public long AccountId{get; set;}

        [Required(ErrorMessage = "Vui long nhap so dien thoai")]
        [Display(Name = "So dien thoai")]
        [StringLength(15)]
        public string Phone {get; set;} = string.Empty;

        [Display(Name = "Chuyen mon")]
        [StringLength(150)]
        public string? Specialization {get; set;} 
        
        [Display(Name = "Gioi thieu")]
        [StringLength(1000)]
        public string? Bio {get; set;}
    }
}