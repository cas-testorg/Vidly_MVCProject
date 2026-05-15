using AutoMapper;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Http;
using Vidly_MVCProject.Dtos;
using Vidly_MVCProject.Models;

namespace Vidly_MVCProject.Controllers.Api
{
    public class CustomersController : ApiController
    {
        private AppDbContext _appDbContext;
        private IMapper _mapper;

        public CustomersController()
        {
            _appDbContext = new AppDbContext();
            _mapper = MvcApplication.Mapper;
        }

        //GET /api/customers
        [HttpGet]
        public IHttpActionResult GetCustomers(string query = null)
        {
            var customersQuery = _appDbContext.Customers
                .Include(c => c.MembershipType);

            if (!String.IsNullOrWhiteSpace(query))
                customersQuery = customersQuery.Where(c => c.Name.Contains(query));

            var customerDtos = customersQuery
                .ToList()
                .Select(_mapper.Map<Customer, CustomerDto>);

            return Ok(customerDtos);
        }

        //GET /api/customers/1
        [HttpGet]
        public IHttpActionResult GetCustomer(int id)
        {
            var customer = _appDbContext.Customers.SingleOrDefault(c => c.Id == id);

            if (customer == null)
                return NotFound();

            return Ok(_mapper.Map<Customer, CustomerDto>(customer));
        }

        //POST /api/customers
        [HttpPost]
        public IHttpActionResult CreateCustomer(CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var customer = _mapper.Map<CustomerDto, Customer>(customerDto);
            _appDbContext.Customers.Add(customer);
            _appDbContext.SaveChanges();

            customerDto.Id = customer.Id;

            return Created(new Uri(Request.RequestUri + "/" + customer.Id), customerDto);
        }

        //PUT /api/customers/1
        [HttpPut]
        public void UpdateCustomer(int id, CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var customerInDb = _appDbContext.Customers.SingleOrDefault(c => c.Id == id);

            if (customerInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            _mapper.Map(customerDto, customerInDb);

            _appDbContext.SaveChanges();
        }

        //DELETE /api/customers/1
        [HttpDelete]
        public void DeleteCustomer(int id)
        {
            var customerInDb = _appDbContext.Customers.SingleOrDefault(c => c.Id == id);

            if (customerInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            _appDbContext.Customers.Remove(customerInDb);
            _appDbContext.SaveChanges();
        }
    }
}