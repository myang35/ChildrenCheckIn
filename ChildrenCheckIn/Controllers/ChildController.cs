using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChildrenCheckIn.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChildrenCheckIn.Controllers
{
    [Route("api/[controller]")]
    public class ChildController : Controller
    {
        private readonly ChildContext _context;
        private readonly ILogger _logger;

        public ChildController(ChildContext context, ILogger<ChildController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Child> Get()
        {
            return _context.Children.ToArray();
        }

        [HttpGet("{id}")]
        public ActionResult<Child> Get(int id)
        {
            var child = _context.Children.FirstOrDefault(c => c.ID == id);
            if (child == null)
            {
                return NotFound();
            }
            return Ok(child);
        }

        [HttpPost]
        public async Task<ActionResult<Child>> Post([FromBody] Child child)
        {
            _context.Children.Add(child);
            await _context.SaveChangesAsync();
            return CreatedAtAction("Get", child);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Child>> Put(int id, [FromBody] Child child)
        {
            if (child.ID != id)
            {
                return BadRequest();
            }

            _context.Entry(child).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException)
            {
                if (!_context.Children.Any(c => c.ID == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(child);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Child>> Delete(int id)
        {
            var child = await _context.Children.FindAsync(id);
            if (child == null)
            {
                return NotFound();
            }

            _context.Children.Remove(child);
            await _context.SaveChangesAsync();

            return Ok(child);
        }

        [HttpPost("CheckIn/{id}")]
        public async Task<ActionResult<Child>> CheckIn(int id)
        {
            var child = await _context.Children.FindAsync(id);
            if (child == null)
            {
                return NotFound();
            }

            if (child.CheckedIn == true)
            {
                return BadRequest();
            }

            child.CheckedIn = true;
            await _context.SaveChangesAsync();

            _logger.LogInformation("Checked in child \"{name}\" with id {id}", child.Name, child.ID);
            return Ok(child);
        }

        [HttpPost("CheckOut/{id}")]
        public async Task<ActionResult<Child>> CheckOut(int id)
        {
            var child = await _context.Children.FindAsync(id);
            if (child == null)
            {
                return NotFound();
            }

            if (child.CheckedIn == false)
            {
                return BadRequest();
            }

            child.CheckedIn = false;
            await _context.SaveChangesAsync();

            _logger.LogInformation("Checked out child \"{name}\" with id {id}", child.Name, child.ID);
            return Ok(child);
        }
    }
}
