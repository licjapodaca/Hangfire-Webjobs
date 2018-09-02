using Newtonsoft.Json;
using System;

namespace Webjobs.API.Models
{
	public class ErrorDetails
	{
		public int StatusCode { get; set; }
		public string Message { get; set; }
		public string StackTrace { get; set; }
		public ErrorDetails InnerException { get; set; }

		public override string ToString()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}
