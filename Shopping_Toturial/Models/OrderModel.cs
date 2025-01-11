namespace Shopping_Toturial.Models;

public class OrderModel
{
    public int Id { get; set; }
    public string OrderCode { get; set; }
    public string Username { get; set; }
    public DateTime OrderDate { get; set; }
    public int status { get; set; } // trang thai thanh toan
}