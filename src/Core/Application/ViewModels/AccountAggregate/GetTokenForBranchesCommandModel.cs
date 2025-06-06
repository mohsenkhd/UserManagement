using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels.AccountAggregate;

public class GetTokenForBranchesCommandModel
{
    [Required]
    public string AppName { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
}
