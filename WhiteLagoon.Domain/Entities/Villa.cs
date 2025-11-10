using System.ComponentModel.DataAnnotations;

namespace WhiteLagoon.Domain.Entities;

public class Villa
{
    public int Id { get; set; }
    [MaxLength(50)]
    public required string Name { get; set; }
    public string? Description { get; set; }
    
    [Range(10,10000)]
    
    [Display(Name = "Price per night")]
    public double Price { get; set; }   
    public int Sqft { get; set; }
    
    [Range(1,100)] 
    public int Occupancy { get; set; }
    
    [Display(Name = "Image Url")]
    public string? ImageUrl { get; set; }
    
    public DateTime? CreatedDate { get; set; }
    
    public DateTime? UpdatedDate { get; set; }
}