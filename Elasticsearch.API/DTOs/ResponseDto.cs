using System.Net;

namespace Elasticsearch.API.DTOs
{
	public record ResponseDto<T>
	{
		public T? Data { get; set; }
		public List<string>? Errors { get; set; }
		public HttpStatusCode StatusCode { get; set; }

		//Static Factory Method
		public static ResponseDto<T> Success(T data, HttpStatusCode statusCode)
		{
			return new ResponseDto<T> { Data = data, StatusCode = statusCode };
		}

		public static ResponseDto<T> Fail(List<string> errors, HttpStatusCode statusCode) 
		{
			return new ResponseDto<T> {  Errors = errors, StatusCode = statusCode };
		}

		public static ResponseDto<T> Fail(string error, HttpStatusCode statusCode)
		{
			return new ResponseDto<T> {Errors=new List<string> {error}, StatusCode = statusCode};
		}
	}
}
