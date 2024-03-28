using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.ViewComponents
{
    public class ItemList:ViewComponent
    {
        public IViewComponentResult Invoke(List<foodItem> data)
        {

            ViewBag.Count = data.Count;
            ViewBag.Total = data.Sum(i => i.ItemTotal);

            return View(data);
        }


    }
}
