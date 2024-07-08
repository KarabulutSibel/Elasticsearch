using Elasticsearch.WEB.Repositories;
using Elasticsearch.WEB.ViewModels;

namespace Elasticsearch.WEB.Services
{
	public class ECommerceService
	{
		private readonly ECommerceRepository _repository;

		public ECommerceService(ECommerceRepository repository)
		{
			_repository = repository;
		}

		public async Task<(List<ECommerceViewModel>, long totalCount, long pageLinkCount)> SearchAsync(ECommerceSearchViewModel searchModel, int page, int pageSize)
		{
			var (eCommerceList, totalCount) = await _repository.SearchAsync(searchModel, page, pageSize);

			var pageLinkCountCalculate = totalCount % pageSize;

			long pageLinkCount = 0;

			if (pageLinkCountCalculate == 0)
			{
				pageLinkCount = totalCount / pageSize;
			}
			else
			{
				pageLinkCount = (totalCount / pageSize) + 1;
			}

			var eCommerceListViewModel = eCommerceList.Select(x => new ECommerceViewModel()
			{
				Id = x.Id,
				Category = String.Join(",", x.Category),
				CustomerFirstName = x.CustomerFirstName,
				CustomerLastName = x.CustomerLastName,
				CustomerFullName = x.CustomerFullName,
				OrderId = x.OrderId,
				OrderDate = x.OrderDate.ToShortDateString(),
				TaxfulTotalPrice = x.TaxfulTotalPrice,
				Gender = x.Gender.ToLower()
			}).ToList();

			return (eCommerceListViewModel, totalCount, pageLinkCount);
		}
	}
}
