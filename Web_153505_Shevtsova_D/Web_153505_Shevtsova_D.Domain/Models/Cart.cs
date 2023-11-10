using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_153505_Shevtsova_D.Domain.Entities;
namespace Web_153505_Shevtsova_D.Domain.Models
{
    public class Cart
    {
        /// <summary>
        /// Список объектов в корзине
        /// key - идентификатор объекта
        /// </summary>
        public Dictionary<int, CartItem> CartItems { get; set; } = new();

        /// <summary>
        /// Добавить объект в корзину
        /// </summary>
        /// <param name="dish">Добавляемый объект</param>
        public virtual void AddToCart(Tea tea)
        {
            // добавляем элемент в корзину. Если элемент уже есть - просто увеличиваем количество
            if(!CartItems.TryAdd(tea.Id, new CartItem { Tea = tea, Count = 1 }))
                CartItems[tea.Id].Count++;
        }

        /// <summary>
        /// Удалить объект из корзины
        /// </summary>
        /// <param name="id"> id удаляемого объекта</param>
        public virtual void RemoveItems(int id)
        {
            CartItems.Remove(id);
        }

        /// <summary>
        /// Очистить корзину
        /// </summary>
        public virtual void ClearAll()
        { 
            CartItems.Clear();
        }

        /// <summary>
        /// Количество объектов в корзине
        /// </summary>
        public int Count { get => CartItems.Sum(item => item.Value.Count); }

        public int Price { get => CartItems.Sum(item => item.Value.Tea.Price * item.Value.Count); }
    }
}