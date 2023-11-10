using Moq;
using Web_153505_Shevtsova_D.Domain.Entities;
using Web_153505_Shevtsova_D.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Web_153505_Shevtsova_D.API.Data;
using Microsoft.AspNetCore.Hosting;
using Web_153505_Shevtsova_D.API.Services.ProductService;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using Web_153505_Shevtsova_D.API.Models;

namespace Web_153505_Shevtsova_D.Tests
{
    public class ProductServiceTests
    {
        private readonly DbConnection _connection;
        private readonly DbContextOptions<AppDbContext> _contextOptions;
        private readonly Mock<IWebHostEnvironment> env;
        private readonly Mock<IHttpContextAccessor> accessor;

        private int maxPageSize = 20;

        public ProductServiceTests()
        {
            env = new();
            accessor = new();

            // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
            // at the end of the test (see Dispose below).
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            // These options will be used by the context instances in this test suite, including the connection opened above.
            _contextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(_connection)
                .Options;

            using var context = new AppDbContext(_contextOptions);
            context.Database.EnsureCreated();

            context.basesType.AddRange(
                new TeaBasesCategory
                {
                    Id = 1,
                    Name = "category1",
                    NormalizedName = "cat1"
                },
                new TeaBasesCategory
                {
                    Id = 2,
                    Name = "category2",
                    NormalizedName = "cat2"
                });
            context.teas.AddRange(
                new Tea
                {
                    Id = 1,
                    Name = "tea1",
                    Description = "d1",
                    PhotoPath = "path1",
                    Price = 1,
                    MIMEType = "type",
                    Category = context.basesType.Find(1)
                },
                new Tea
                {
                    Id = 2,
                    Name = "tea2",
                    Description = "d2",
                    PhotoPath = "path2",
                    Price = 2,
                    MIMEType = "type",
                    Category = context.basesType.Find(2)
                },
                new Tea
                {
                    Id = 3,
                    Name = "tea3",
                    Description = "d3",
                    PhotoPath = "path3",
                    Price = 3,
                    MIMEType = "type",
                    Category = context.basesType.Find(2)
                },
                new Tea
                {
                    Id = 4,
                    Name = "tea4",
                    Description = "d4",
                    PhotoPath = "path4",
                    Price = 4,
                    MIMEType = "type",
                    Category = context.basesType.Find(1)
                }
                );
            context.SaveChanges();

        }

        public void Dispose() => _connection.Dispose();

        AppDbContext GetContext() => new AppDbContext(_contextOptions);

        [Fact]
        public void GetProductListAsync_Default_ThreeObject_CorrectPageCountCalculation()
        {
            Mock<IOptions<ConfigData>> opt = new();
            opt.Setup(o => o.Value).Returns(new ConfigData { MaxPageSize=20});
            var context = GetContext();
            var productService = new ProductService(context, env.Object, accessor.Object, opt.Object);

            var result = productService.GetProductListAsync(null).Result;
            Assert.IsType<ResponseData<ListModel<Tea>>>(result);
            Assert.True(result.Success);
            Assert.Equal(1, result.Data.CurrentPage);
            Assert.Equal(3, result.Data.Items.Count);
            Assert.Equal(2, result.Data.TotalPages);
            Assert.Equal(context.teas.First(), result.Data.Items[0]);
        }

        [Theory]
        [InlineData(2)]
        public void GetProductListAsync_ReturnsRightPage(int pageno)
        {
            Mock<IOptions<ConfigData>> opt = new();
            opt.Setup(o => o.Value).Returns(new ConfigData { MaxPageSize=20});
            var context = GetContext();
            var productService = new ProductService(context, env.Object, accessor.Object, opt.Object);

            var result = productService.GetProductListAsync(null, pageno).Result;
            Assert.IsType<ResponseData<ListModel<Tea>>>(result);
            Assert.True(result.Success);
            Assert.Equal(2, result.Data.CurrentPage);
            Assert.Equal(1, result.Data.Items.Count);
            Assert.Equal(2, result.Data.TotalPages);
        }

        [Theory]
        [InlineData("cat1")]
        public void GetProductListAsync_ReturnsRightElements(string category)
        {
            Mock<IOptions<ConfigData>> opt = new();
            opt.Setup(o => o.Value).Returns(new ConfigData { MaxPageSize=20});
            var context = GetContext();
            var productService = new ProductService(context, env.Object, accessor.Object, opt.Object);

            List<Tea> requiredResult = new() { context.teas.Find(1), context.teas.Find(4) };

            var result = productService.GetProductListAsync(category).Result;
            Assert.IsType<ResponseData<ListModel<Tea>>>(result);
            Assert.True(result.Success);
            Assert.Equal(1, result.Data.CurrentPage);
            Assert.Equal(2, result.Data.Items.Count);
            Assert.Equal(1, result.Data.TotalPages);
            Assert.Equal(requiredResult, result.Data.Items);
        }

        [Fact]
        public void GetProductListAsync_CorrectMaxPageSize()
        {
            Mock<IOptions<ConfigData>> opt = new();
            opt.Setup(o => o.Value).Returns(new ConfigData { MaxPageSize=20});
            var context = GetContext();
            var productService = new ProductService(context, env.Object, accessor.Object, opt.Object);


            var result = productService.GetProductListAsync(null, 1, pageSize: 21).Result;
            Assert.IsType<ResponseData<ListModel<Tea>>>(result);
            Assert.True(result.Success);

            Assert.True(result.Data.Items.Count <= maxPageSize);
        }

        [Fact]
        public void GetProductListAsync_Pageno_GT_PageCount_ReturnsFalse()
        {
            Mock<IOptions<ConfigData>> opt = new();
            opt.Setup(o => o.Value).Returns(new ConfigData { MaxPageSize=20});
            var context = GetContext();
            var productService = new ProductService(context, env.Object, accessor.Object, opt.Object);

            var result = productService.GetProductListAsync(null, 3).Result;
            Assert.IsType<ResponseData<ListModel<Tea>>>(result);
            Assert.True(!result.Success);
        }

    }
}