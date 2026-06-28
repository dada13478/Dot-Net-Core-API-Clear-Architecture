using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Product
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "商品名稱不能為空")]
    [StringLength(100, ErrorMessage = "商品名稱不能超過100字")]
    public string Name { get; set; }

    [Range(1, 999999, ErrorMessage = "價格必須在 1 ~ 999999 之間")]
    public int Price { get; set; }
}