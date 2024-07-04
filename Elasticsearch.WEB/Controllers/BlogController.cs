using Elasticsearch.WEB.Models;
using Elasticsearch.WEB.Services;
using Elasticsearch.WEB.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Elasticsearch.WEB.Controllers
{
	public class BlogController : Controller
	{
		private readonly BlogService _blogService;

		public BlogController(BlogService blogService)
		{
			_blogService = blogService;
		}

		public async Task<IActionResult> Search()
		{
			return View(await _blogService.SearchAsync(string.Empty));
		}

		[HttpPost]
		public async Task<IActionResult> Search(string searchText)
		{
			ViewBag.SearchText = searchText;
			return View(await _blogService.SearchAsync(searchText));
		}

		public IActionResult Save()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Save(BlogCreateViewModel model)
		{
			var isSuccess = await _blogService.SaveAsync(model);

			if (!isSuccess)
			{
				TempData["result"] = "Kayıt başarısız";
				return RedirectToAction(nameof(BlogController.Save));
			}

			TempData["result"] = "Kayıt başarılı";
			return RedirectToAction(nameof(BlogController.Save));
		}
	}
}
