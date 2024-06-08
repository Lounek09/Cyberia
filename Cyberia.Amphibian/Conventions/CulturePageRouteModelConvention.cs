using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Cyberia.Amphibian.wwwroot;

public sealed class CulturePageRouteModelConvention : IPageRouteModelConvention
{
    public void Apply(PageRouteModel model)
    {
        foreach (var selector in model.Selectors)
        {
            var attributeRouteModel = selector.AttributeRouteModel;
            if (attributeRouteModel != null)
            {
                attributeRouteModel.Template = "{culture?}/" + attributeRouteModel.Template;
            }
        }
    }
}
