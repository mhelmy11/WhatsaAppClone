using System;

namespace MongoDBDemo.Models;

public class AveragePriceResponseDTO
{

    public string Category { get; set; } = null!;

    public decimal AveragePrice { get; set; }

}
