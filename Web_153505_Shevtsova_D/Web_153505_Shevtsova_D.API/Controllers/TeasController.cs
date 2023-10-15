/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_153505_Shevtsova_D.API.Data;
using Web_153505_Shevtsova_D.Domain.Entities;

namespace Web_153505_Shevtsova_D.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TeasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Teas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tea>>> Getteas()
        {
          if (_context.teas == null)
          {
              return NotFound();
          }

          var res = await _context.teas.Include(t => t.Category).ToListAsync();
          return res;
        }

        // GET: api/Teas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tea>> GetTea(int id)
        {
          if (_context.teas == null)
          {
              return NotFound();
          }
            var tea = await _context.teas.FindAsync(id);

            if (tea == null)
            {
                return NotFound();
            }

            return tea;
        }

        // PUT: api/Teas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTea(int id, Tea tea)
        {
            if (id != tea.Id)
            {
                return BadRequest();
            }

            _context.Entry(tea).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Teas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tea>> PostTea(Tea tea)
        {
          if (_context.teas == null)
          {
              return Problem("Entity set 'AppDbContext.teas'  is null.");
          }
            _context.teas.Add(tea);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTea", new { id = tea.Id }, tea);
        }

        // DELETE: api/Teas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTea(int id)
        {
            if (_context.teas == null)
            {
                return NotFound();
            }
            var tea = await _context.teas.FindAsync(id);
            if (tea == null)
            {
                return NotFound();
            }

            _context.teas.Remove(tea);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TeaExists(int id)
        {
            return (_context.teas?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
*/

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_153505_Shevtsova_D.API.Data;
using Web_153505_Shevtsova_D.API.Services.ProductService;
using Web_153505_Shevtsova_D.Domain.Entities;
using Web_153505_Shevtsova_D.Domain.Models;

namespace WEB_153501_BYCHKO.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirplanesController : ControllerBase
    {
        private readonly IProductService _service;
        private readonly IWebHostEnvironment env;
        private readonly IConfiguration conf;
        private readonly string appUrl;

        public AirplanesController(IProductService service, IWebHostEnvironment env, IConfiguration conf)
        {
            _service = service;
            this.env = env;
            this.conf = conf;
            appUrl = conf.GetSection("AppUrl").Value!;
        }

        // GET: api/Airplanes
        [HttpGet]
        [Route("")]
        [Route("{category}/pageno{pageno:int}/pagesize{pagesize:int}")]
        [Route("{category}/pageno{pageno:int}")]
        [Route("{category}/pagesize{pagesize:int}")]
        [Route("pageno{pageno:int}/pagesize{pagesize:int}")]
        [Route("pageno{pageno:int}")]
        [Route("{category}")]
        public async Task<ActionResult<IEnumerable<Tea>>> Getteas(string? category = null, int pageNo = 1, int pageSize = 3)
        {
            var responde = await _service.GetProductListAsync(category, pageNo, pageSize);
            if (!responde.Success)
            {
                return NotFound();
            }

            return Ok(responde);

        }

        // GET: api/Airplanes/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Tea>> GetTea(int id)
        {
            var response = await _service.GetProductByIdAsync(id);
            if (!response.Success)
            {
                return NotFound();
            }
            var airplane = response.Data;

            if (airplane == null)
            {
                return NotFound();
            }

            //return airplane;
            return Ok(response);
        }

        // PUT: api/Airplanes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<ResponseData<Tea>>> PutAirplane(int id, Tea tea)
        {
            if (id != tea.Id)
            {
                return BadRequest();
            }            

            try
            {
                await _service.UpdateProductAsync(id, tea);
                return Ok(tea);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AirplaneExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // POST: api/Airplanes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Tea>> PostAirplane(Tea tea)
        {
           await _service.CreateProductAsync(tea);
            return CreatedAtAction("GetTea", new { id = tea.Id }, tea);
        }

        // DELETE: api/Airplanes/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteAirplane(int id)
        {
            await _service.DeleteProductAsync(id);

            return NoContent();
        }

        private bool AirplaneExists(int id)
        {
            var response =  _service.GetProductByIdAsync(id).Result;
            if (!response.Success || response.Data == null)
            {
                return false;
            }

            return true;
        }

        // POST: api/Airplanes/5
        [HttpPost("{id}")]
        [Authorize]
        public async Task<ActionResult<ResponseData<string>>> PostImage(
                                                                int id,
                                                                IFormFile formFile)
        {
            var response = await _service.SaveImageAsync(id, formFile);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

    }
}
