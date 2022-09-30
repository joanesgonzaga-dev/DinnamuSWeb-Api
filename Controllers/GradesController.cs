using DinnamuSWebApi.Models;
using DinnamuSWebApi.Repositories.Grades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DinnamuSWebApi.Controllers
{
    public class GradesController : ApiController
    {
        IGradeRepository _repository;

        public GradesController(IGradeRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("api/grades")]
        public IHttpActionResult Grades()
        {
            try
            {
                return Json<List<Grade>>(_repository.Get());
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }

    }
}
