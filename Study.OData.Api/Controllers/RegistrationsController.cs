using Study.OData.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.OData;

namespace Study.OData.Api.Controllers
{
	public class RegistrationsController : ODataController
	{
		[HttpGet]
		[EnableQuery]
		// GET api/<controller>
		public IHttpActionResult Get()
		{
			return Ok(DataModel.Registrations);
		}

		[HttpGet]
		[EnableQuery]
		// GET api/<controller>
		public IHttpActionResult Get([FromODataUri]int threadId, [FromODataUri]string participant)
		{
			return Ok(DataModel.Registrations.FirstOrDefault(p => p.ThreadId == threadId && p.ParticipantLogin == participant));
		}
	}
}