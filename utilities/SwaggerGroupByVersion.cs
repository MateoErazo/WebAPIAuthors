using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace WebAPIAuthors.utilities
{
  public class SwaggerGroupByVersion : IControllerModelConvention
  {
    public void Apply(ControllerModel controller)
    {
      string namespaceController = controller.ControllerType.Namespace;
      var versionAPI = namespaceController.Split('.').Last().ToLower();
      controller.ApiExplorer.GroupName = versionAPI;

    }
  }
}
