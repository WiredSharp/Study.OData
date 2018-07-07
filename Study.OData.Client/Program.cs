using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;
using Microsoft.OData.Edm.Validation;
using Newtonsoft.Json;
using Study.OData.Api.Models;
using Study.OData.Client.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.OData.Builder;
using System.Xml;

namespace Study.OData.Client
{
	class Program
	{
		static void Main(string[] args)
		{
			using (var client = new HttpClient(new TraceHandler(new HttpClientHandler()), true))
			{
				StringContent content = new StringContent("");
				content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
				var request = new HttpRequestMessage(HttpMethod.Get, "http://this") { Content = content };
				Console.WriteLine("GET...");
				client.SendAsync(request).Wait();
				Console.WriteLine("POST...");
				client.PostAsync("http://this", content).Wait();
			}
		}

		private static void CreateModels()
		{
			IEdmModel model = EdmBuilder.NewNonConventionalBuildModel();
			EdmBuilder.WriteCsdl(model, "non-conventional.xml");
			model = EdmBuilder.NewConventionalBuildModel();
			EdmBuilder.WriteCsdl(model, "conventional.xml");
		}
	}

	class TraceHandler : DelegatingHandler
	{
		public TraceHandler(HttpMessageHandler inner)
			:base(inner)
		{
		}

		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			Console.WriteLine("Request headers:");
			Console.WriteLine(JsonConvert.SerializeObject(request.Headers));
			Console.WriteLine("Request Content headers:");
			Console.WriteLine(JsonConvert.SerializeObject(request.Content.Headers));
			return Task.FromResult(new HttpResponseMessage());
		}
	}
}
