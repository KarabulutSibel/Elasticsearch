using Elasticsearch.WEB.Services;
using Elasticsearch.WEB.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Elasticsearch.WEB.Controllers
{
	public class ECommerceController : Controller
	{
		private readonly ECommerceService _eCommerceService;

		public ECommerceController(ECommerceService eCommerceService)
		{
			_eCommerceService = eCommerceService;
		}

		public async Task<IActionResult> Search([FromQuery] SearchPageViewModel searchPageViewModel)
		{
			var (eCommerceList,totalCount,pageLinkCount) = await _eCommerceService.SearchAsync(searchPageViewModel.SearchViewModel, searchPageViewModel.Page, searchPageViewModel.PageSize);

			searchPageViewModel.List = eCommerceList;
			searchPageViewModel.TotalCount = totalCount;
			searchPageViewModel.PageLinkCount = pageLinkCount;

			return View(searchPageViewModel);
		}
	}
}
