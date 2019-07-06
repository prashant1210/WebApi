using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EmployeeDataAccess;

namespace WebApi.Controllers
{
    public class EmployeesController : ApiController
    {
        public IEnumerable<EmployeeNew> Get()
        {
            using (AdventureWorks2014Entities entities = new AdventureWorks2014Entities())
            {
                return entities.EmployeeNews.ToList();
            }
        }
        // Only drawback of this method is it returns http 200 even though employee does not exists in database which is not correct as per Rest standard
        //ideally it should return 404 if item not found
        //public EmployeeNew Get(int id)
        //{
        //    using (AdventureWorks2014Entities entities = new AdventureWorks2014Entities())
        //    {
        //        return entities.EmployeeNews.FirstOrDefault(e => e.Id == id);
        //    }

        //}


        public HttpResponseMessage Get(int id)
        {
            using (AdventureWorks2014Entities entities = new AdventureWorks2014Entities())
            {
                var entity =  entities.EmployeeNews.FirstOrDefault(e => e.Id == id);
                if(entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,"Employee with Id = "+id.ToString()+"NotFound");
                }
            }

        }
        // Below method return void and status code is 204i.e: no content
        //public void Post([FromBody] EmployeeNew employee)
        //{
        //    using (AdventureWorks2014Entities entities = new AdventureWorks2014Entities())
        //    {
        //        entities.EmployeeNews.Add(employee);
        //        entities.SaveChanges();
        //    }

        //}

        //below method return httpstatuscode 201 which is correct as per REST method for newly created item in api
        //This method also return
        public HttpResponseMessage Post([FromBody] EmployeeNew employee)
        {
            try { 
            using (AdventureWorks2014Entities entities = new AdventureWorks2014Entities())
            {
                entities.EmployeeNews.Add(employee);
                entities.SaveChanges();
                var message = Request.CreateResponse(HttpStatusCode.Created, employee);
                message.Headers.Location = new Uri(Request.RequestUri +@"\"+ employee.Id.ToString());
                return message;
            }
            }
            catch(Exception ex)
            {
               return Request.CreateErrorResponse(HttpStatusCode.BadGateway,ex);
            }

        }

        //Implementing Delete method
        public HttpResponseMessage Delete(int id)
        {
            try {
            using (AdventureWorks2014Entities entities = new AdventureWorks2014Entities())
            {
                var entity = entities.EmployeeNews.FirstOrDefault(e => e.Id == id);
                if (entity == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with Id = " + id.ToString() + "NotFound");
                }
                else
                {
                    entities.EmployeeNews.Remove(entity);
                    entities.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Put(int id,[FromBody]EmployeeNew employee)
        {
            try
            {
                using (AdventureWorks2014Entities entities = new AdventureWorks2014Entities())
                {
                    var entity = entities.EmployeeNews.FirstOrDefault(e => e.Id == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with Id = " + id.ToString() + "NotFound to update");

                    }
                    else
                    {
                        entity.Name = employee.Name;
                        entity.Gender = employee.Gender;
                        entity.City = employee.City;
                        entity.DateOfBirth = employee.DateOfBirth;
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }

                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
