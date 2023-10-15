using Microsoft.EntityFrameworkCore;
using Web_153505_Shevtsova_D.Domain.Entities;

namespace Web_153505_Shevtsova_D.API.Data
{
    public class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {
            // Получение контекста БД
            using var scope = app.Services.CreateScope();

            var context =
            scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Выполнение миграций
            await context.Database.MigrateAsync();

            var appUrl = app.Configuration["AppUrl"];

            //если категории в бд пусты, создаем их
            if (context.basesType.Count() == 0)
            {
                context.basesType.AddRange
                    (
                        new TeaBasesCategory { Name = "Leaves", NormalizedName = "listia" },
                        new TeaBasesCategory { Name = "Roots", NormalizedName = "korni" },
                        new TeaBasesCategory { Name = "Flowers", NormalizedName = "tsveti" },
                        new TeaBasesCategory { Name = "турбовентиляторный", NormalizedName = "kora" },
                        new TeaBasesCategory{Name = "Fruits", NormalizedName= "frukti"}
                    );
            }

            context.SaveChanges();

            //если продукты в бд пусты, создаем их
            if (context.teas.Count() == 0)
            {
                context.teas.AddRange
                    (
                        new Tea
                        {
                            Name = "Taiga glade",
                            Description = "hibiscus, apple pieces, pine cones, juniper berries, blackberries, red currants, " +
                                "blueberries, cornflower petals, aroma of grandma's jam",
                            Category = context.basesType.Where(c => c.NormalizedName.Equals("frukti")).First()!,
                            Price = 15,
                            PhotoPath = $"{appUrl}Images/taiga.jpeg",
                        },
                        new Tea
                        {
                            Name = "Lavender apple",
                            Description = "Pieces of apples, linden, lemon balm, blackberry leaves, lavender, chamomile.",
                            Category = context.basesType.Where(c => c.NormalizedName.Equals("listia")).First()!,
                            Price = 17,
                            PhotoPath = $"{appUrl}Images/lavenderapple.jpeg",
                        },
                        new Tea
                        {
                            Name = "Lapacho",
                            Description = "tincture of Lapacho tree bark, sugar, honey",
                            Category = context.basesType.Where(c => c.NormalizedName.Equals("kora")).First()!,
                            Price = 20,
                            PhotoPath = $"{appUrl}Images/lapacho.jpeg",
                        },
                        new Tea
                        {
                            Name = "Peach Tea",
                            Description = "peaches, herbal lemon balm, granulated sugar or a sugar",
                            Category = context.basesType.Where(c => c.NormalizedName.Equals("frukti")).First()!,
                            Price = 15,
                            PhotoPath = $"{appUrl}Images/peachtea.jpeg",
                        },
                         new Tea
                         {
                             Name = "Matcha",
                             Description = "Japanese green tea powder",
                             Category = context.basesType.Where(c => c.NormalizedName.Equals("listia")).First()!,
                             Price = 22,
                             PhotoPath = $"{appUrl}Images/matcha.jpeg",
                         },
                        new Tea
                        {
                            Name = "Buddha Basket (with marigold flower)",
                            Description = "This bound tea is made from tea leaves bound with calendula flowers. With orange and peach aroma.",
                            Category = context.basesType.Where(c => c.NormalizedName.Equals("tsveti")).First()!,
                            Price = 10,
                            PhotoPath = $"{appUrl}Images/budda.jpeg",
                        },
                        new Tea
                        {
                            Name = "Masala (spiced tea)",
                            Description = "Fennel, cardamom, cinnamon, cumin, ginger, cloves, coriander, white, black and pink pepper",
                            Category = context.basesType.Where(c => c.NormalizedName.Equals("korni")).First()!,
                            Price = 16,
                            PhotoPath = $"{appUrl}Images/masala.jpeg",
                        },
                        new Tea
                        {
                            Name = "Black Tea",
                            Description = "Just leaves",
                            Category = context.basesType.Where(c => c.NormalizedName.Equals("listia")).First()!,
                            Price = 12,
                            PhotoPath = $"{appUrl}Images/blacktea.jpeg",
                        }
                    );
            }

            context.SaveChanges();
        }
    }
}

