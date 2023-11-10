using System.Text.Json.Serialization;
using Web_153505_Shevtsova_D.Domain.Entities;
using Web_153505_Shevtsova_D.Domain.Models;
using Web_153505_Shevtsova_D.Extensions;

namespace Web_153505_Shevtsova_D.Models
{
    public class SessionCart : Cart
    {
        public static Cart GetCart(IServiceProvider services)
        {
            ISession? session = services.GetRequiredService<IHttpContextAccessor>()
                .HttpContext?.Session;
            SessionCart cart = session?.Get<SessionCart>("Cart") ?? new SessionCart();
            cart.session = session;
            return cart;
        }

        public override void AddToCart(Tea tea)
        {
            base.AddToCart(tea);
            session?.Set<Cart>("Cart", this);
        }

        public override void RemoveItems(int id)
        {
            base.RemoveItems(id);
            session?.Set<Cart>("Cart", this);
        }

        public override void ClearAll()
        {
            base.ClearAll();
            session?.Set<Cart>("Cart", this);
        }

        [JsonIgnore]
        ISession? session;


    }
}