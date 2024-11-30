using Microsoft.AspNetCore.Mvc;

namespace ProjectMVC.Controllers
{
    public class ErrorController : Controller
    {

		public IActionResult Error(){

			ViewBag.ErrorMessage = "An unexpected error occurred Please try again later";

			if(ViewBag.ErrorMessage != null)
			{
				ModelState.AddModelError(string.Empty, ViewBag.ErrorMessage.ToString());
			}

			return View();

		}

    }
}
