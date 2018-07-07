using Study.OData.Api.Models;
using Study.OData.Client.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.OData;

namespace Study.OData.Api.Controllers
{
	public class ThreadsController : ODataController
	{
		[EnableQuery]
		// GET api/<controller>
		public IHttpActionResult Get()
		{
			return Ok(DataModel.Threads);
		}

		// GET api/<controller>/5
		public string Get(int id)
		{
			return "value";
		}

		// POST api/<controller>
		public HttpResponseMessage Post([FromBody]string value)
		{
			return Request.CreateErrorResponse(HttpStatusCode.MethodNotAllowed, "to be implemented");
		}

		// PUT api/<controller>/5
		public HttpResponseMessage Put(int id, [FromBody]string value)
		{
			return Request.CreateErrorResponse(HttpStatusCode.MethodNotAllowed, "to be implemented");
		}

		// DELETE api/<controller>/5
		public HttpResponseMessage Delete(int id)
		{
			return Request.CreateErrorResponse(HttpStatusCode.MethodNotAllowed, "to be implemented");
		}
	}
}