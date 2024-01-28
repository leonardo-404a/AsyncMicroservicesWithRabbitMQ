namespace SharedLib;

public class Order
{
    public Order(string customer, string details)
    {
        Customer = customer;
        Details = details;
    }

    public int Id { get; set; }
    public string Customer { get; set; }
    public string Details { get; set; }
}
