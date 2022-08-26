namespace Shopping.Aggregator.Models
{
    public class BasketModel
    {
        public string UserName { get; set; }
        public List<BasketItemsExtendedModel> Items { get; set; } = new List<BasketItemsExtendedModel>();
        public decimal TotalPrice { get; set; }
    }
}
