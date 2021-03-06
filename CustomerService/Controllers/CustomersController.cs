using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Api;
using CustomerService.ActionFilters;
using CustomerService.Model.Dtos.Requests;
using CustomerService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CustomerService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ApiController
    {
        private readonly ICustomersService _customersService;

        public CustomersController(ICustomersService customersService)
        {
            _customersService = customersService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(){
            var customers = await _customersService.GetAll();
            return ApiResponse(customers);
        }

        [HttpGet("Search/{id}")]
        
        [TokenAuthentication("Admin","Customer")]
        //[SelfAuthentication]
        public async Task<IActionResult> Get(Guid id){
            var customer = await _customersService.GetById(id);
            return ApiResponse(customer);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCustomerRequest request){
            var result = await _customersService.Create(request);
            return ApiResponse(result);
        }

        [HttpPut("Update/{id}")]
        //[SelfAuthentication]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCustomerRequest request){
            var result = await _customersService.Update(id,request);
            return ApiResponse(result);
        }

        [HttpDelete("Delete/{id}")]
        //[TokenAuthentication("Admin")]
        public async Task<IActionResult> Delete(Guid id){
            var result = await _customersService.Delete(id);
            return ApiResponse(result);
        }

        [HttpGet("GetAllBySoftDeleted")]
        [TokenAuthentication("Admin")]
        public async Task<IActionResult> GetAllBySoftDeleted(){
            var customers = await _customersService.GetAllBySoftDeleted();
            return ApiResponse(customers);
        }

        [HttpPost("CreateMany")]
        [TokenAuthentication("Admin")]
        public async Task<IActionResult> CreateMany(List<CreateCustomerRequest> requests){
            var customers = await _customersService.CreateMany(requests);
            return ApiResponse(customers);
        }

        [HttpPut("SoftDelete/{id}")]
        [SelfAuthentication]
        public async Task<IActionResult> SetSoftDeleteTrue(Guid id){
            var result = await _customersService.SoftDelete(id);
            return ApiResponse(result);
        }

        [HttpGet("Validate/{id}")]
        public async Task<IActionResult> Validate(Guid id){
            var result = await _customersService.Validate(id);
            return ApiResponse(result);
        }

        [HttpGet("Page/")]
        public async Task<IActionResult> Page([FromQuery(Name = "from")] int from, int size){
            var customers = await _customersService.Page(from,size);
            return ApiResponse(customers);
        }

    }
}