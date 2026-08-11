using System.ComponentModel.DataAnnotations;

namespace DotNet_B2B_tradesphere.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "E-posta veya vergi numarası zorunludur.")]
    [Display(Name = "E-posta veya Vergi No")]
    public string EmailOrTaxNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Şifre zorunludur.")]
    [DataType(DataType.Password)]
    [Display(Name = "Şifre")]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Beni Hatırla")]
    public bool RememberMe { get; set; }
}
