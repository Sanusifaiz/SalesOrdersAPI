using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesOrders.Domains.DTOs;
using SalesOrders.Domains.Models;
using SalesOrders.Services.IServices;
using SalesOrdersAPI.Validations;

namespace SalesOrdersAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesOrderController : ControllerBase
    {
        private readonly ISalesOrderService _salesOrderService;

        public SalesOrderController(ISalesOrderService salesOrderService)
        {
            _salesOrderService = salesOrderService;
        }

        /// <summary>
        /// Get Sales Orders List Endpoint
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetSalesOrders()
        {
            var salesOrders = await _salesOrderService.GetSalesOrders();
            return Ok(salesOrders);
        }

        /// <summary>
        /// Get Sales Orders By Id Endpoint
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSalesOrderById(int id)
        {
            var salesOrder = await _salesOrderService.GetSalesOrderByIdAsync(id);
            if (salesOrder == null)
            {
                return NotFound();
            }

            return Ok(salesOrder);
        }

        /// <summary>
        /// Create Sales Orders Endpoint
        /// </summary>
        /// <param name="salesOrder"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateSalesOrder(CreateSalesOrderDTO salesOrder)
        {
            //validate request
            var validator = new CreateSalesOrderRequestValidator().Validate(salesOrder);
            if (!validator.IsValid)
            {
                return BadRequest(ResponseMessage<string>.Fail(validator.Errors.First().ErrorMessage));
            }

            var createdSalesOrder = await _salesOrderService.CreateSalesOrder(salesOrder.ProductId, salesOrder.CustomerId, salesOrder.Quantity);
            if (createdSalesOrder.Error)
            {
                return BadRequest(createdSalesOrder);
            }
            return StatusCode(201, createdSalesOrder);
        }

        /// <summary>
        /// Delete Sales Order Endpoint
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSalesOrder(int id)
        {
            await _salesOrderService.DeleteSalesOrder(id);
            return Ok();
        }
    }
}
