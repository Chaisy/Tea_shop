
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
    public class TeaBasesCategoriesController : ControllerBase
    {
        private readonly ICategoryService _service;

        public TeaBasesCategoriesController(ICategoryService svc)
        {
            _service = svc;
        }

        // GET: api/TeaBasesCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeaBasesCategory>>> GetTeaBases()
        {
            var categories = (await _service.GetCategoryListAsync());
          if (categories == null)
          {
              return NotFound();
          }
            return Ok(categories);
        }

        // GET: api/TeaBasesCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TeaBasesCategory>> GetTeaBasesCategory(int id)
        {
            var categories = (await _service.GetCategoryListAsync()).Data;
            if (categories == null)
          {
              return NotFound();
          }
            var teaBasesCategory = categories.Where(c => c.Id == id).FirstOrDefault();

            if (teaBasesCategory == null)
            {
                return NotFound();
            }

            return teaBasesCategory;
        }
    }
}
