namespace Web_153505_Shevtsova_D.Models;

public class ErrorViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
public class ListDemo
{
    public int Id { get; set; }
    public string Name { get; set; }
}