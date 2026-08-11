using System.ComponentModel.DataAnnotations;

namespace DotNet_B2B_tradesphere.ViewModels;

public class ProductUpdateViewModel : ProductCreateViewModel
{
    public int Id { get; set; }

    [Display(Name = "Aktif")]
    public bool IsActive { get; set; } = true;
}
