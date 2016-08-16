using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DonorGateway.Data;

namespace admin.web.Controllers
{
    [RoutePrefix("api/constituent"), Authorize]
    public class ConstituentController : ApiController
    {

        private readonly DataContext context;

        public ConstituentController()
        {
            context = new DataContext();
        }

        public IHttpActionResult Get()
        {
            var list = context.Constituents.OrderBy(x => x.Id).Skip(0).Take(10);
            return Ok(list);
        }

    }
}
