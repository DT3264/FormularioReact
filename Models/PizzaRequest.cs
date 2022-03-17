public class PizzaRequest
    {
        public int id { get; set; }
        public string? name { get; set; }
        public int? sauce { get; set; }
        public int[]? toppings { get; set; }
    }