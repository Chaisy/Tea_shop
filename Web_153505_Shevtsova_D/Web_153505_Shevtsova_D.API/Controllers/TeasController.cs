using System;
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

namespace Web_153505_Shevtsova_D.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeasController : ControllerBase
    {
        private readonly IProductService _service;
        private readonly IWebHostEnvironment env;
        private readonly IConfiguration conf;
        private readonly string appUrl;

        public TeasController(IProductService service, IWebHostEnvironment env, IConfiguration conf)
        {
            _service = service;
            this.env = env;
            this.conf = conf;
            appUrl = conf.GetSection("AppUrl").Value!;
        }

        // GET: api/Teas
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

        // GET: api/Teas/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Tea>> GetTea(int id)
        {
            var response = await _service.GetProductByIdAsync(id);
            if (!response.Success)
            {
                return NotFound();
            }
            var tea = response.Data;

            if (tea == null)
            {
                return NotFound();
            }


            return Ok(response);
        }

        // PUT: api/Teas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<ResponseData<Tea>>> PutTea(int id, Tea tea)
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
                if (!TeaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // POST: api/Teas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Tea>> PostTea(Tea tea)
        {
           await _service.CreateProductAsync(tea);
            return CreatedAtAction("GetTea", new { id = tea.Id }, tea);
        }

        // DELETE: api/Tea/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteTea(int id)
        {
            await _service.DeleteProductAsync(id);

            return NoContent();
        }

        private bool TeaExists(int id)
        {
            var response =  _service.GetProductByIdAsync(id).Result;
            if (!response.Success || response.Data == null)
            {
                return false;
            }

            return true;
        }

        // POST: api/Teas/5
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
