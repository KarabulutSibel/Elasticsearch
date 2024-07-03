using Elasticsearch.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elasticsearch.API.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class ECommerceController : ControllerBase
	{
		private readonly ECommerceRepository _eCommerceRepository;

		public ECommerceController(ECommerceRepository eCommerceRepository)
		{
			_eCommerceRepository = eCommerceRepository;
		}

		[HttpGet]
		public async Task<IActionResult> TermQuery(string customerFirstName)
		{
			return Ok(await _eCommerceRepository.TermQueryAsync(customerFirstName));
		}

		[HttpPost]
		public async Task<IActionResult> TermsQuery(List<string> customerFirstNameList)
		{
			return Ok(await _eCommerceRepository.TermsQueryAsync(customerFirstNameList));
		}

		[HttpGet]
		public async Task<IActionResult> PrefixQuery(string customerFullName)
		{
			return Ok(await _eCommerceRepository.PrefixQueryAsync(customerFullName));
		}

		[HttpGet]
		public async Task<IActionResult> RangeQuery(double fromPrice, double toPrice)
		{
			return Ok(await _eCommerceRepository.RangeQueryAsync(fromPrice, toPrice));
		}

		[HttpGet]
		public async Task<IActionResult> MatchAll()
		{
			return Ok(await _eCommerceRepository.MatchAllQueryAsync());
		}

		[HttpGet]
		public async Task<IActionResult> PaginationQuery(int page=1, int pageSize=5)
		{
			return Ok(await _eCommerceRepository.PaginationQueryAsync(page, pageSize));
		}

		[HttpGet]
		public async Task<IActionResult> WildCardQuery(string customerFullName)
		{
			return Ok(await _eCommerceRepository.WildCardQueryAsync(customerFullName));
		}

		[HttpGet]
		public async Task<IActionResult> FuzzyQuery(string customerFirstName)
		{
			return Ok(await _eCommerceRepository.FuzzyQueryAsync(customerFirstName));
		}

		[HttpGet]
		public async Task<IActionResult> MatchQueryFullText(string categoryName)
		{
			return Ok(await _eCommerceRepository.MatchQueryFullTextAsync(categoryName));
		}

		[HttpGet]
		public async Task<IActionResult> MatchBoolPrefixQueryFullText(string customerFullName)
		{
			return Ok(await _eCommerceRepository.MatchBoolPrefixQueryFullTextAsync(customerFullName));
		}

		[HttpGet]
		public async Task<IActionResult> MatchPhraseQueryFullText(string customerFullName)
		{
			return Ok(await _eCommerceRepository.MatchPhraseQueryFullTextAsync(customerFullName));
		}

		[HttpGet]
		public async Task<IActionResult> CompoundQueryExampleOne(string cityName, double taxfulTotalPrice, string categoryName, string manufacturer)
		{
			return Ok(await _eCommerceRepository.CompoundQueryExampleOneAsync(cityName, taxfulTotalPrice, categoryName, manufacturer));
		}

		[HttpGet]
		public async Task<IActionResult> CompoundQueryExampleTwo(string customerFullName)
		{
			return Ok(await _eCommerceRepository.CompoundQueryExampleTwoAsync(customerFullName));
		}

		[HttpGet]
		public async Task<IActionResult> MultiMatchQueryFullText(string name)
		{
			return Ok(await _eCommerceRepository.MultiMatchQueryFullTextAsync(name));
		}
	}
}
