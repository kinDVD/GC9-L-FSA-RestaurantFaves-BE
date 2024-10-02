using LabRestaurantFaves.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LabRestaurantFaves.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private RestaurantFavesContext _ordersContext = new RestaurantFavesContext();

        [HttpGet()]
        public IActionResult getAll(string? restaurant = null, bool? orderAgain = null)
        {
            //no filter
            if (restaurant == null && orderAgain == null)
            {
                List<Orders> result = _ordersContext.Orders.ToList();
                return Ok(result);
            }
            //by restaurant
            else if (restaurant != null && orderAgain == null)
            {
                List<Orders> result = _ordersContext.Orders.Where(o => o.Restaurant == restaurant).ToList();
                return Ok(result);
            }
            //by orderAgain
            else if (restaurant == null && orderAgain != null)
            {
                List<Orders> result = _ordersContext.Orders.Where(o => o.OrderAgain == orderAgain).ToList();
                return Ok(result);
            }
            //by restaurant and orderAgain
            else
            {
                List<Orders> result = _ordersContext.Orders.Where(o => o.OrderAgain == orderAgain && o.Restaurant == restaurant).ToList();
                return Ok(result);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            Orders result = _ordersContext.Orders.FirstOrDefault(o => o.Id == id);
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpPost]
        public IActionResult AddOrder([FromBody] Orders newOrder)
        {
            newOrder.Id = 0;
            _ordersContext.Orders.Add(newOrder);
            _ordersContext.SaveChanges();
            return Created($"/api/Orders/{newOrder.Id}", newOrder);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateById([FromBody] Orders updatedOrder, int id)
        {
            if (id != updatedOrder.Id)
            {
                return BadRequest();
            }
            else if (!_ordersContext.Orders.Any(o => o.Id == id))
            {
                return NotFound();
            }
            else
            {
                _ordersContext.Orders.Update(updatedOrder);
                _ordersContext.SaveChanges();
                return NoContent();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteById(int id)
        {
            Orders result = _ordersContext.Orders.FirstOrDefault(o => o.Id == id);
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                _ordersContext.Orders.Remove(result);
                _ordersContext.SaveChanges();
                return NoContent();
            }
        }
    }
}
