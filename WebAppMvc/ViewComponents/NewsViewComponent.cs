using Microsoft.AspNetCore.Mvc;

namespace WebAppMvc.ViewComponents
{
    public class NewsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int count)
        {
            return Content($"Hello {count}");
        }
    }
}