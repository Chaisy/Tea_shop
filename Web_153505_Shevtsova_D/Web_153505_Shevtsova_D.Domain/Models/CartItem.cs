using Web_153505_Shevtsova_D.Domain.Entities;

namespace Web_153505_Shevtsova_D.Domain.Models
{
    public class CartItem
    {
        public int Count { get; set; }

        public Tea Tea { get; set; }
    }
}