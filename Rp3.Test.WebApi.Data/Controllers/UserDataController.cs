using Rp3.Test.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Rp3.Test.WebApi.Data.Controllers
{
    public class UserDataController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetByUserPass(string user = null, string pass = null)
        {
            if (string.IsNullOrEmpty(user))
                return BadRequest("User not null");

            if (string.IsNullOrEmpty(pass))
                return BadRequest("Pass not null");

            List<Rp3.Test.Common.Models.User> commonModel = new List<Common.Models.User>();

            using (DataService service = new DataService())
            {
                var query = service.Users.GetQueryable();

                query = query.Where(p => p.Name == user && p.Password == pass && p.Active == true);
                commonModel = query.Select(p => new Common.Models.User()
                {
                    UserId = p.UserId,
                    Name = p.Name,
                    Password = p.Password,
                    Active = p.Active,
                }).ToList();

                if (commonModel.Count() == 0)
                    return BadRequest("No user");
            }

            //return Ok(commonModel[0]);
            return Ok(true);
        }
    }
}