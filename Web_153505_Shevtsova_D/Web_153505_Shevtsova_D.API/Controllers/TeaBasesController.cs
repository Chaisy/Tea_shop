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
    public class TeaBasesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TeaBasesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/TeaBases
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tea>>> Getteas()
        {
          if (_context.teas == null)
          {
              return NotFound();
          }
            return await _context.teas.ToListAsync();
        }

        // GET: api/TeaBases/5
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

        // PUT: api/TeaBases/5
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

        // POST: api/TeaBases
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

        // DELETE: api/TeaBases/5
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_153505_Shevtsova_D.API.Data;
using Web_153505_Shevtsova_D.API.Services.CategoryService;
using Web_153505_Shevtsova_D.Domain.Entities;

namespace Web_153505_Shevtsova_D.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EngineTypeCategoriesController : ControllerBase
    {
        private readonly ICategoryService _service;

        public EngineTypeCategoriesController(ICategoryService svc)
        {
            _service = svc;
        }

        // GET: api/EngineTypeCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeaBasesCategory>>> GetEngineTypes()
        {
            var categories = (await _service.GetCategoryListAsync());
          if (categories == null)
          {
              return NotFound();
          }
            return Ok(categories);
        }

        // GET: api/EngineTypeCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TeaBasesCategory>> GetEngineTypeCategory(int id)
        {
            var categories = (await _service.GetCategoryListAsync()).Data;
            if (categories == null)
          {
              return NotFound();
          }
            var engineTypeCategory = categories.Where(c => c.Id == id).FirstOrDefault();

            if (engineTypeCategory == null)
            {
                return NotFound();
            }

            return engineTypeCategory;
        }

        //// PUT: api/EngineTypeCategories/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutEngineTypeCategory(int id, EngineTypeCategory engineTypeCategory)
        //{
        //    if (id != engineTypeCategory.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _service.Entry(engineTypeCategory).State = EntityState.Modified;

        //    try
        //    {
        //        await _service.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!EngineTypeCategoryExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/EngineTypeCategories
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<EngineTypeCategory>> PostEngineTypeCategory(EngineTypeCategory engineTypeCategory)
        //{
        //  if (_service.engineTypes == null)
        //  {
        //      return Problem("Entity set 'AppDbContext.engineTypes'  is null.");
        //  }
        //    _service.engineTypes.Add(engineTypeCategory);
        //    await _service.SaveChangesAsync();

        //    return CreatedAtAction("GetEngineTypeCategory", new { id = engineTypeCategory.Id }, engineTypeCategory);
        //}

        //// DELETE: api/EngineTypeCategories/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteEngineTypeCategory(int id)
        //{
        //    if (_service.engineTypes == null)
        //    {
        //        return NotFound();
        //    }
        //    var engineTypeCategory = await _service.engineTypes.FindAsync(id);
        //    if (engineTypeCategory == null)
        //    {
        //        return NotFound();
        //    }

        //    _service.engineTypes.Remove(engineTypeCategory);
        //    await _service.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool EngineTypeCategoryExists(int id)
        //{
        //    return (_service.engineTypes?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}
