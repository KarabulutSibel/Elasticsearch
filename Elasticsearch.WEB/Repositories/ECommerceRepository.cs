using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Elasticsearch.WEB.Models;
using Elasticsearch.WEB.ViewModels;

namespace Elasticsearch.WEB.Repositories
{
	public class ECommerceRepository
	{
		private readonly ElasticsearchClient _elasticsearchClient;
		private const string indexName = "kibana_sample_data_ecommerce";

		public ECommerceRepository(ElasticsearchClient elasticsearchClient)
		{
			_elasticsearchClient = elasticsearchClient;
		}

		public async Task<(List<ECommerce> list, long count)> SearchAsync(ECommerceSearchViewModel searchViewModel, int page, int pageSize)
		{
			List<Action<QueryDescriptor<ECommerce>>> listQuery = new();

			if (searchViewModel is null)
			{
				listQuery.Add(q => q.MatchAll(m => { }));
				return await CalculateResultSet(page, pageSize, listQuery);
			}

			if (!string.IsNullOrEmpty(searchViewModel.Category))
			{
				listQuery.Add((q) => q.Match(m => m.Field(f => f.Category).Query(searchViewModel.Category)));
			}

			if (!string.IsNullOrEmpty(searchViewModel.CustomerFullName))
			{
				listQuery.Add((q) => q.Match(m => m.Field(f => f.CustomerFullName).Query(searchViewModel.CustomerFullName)));
			}

			if (searchViewModel.OrderDateStart.HasValue)
			{
				listQuery.Add((q) => q.Range(r => r.DateRange(dr => dr.Field(f => f.OrderDate).Gte(searchViewModel.OrderDateStart.Value))));
			}

			if (searchViewModel.OrderDateEnd.HasValue)
			{
				listQuery.Add((q) => q.Range(r => r.DateRange(dr => dr.Field(f => f.OrderDate).Lte(searchViewModel.OrderDateEnd.Value))));
			}

			if (!string.IsNullOrEmpty(searchViewModel.Gender))
			{
				listQuery.Add((q) => q.Term(t => t.Field(f => f.Gender).Value(searchViewModel.Gender).CaseInsensitive()));
			}

			if (!listQuery.Any())
			{
				listQuery.Add(q => q.MatchAll(m => { }));
			}

			return await CalculateResultSet(page, pageSize, listQuery);
		}

		private async Task<(List<ECommerce> list, long count)> CalculateResultSet(int page, int pageSize, List<Action<QueryDescriptor<ECommerce>>> listQuery)
		{
			var pageFrom = (page - 1) * pageSize;

			var result = await _elasticsearchClient.SearchAsync<ECommerce>(s => s.Index(indexName).Size(pageSize).From(pageFrom).Query(q => q.Bool(b => b.Must(listQuery.ToArray()))));

			foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

			return (list: result.Documents.ToList(), result.Total);
		}
	}
}
