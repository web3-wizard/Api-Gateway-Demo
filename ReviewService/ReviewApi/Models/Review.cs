namespace ReviewApi.Models;

public record Review(
    int Id,
    int ProductId,
    int Rating,
    string Comment
);
