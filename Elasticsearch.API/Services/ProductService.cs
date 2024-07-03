using Elastic.Clients.Elasticsearch;
using Elasticsearch.API.DTOs;
using Elasticsearch.API.Models;
using Elasticsearch.API.Repositories;
using System.Collections.Immutable;
using System.Net;

namespace Elasticsearch.API.Services
{
	public class ProductService
	{
		private readonly ProductRepository _productRepository;
		private readonly ILogger<ProductService> _logger;

		public ProductService(ProductRepository productRepository, ILogger<ProductService> logger)
		{
			_productRepository = productRepository;
			_logger = logger;
		}

		public async Task<ResponseDto<ProductDto>> SaveAsync(ProductCreateDto request)
		{
			var responseProduct = await _productRepository.SaveAsync(request.CreateProduct());
			if (responseProduct == null)
			{
				return ResponseDto<ProductDto>.Fail(new List<string> { "Kayıt esnasında bir hata meydana geldi." }, HttpStatusCode.InternalServerError);
			}

			return ResponseDto<ProductDto>.Success(responseProduct.CreateDto(), HttpStatusCode.Created);

		}

		public async Task<ResponseDto<List<ProductDto>>> GetAllAsync()
		{
			var products = await _productRepository.GetAllAsync();
			var productListDto = new List<ProductDto>();

			foreach (var x in products)
			{
				if (x.Feature is null)
				{
					productListDto.Add(new ProductDto(x.Id, x.Name, x.Price, x.Stock, null));
					continue;
				}

				productListDto.Add(new ProductDto(x.Id, x.Name, x.Price, x.Stock, new ProductFeatureDto(x.Feature.Width, x.Feature.Height, x.Feature.Color.ToString())));
			}

			return ResponseDto<List<ProductDto>>.Success(productListDto, HttpStatusCode.OK);

		}

		public async Task<ResponseDto<ProductDto>> GetByIdAsync(string id)
		{
			var hasProduct = await _productRepository.GetByIdAsync(id);

			if (hasProduct == null) return ResponseDto<ProductDto>.Fail("Ürün bulunamadı.", HttpStatusCode.NotFound);

			return ResponseDto<ProductDto>.Success(hasProduct.CreateDto(),HttpStatusCode.OK);
		}

		public async Task<ResponseDto<bool>> UpdateAsync(ProductUpdateDto productUpdateDto)
		{
			var isSuccess=await _productRepository.UpdateAsync(productUpdateDto);

			if (!isSuccess)
				return ResponseDto<bool>.Fail(new List<string> { "Güncelleme esnasında bir hata meydana geldi." }, HttpStatusCode.InternalServerError);

			return ResponseDto<bool>.Success(true, HttpStatusCode.NoContent);
		}

		public async Task<ResponseDto<bool>> DeleteAsync(string id)
		{
			var deleteResponse = await _productRepository.DeleteAsync(id);

			if(!deleteResponse.IsValidResponse && deleteResponse.Result==Result.NotFound)
				return ResponseDto<bool>.Fail(new List<string> { "Silmeye çalıştığınız ürün bulunamamıştır." }, HttpStatusCode.NotFound);

			if (!deleteResponse.IsValidResponse)
			{
				deleteResponse.TryGetOriginalException(out Exception? exception);
				_logger.LogError(exception, deleteResponse.ElasticsearchServerError.Error.ToString());

				return ResponseDto<bool>.Fail(new List<string> { "Silme esnasında bir hata meydana geldi." }, HttpStatusCode.InternalServerError);
			}
			return ResponseDto<bool>.Success(true,HttpStatusCode.NoContent);
		}
	}
}