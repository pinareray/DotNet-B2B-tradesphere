using System.ComponentModel.DataAnnotations;

namespace DotNet_B2B_tradesphere.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Şirket adı zorunludur.")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "Şirket adı 2-150 karakter olmalıdır.")]
    [Display(Name = "Şirket Adı")]
    public string CompanyName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vergi numarası zorunludur.")]
    [RegularExpression(@"^\d{10,11}$", ErrorMessage = "Vergi numarası 10 veya 11 haneli olmalıdır.")]
    [Display(Name = "Vergi No")]
    public string TaxNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "E-posta zorunludur.")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi girin.")]
    [Display(Name = "E-posta")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Şifre zorunludur.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
    [DataType(DataType.Password)]
    [Display(Name = "Şifre")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Şifre tekrarı zorunludur.")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Şifreler eşleşmiyor.")]
    [Display(Name = "Şifre Tekrar")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
