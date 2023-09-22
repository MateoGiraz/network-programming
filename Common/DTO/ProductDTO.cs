namespace Common.DTO;

public class ProductDTO
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Stock { get; set; }
    public string Price { get; set; }
    public string ImageData { get; set; }
    public List<RatingDTO> Ratings { get; set; }
    public UserDTO Owner { get; set; }
}