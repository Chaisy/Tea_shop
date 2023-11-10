using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_153505_Shevtsova_D.Controllers;
using Web_153505_Shevtsova_D.Domain.Entities;
using Web_153505_Shevtsova_D.Domain.Models;
using Web_153505_Shevtsova_D.Services.TeaBasesService;
using Web_153505_Shevtsova_D.Services.ProductService;

namespace Web_153505_Shevtsova_D.Tests
{
    public class TeaProductControllerTests
    {
        Mock<IProductService> productService;
        Mock<ICategoryService> categoryService;
        TeaProductController controller;

        public TeaProductControllerTests()
        {
            productService = new();
            categoryService = new();
            controller = new TeaProductController(productService.Object, categoryService.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
        }

        [Fact]
        public void FailedGetCategoryListReturns404()
        {
            //arrange
            productService.Setup(m => m.GetProductListAsync("a", 1)).Returns(Task.FromResult(new ResponseData<ListModel<Tea>> { Success = false, ErrorMessage = "err" }));

            //act
            var result = controller.Index("a", 1).Result;

            //assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void FailedGetProductListReturns404()
        {
            //arrange
            productService.Setup(m => m.GetProductListAsync("a", 1)).Returns(Task.FromResult(new ResponseData<ListModel<Tea>> { Success = true }));
            categoryService.Setup(m => m.GetCategoryListAsync()).Returns(Task.FromResult(new ResponseData<List<TeaBasesCategory>> { Success = false }));

            //act
            var result = controller.Index("a", 1).Result;

            //assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void ViewDataGotCategotyList()
        {
            //arrange
            productService.Setup(m => m.GetProductListAsync("a", 1)).Returns(Task.FromResult(new ResponseData<ListModel<Tea>> { Success = true }));
            categoryService.Setup(m => m.GetCategoryListAsync()).Returns(Task.FromResult(new ResponseData<List<TeaBasesCategory>> { Success = true, Data = new List<TeaBasesCategory>() }));

            //act
            controller.Index("a", 1);

            var result = controller.ViewBag.categories;

            Assert.IsType<List<TeaBasesCategory>>(result);
        }

        [Theory]
        [InlineData(null, 1)]
        public void ViewDataGotAllCategory(string? category, int pageno)
        {
            //arrange
            productService.Setup(m => m.GetProductListAsync(category, pageno)).Returns(Task.FromResult(new ResponseData<ListModel<Tea>> { Success = true }));
            categoryService.Setup(m => m.GetCategoryListAsync()).Returns(Task.FromResult(new ResponseData<List<TeaBasesCategory>> { Success = true, Data = new List<TeaBasesCategory>() }));

            //act
            controller.Index(category, pageno);
            var result = controller.ViewData["currentCategory"];

            //assert
            Assert.Equal(result, "ALL");
        }

        [Theory]
        [InlineData("a", 1)]
        public void ViewDataGotCurrentCategory(string? category, int pageno)
        {
            //arrange
            productService.Setup(m => m.GetProductListAsync(category, pageno)).Returns(Task.FromResult(new ResponseData<ListModel<Tea>> { Success = true }));
            categoryService.Setup(m => m.GetCategoryListAsync()).Returns(Task.FromResult(new ResponseData<List<TeaBasesCategory>> { Success = true, Data = new List<TeaBasesCategory> { new TeaBasesCategory { Id = 1, Name = "a", NormalizedName = "a" } } }));

            //act
            controller.Index(category, pageno);
            var result = controller.ViewData["currentCategory"];

            //assert
            Assert.Equal(result, category);

        }

        [Fact]
        public void IndexReturnedObjectList()
        {
            //arrange
            productService.Setup(m => m.GetProductListAsync(null, 1)).Returns(Task.FromResult(new ResponseData<ListModel<Tea>> { Success = true, Data = new ListModel<Tea>() }));
            categoryService.Setup(m => m.GetCategoryListAsync()).Returns(Task.FromResult(new ResponseData<List<TeaBasesCategory>> { Success = true, Data = new List<TeaBasesCategory> { new TeaBasesCategory() } }));

            Mock<HttpRequest> request = new Mock<HttpRequest>();
            request.Setup(r => r.Headers["X-Requested-With"]).Returns("XMLHttpRequest");

            TeaProductController controller = new TeaProductController(productService.Object, categoryService.Object) { ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() } };


            //act
            var result = controller.Index(null, 1).Result;

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<ListModel<Tea>>(viewResult.Model);

        }

    }
}