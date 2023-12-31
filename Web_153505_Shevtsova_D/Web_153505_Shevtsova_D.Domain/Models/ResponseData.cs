using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Web_153505_Shevtsova_D.Domain.Models;

public class ResponseData<T>
{
    // запрашиваемые данные
    public T Data { get; set; }
    // признак успешного завершения запроса
    public bool Success { get; set; } = true;
    // сообщение в случае неуспешного завершения
    public string? ErrorMessage { get; set; }
}
