namespace ReviewApi.Models;

public record Product(
    int Id,
    string Name,
    double Price,
    int Stock
);
