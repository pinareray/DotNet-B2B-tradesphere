using System.ComponentModel.DataAnnotations;

namespace DotNet_B2B_tradesphere.ViewModels;

public class PaymentViewModel
{
    [Required(ErrorMessage = "Kart sahibi adı zorunludur.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Kart sahibi adı 3-100 karakter olmalıdır.")]
    [Display(Name = "Kart Üzerindeki İsim")]
    public string CardHolderName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Kart numarası zorunludur.")]
    [RegularExpression(@"^\d{16}$", ErrorMessage = "Kart numarası 16 haneli olmalıdır.")]
    [Display(Name = "Kart Numarası")]
    public string CardNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Son kullanma tarihi zorunludur.")]
    [RegularExpression(@"^(0[1-9]|1[0-2])\/\d{2}$", ErrorMessage = "Geçerli bir tarih girin (MM/YY).")]
    [Display(Name = "Son Kullanma Tarihi")]
    public string ExpirationDate { get; set; } = string.Empty;

    [Required(ErrorMessage = "CVV zorunludur.")]
    [StringLength(3, MinimumLength = 3, ErrorMessage = "CVV 3 haneli olmalıdır.")]
    [RegularExpression(@"^\d{3}$", ErrorMessage = "CVV yalnızca rakam içermelidir.")]
    [Display(Name = "CVV")]
    public string CVV { get; set; } = string.Empty;
}
