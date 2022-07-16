using System.Threading.Tasks;

namespace Demo.Application.Shared.Interfaces;

public interface IRazorViewRenderer
{
    Task<string> RenderViewAsync<TModel>(string viewName, TModel model);
}
