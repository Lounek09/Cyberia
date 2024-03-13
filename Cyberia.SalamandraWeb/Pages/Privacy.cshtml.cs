using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cyberia.SalamandraWeb.Pages;

public class PrivacyModel : PageModel
{
    private readonly ILogger<PrivacyModel> _logger;

    public PrivacyModel(ILogger<PrivacyModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        _logger.LogInformation("Privacy page requested");
    }
}

