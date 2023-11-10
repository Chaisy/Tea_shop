namespace Web_153505_Shevtsova_D.Extensions
{
    public static class AjaxRequestExtension
    {
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            return request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
    }
}
